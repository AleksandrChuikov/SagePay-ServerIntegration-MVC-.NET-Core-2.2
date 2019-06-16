using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SagePayServerIntegration.Entities;
using System.Net.Http.Formatting;
using System.Text.RegularExpressions;
using System.Text.Encodings.Web;
using System.Web;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Security.Cryptography;
using SagePayServerIntegration.Services;
using SagePayServerIntegration.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SagePayServerIntegration.Models;

namespace SagePayServerIntegration.Controllers
{
    public class PaymentController : Controller
    {
        public IConfiguration _configuration;
        private IHashService _hashService;
        private IPaymentRepository _invoiceRepository;
        public PaymentController(IConfiguration configuration, IHashService hashService, IPaymentRepository invoiceRepository)
        {
            _configuration = configuration;
            _hashService = hashService;
            _invoiceRepository = invoiceRepository;
        }

        public IActionResult Index()
        {
            var paymentDTO = new PaymentDTO
            {
                Currency = "GBP",
                Amount = 45,
            };
            var countries = _invoiceRepository.GetCountries();
            ViewBag.Countries = new SelectList(countries, "Code", "Name");
            var states = _invoiceRepository.GetStates();
            ViewBag.States = new SelectList(states, "Code", "Name");
            ViewBag.Amount = string.Join(" ", paymentDTO.Currency, paymentDTO.Amount);
            return View(paymentDTO);
        }

        [HttpPost("submited")]
        public async Task<IActionResult> SubmitedPayment(PaymentDTO paymentDTO)
        {
            paymentDTO.Description = "Invoice Description";
            paymentDTO.VendorTxCode = Guid.NewGuid().ToString().ToUpper();
            paymentDTO.NotificationURL = $"{_configuration["AppUrl"]}/Payment/RedirectURL";
            paymentDTO.Vendor = _configuration["Vendor"];
            var client = new HttpClient();
            var data = PostData(paymentDTO);
            var result = await client.PostAsync(_configuration["SagePayUrl"], new FormUrlEncodedContent(data));
            var contentResponse = await result.Content.ReadAsStringAsync();
            if (contentResponse.Contains("Status=OK"))
                return Redirect(await SaveSuccessResponseData(paymentDTO, contentResponse));
            ViewBag.StatusDetail = contentResponse;
            return View("Error");
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            return View("ErrorPage");
        }

        [HttpGet("response/{status}")]
        public IActionResult SagePayResponse(string status)
        {
            if (status == "PENDING")
                return View("Pending");
            if (status == "NOTAUTHED")
                return View("Notauthed");
            if (status == "ABORT")
                return View("Canceled");
            if (status == "REJECTED")
                return View("TamperedTransaction");
            if (status == "ERROR")
                return View("ErrorSagePay");
            return View();
        }

        [HttpPost]
        public string RedirectURL(SagePayPaymentResponse paymentResponse)
        {
            string MD5signature = CreateMD5signature(paymentResponse);
            var hashResult = _hashService.MD5Hash(MD5signature);
            var sagePayPaymentDetail = new SagePayPaymentDetail
            {
                TxAuthNo = paymentResponse.TxAuthNo,
                VPSSignature = paymentResponse.VPSSignature,
                BankAuthCode = paymentResponse.BankAuthCode,
                Status = paymentResponse.Status
            };
            string url = CreateUrl(paymentResponse.Status, paymentResponse.VPSSignature, hashResult, sagePayPaymentDetail);
            _invoiceRepository.UpdatePayment(paymentResponse.VendorTxCode, sagePayPaymentDetail);
            return url;
        }

        private Dictionary<string, string> PostData(PaymentDTO paymentDTO)
        {
            return new Dictionary<string, string>
            {
                { "VPSProtocol", "3.00" },
                { "TxType", "PAYMENT" },
                { "Vendor", _configuration["Vendor"] },
                { "Currency", paymentDTO.Currency },
                { "Amount", paymentDTO.Amount.ToString() },
                { "Description", paymentDTO.Description },
                { "VendorTxCode", paymentDTO.VendorTxCode },
                { "NotificationURL", paymentDTO.NotificationURL},
                { "BillingFirstnames", paymentDTO.BillingFirstnames },
                { "BillingSurname", paymentDTO.BillingSurname },
                { "BillingAddress1", paymentDTO.BillingAddress1 },
                { "BillingAddress2", paymentDTO.BillingAddress2 },
                { "BillingCity", paymentDTO.BillingCity },
                { "BillingPostCode", paymentDTO.BillingPostCode },
                { "BillingCountry", paymentDTO.BillingCountry },
                { "DeliveryFirstnames", paymentDTO.DeliveryFirstnames ?? paymentDTO.BillingFirstnames},
                { "DeliverySurname", paymentDTO.DeliverySurname ?? paymentDTO.BillingSurname},
                { "DeliveryAddress1", paymentDTO.DeliveryAddress1 ?? paymentDTO.BillingAddress1},
                { "DeliveryAddress2", paymentDTO.DeliveryAddress2 ?? paymentDTO.BillingAddress2},
                { "DeliveryCity", paymentDTO.DeliveryCity ?? paymentDTO.BillingCity},
                { "DeliveryPostCode", paymentDTO.DeliveryPostCode ?? paymentDTO.BillingPostCode},
                { "DeliveryCountry", paymentDTO.DeliveryCountry ?? paymentDTO.BillingCountry},
                { "BillingState", paymentDTO.BillingState },
                { "DeliveryState", paymentDTO.DeliveryState ?? paymentDTO.BillingState},
                { "CustomerEMail", paymentDTO.CustomerEMail}
            };
        }

        private async Task<string> SaveSuccessResponseData(PaymentDTO paymentDTO, string contentResponse)
        {
            string securityKey = string.Empty;
            string vPSTxId = string.Empty;
            string nextURL = string.Empty;
            MatchCollection ms = Regex.Matches(contentResponse, @"(www.+|https.+)([\s]|$)");
            nextURL = ms[0].Value.ToString();

            string[] substrings = contentResponse.Split("\r\n");
            if (substrings[3].Contains("VPSTxId"))
            {
                vPSTxId = substrings[3].Split("=")[1];
            }
            if (substrings[4].Contains("SecurityKey"))
            {
                securityKey = substrings[4].Split("=")[1];
            }
            var invoice = new SagePayPaymentDetail
            {
                VendorTxCode = paymentDTO.VendorTxCode,
                SecurityKey = securityKey,
                VPSTxId = vPSTxId,
                BillingFirstnames = paymentDTO.BillingFirstnames,
                BillingSurname = paymentDTO.BillingSurname,
                BillingAddress1 = paymentDTO.BillingAddress1,
                BillingAddress2 = paymentDTO.BillingAddress2,
                BillingCity = paymentDTO.BillingCity,
                BillingPostCode = paymentDTO.BillingPostCode,
                BillingCountry = paymentDTO.BillingCountry,
                DeliveryFirstnames = paymentDTO.DeliveryFirstnames ?? paymentDTO.BillingFirstnames,
                DeliverySurname = paymentDTO.DeliverySurname ?? paymentDTO.BillingSurname,
                DeliveryAddress1 = paymentDTO.DeliveryAddress1 ?? paymentDTO.BillingAddress1,
                DeliveryAddress2 = paymentDTO.DeliveryAddress2 ?? paymentDTO.BillingAddress2,
                DeliveryCity = paymentDTO.DeliveryCity ?? paymentDTO.BillingCity,
                DeliveryPostCode = paymentDTO.DeliveryPostCode ?? paymentDTO.BillingPostCode,
                DeliveryCountry = paymentDTO.DeliveryCountry ?? paymentDTO.BillingCountry,
                DeliveryState = paymentDTO.DeliveryState ?? paymentDTO.BillingState,
                BillingState = paymentDTO.BillingState,
                CustomerEMail = paymentDTO.CustomerEMail
            };
            await _invoiceRepository.Create(invoice);
            return nextURL;
        }

        private string CreateMD5signature(SagePayPaymentResponse paymentResponse)
        {
            var SecurityKey = _invoiceRepository.GetSecurityKey(paymentResponse.VendorTxCode);
            var VendorName = _configuration["Vendor"].ToLower();
            var MD5signature = $@"{paymentResponse.VPSTxId}{paymentResponse.VendorTxCode}{paymentResponse.Status}{paymentResponse.TxAuthNo}{VendorName}{paymentResponse.AVSCV2}{SecurityKey}{paymentResponse.AddressResult}{paymentResponse.PostCodeResult}{paymentResponse.CV2Result}{paymentResponse.GiftAid}NOTCHECKED{paymentResponse.CAVV}{paymentResponse.AddressStatus}{paymentResponse.PayerStatus}{paymentResponse.CardType}{paymentResponse.Last4Digits}{paymentResponse.DeclineCode}{paymentResponse.ExpiryDate}{paymentResponse.FraudResponse}{paymentResponse.BankAuthCode}";
            return MD5signature;
        }

        private string CreateUrl(string status, string VPSSignature, string hashResult, SagePayPaymentDetail paymentDetail)
        {
            string url = string.Empty;
            switch (status)
            {
                case "ABORT":
                    url = $"Status=INVALID\r\nRedirectURL={_configuration["AppUrl"]}/response/{status}";
                    break;
                case "PENDING":
                    url = $"Status=INVALID\r\nRedirectURL={_configuration["AppUrl"]}/response/{status}";
                    break;
                case "NOTAUTHED":
                    url = $"Status=INVALID\r\nRedirectURL={_configuration["AppUrl"]}/response/{status}";
                    break;
                case "Canceled":
                    url = $"Status=INVALID\r\nRedirectURL={_configuration["AppUrl"]}/response/{status}";
                    break;
                case "ERROR":
                    url = $"Status=INVALID\r\nRedirectURL={_configuration["AppUrl"]}/response/{status}";
                    break;
                case "OK":
                    paymentDetail.VPSSignatureServerValue = hashResult;
                    //Validation of the MD5 digital signature that is attached to the message to ensure it has not been tampered with and genuinely comes from Sage Pay
                    if (!VPSSignature.Equals(hashResult))
                    {
                        url = $"Status=INVALID\r\nRedirectURL={_configuration["AppUrl"]}/response/REJECTED";
                        paymentDetail.Status = "REJECTED";
                    }
                    else
                        url = $"Status=OK\r\nRedirectURL={_configuration["AppUrl"]}/response/{status}";
                    break;
            }
            return url;
        }
    }
}