using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagePayServerIntegration.Entities
{
    public class SagePayPaymentDetail
    {
        public int ID { get; set; }
        public string VendorTxCode { get; set; }
        public string VPSSignature { get; set; }
        public string VPSSignatureServerValue { get; set; }
        public string VPSTxId { get; set; }
        public string SecurityKey { get; set; }
        public string TxAuthNo { get; set; }
        public string BankAuthCode { get; set; }
        public string Status { get; set; }
        public DateTime? TransactionCompleted { get; set; }
        public string BillingFirstnames { get; set; }
        public string BillingSurname { get; set; }
        public string BillingAddress1 { get; set; }
        public string BillingAddress2 { get; set; }
        public string BillingCity { get; set; }
        public string BillingPostCode { get; set; }
        public string BillingCountry { get; set; }
        public string DeliveryFirstnames { get; set; }
        public string DeliverySurname { get; set; }
        public string DeliveryAddress1 { get; set; }
        public string DeliveryAddress2 { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryPostCode { get; set; }
        public string DeliveryCountry { get; set; }
        public string DeliveryState { get; set; }
        public string BillingState { get; set; }
        public string CustomerEMail { get; set; }
    }
}
