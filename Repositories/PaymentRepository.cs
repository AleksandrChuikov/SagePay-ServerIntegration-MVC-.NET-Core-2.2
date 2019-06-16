using Microsoft.EntityFrameworkCore;
using SagePayServerIntegration.DbContexts;
using SagePayServerIntegration.Entities;
using SagePayServerIntegration.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagePayServerIntegration.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationContext _dbContext;
        public PaymentRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(SagePayPaymentDetail sagePayPaymentDetail)
        {
            _dbContext.SagePayPaymentDetail.Add(sagePayPaymentDetail);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<SagePayPaymentDetail> Get(int Id)
        {
            return await _dbContext.SagePayPaymentDetail.FirstOrDefaultAsync(x => x.ID == Id);
        }

        public async Task Update(SagePayPaymentDetail sagePayPaymentDetail)
        {
            _dbContext.Entry(sagePayPaymentDetail).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public int UpdatePayment(string vendorTxCode, SagePayPaymentDetail sagePayPaymentDetail)
        {
            var sageDetail = _dbContext.SagePayPaymentDetail.FirstOrDefault(x => x.VendorTxCode == vendorTxCode);
            if (sageDetail != null)
            {
                sageDetail.TransactionCompleted = DateTime.UtcNow;
                sageDetail.VPSSignatureServerValue = sagePayPaymentDetail.VPSSignatureServerValue;
                sageDetail.TxAuthNo = sagePayPaymentDetail.TxAuthNo;
                sageDetail.VPSSignature = sagePayPaymentDetail.VPSSignature;
                sageDetail.BankAuthCode = sagePayPaymentDetail.BankAuthCode;
                sageDetail.Status = sagePayPaymentDetail.Status;
                _dbContext.Entry(sageDetail).State = EntityState.Modified;
                return _dbContext.SaveChanges();
            }
            else
                return 0;
        }

        public string GetSecurityKey(string vendorTxCode)
        {
            return _dbContext.SagePayPaymentDetail.FirstOrDefault(x => x.VendorTxCode == vendorTxCode)?.SecurityKey;
        }

        public IEnumerable<Country> GetCountries()
        {
            return _dbContext.Countries.OrderBy(x=>x.Name);
        }    

        public IEnumerable<State> GetStates()
        {
            return _dbContext.States.OrderBy(x => x.Name);
        }
    }
}
