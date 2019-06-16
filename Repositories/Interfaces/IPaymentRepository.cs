using SagePayServerIntegration.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagePayServerIntegration.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<SagePayPaymentDetail> Get(int Id);
        Task Create(SagePayPaymentDetail invoiceDbModel);
        Task Update(SagePayPaymentDetail invoiceDbModel);
        int UpdatePayment(string vendorTxCode, SagePayPaymentDetail sagePayPaymentDetail);
        string GetSecurityKey(string vendorTxCode);
        IEnumerable<Country> GetCountries();
        IEnumerable<State> GetStates();
    }
}
