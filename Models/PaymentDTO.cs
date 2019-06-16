using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SagePayServerIntegration.Models
{
    public class PaymentDTO
    {
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string VendorTxCode { get; set; }
        public string NotificationURL { get; set; }
        public string Vendor { get; set; }
        public string Description { get; set; }
        [MaxLength(20)]
        [Display(Prompt = "Billing Company Name*")]
        [Required(ErrorMessage = "The Billing Company Name field is required.")]
        public string BillingFirstnames { get; set; }
        [MaxLength(20)]
        [Display(Prompt = "Billing CardHolder Name*")]
        [Required(ErrorMessage = "The Billing CardHolder Name field is required.")]
        public string BillingSurname { get; set; }
        [MaxLength(100)]
        [Display(Prompt = "Billing Address*")]
        [Required(ErrorMessage = "The Billing Address field is required.")]
        public string BillingAddress1 { get; set; }
        [Required]
        public string BillingAddress2 { get; set; }
        [MaxLength(40)]
        [Display(Prompt = "Billing City*")]
        [Required(ErrorMessage = "The Billing City field is required.")]
        public string BillingCity { get; set; }
        [MaxLength(10)]
        [Display(Prompt = "Billing Postcode*")]
        [Required(ErrorMessage = "The Billing Postcode field is required.")]
        public string BillingPostCode { get; set; }
        [Display(Prompt = "Billing Country")]
        [Required(ErrorMessage = "The Billing Country field is required.")]
        public string BillingCountry { get; set; }
        [MaxLength(20)]
        [Display(Prompt = "Delivery Firstnames*")]
        [Required(ErrorMessage = "The Delivery Firstnames field is required.")]
        public string DeliveryFirstnames { get; set; }
        [MaxLength(20)]
        [Display(Prompt = "Delivery Surname*")]
        [Required(ErrorMessage = "The Delivery Surname field is required.")]
        public string DeliverySurname { get; set; }
        [MaxLength(100)]
        [Display(Prompt = "Delivery Address*")]
        [Required(ErrorMessage = "The Delivery Address field is required.")]
        public string DeliveryAddress1 { get; set; }
        [Required]
        public string DeliveryAddress2 { get; set; }
        [MaxLength(40)]
        [Display(Prompt = "Delivery City")]
        [Required(ErrorMessage = "The Delivery City field is required.")]
        public string DeliveryCity { get; set; }
        [MaxLength(10)]
        [Display(Prompt = "Delivery Postcode*")]
        [Required(ErrorMessage = "The Delivery Postcode field is required.")]
        public string DeliveryPostCode { get; set; }
        [Display(Prompt = "Delivery Country")]
        [Required(ErrorMessage = "The Delivery Country field is required.")]
        public string DeliveryCountry { get; set; }
        [Display(Prompt = "Delivery State")]
        [Required(ErrorMessage = "The Delivery State field is required.")]
        public string DeliveryState { get; set; }
        [Display(Prompt = "Billing State")]
        [Required(ErrorMessage = "The Billing State field is required.")]
        public string BillingState { get; set; }
        [EmailAddress]
        [Display(Prompt = "Email Address")]
        [MaxLength(255)]
        public string CustomerEMail { get; set; }
    }
}
