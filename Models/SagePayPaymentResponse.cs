using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagePayServerIntegration.Models
{
    public class SagePayPaymentResponse
    {
        public string Status { get; set; }
        public string StatusDetail { get; set; }
        public string VendorTxCode { get; set; }
        public string VPSTxId { get; set; }
        public string VPSSignature { get; set; }
        public string TxAuthNo { get; set; }
        public string BankAuthCode { get; set; }
        public string AVSCV2 { get; set; }
        public string AddressResult { get; set; }
        public string PostCodeResult { get; set; }
        public string CV2Result { get; set; }
        public string GiftAid { get; set; }
        public string CAVV { get; set; }
        public string AddressStatus { get; set; }
        public string PayerStatus { get; set; }
        public string CardType { get; set; }
        public string Last4Digits { get; set; }
        public string DeclineCode { get; set; }
        public string ExpiryDate { get; set; }
        public string FraudResponse { get; set; }
    }
}
