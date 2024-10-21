using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.PaymentMoads
{
    public interface IPaymentMoad : IGenericRepository<PaymentMoad>
    {
        Task<IEnumerable<PaymentMoad>> GetAll(int OrgId);
    }
    public class PaymentMoadRepository : IPaymentMoad
    {
        private readonly IApplicationDbContext _contex;

        public PaymentMoadRepository(IApplicationDbContext contex)
        {
            _contex = contex;
        }

        public async Task<int> Add(PaymentMoad entity)
        {
            _contex.PaymentMoads.Add(entity);
            return await _contex.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var exist = await GetById(id);
            if (exist == null) return 0;
            _contex.PaymentMoads.Remove(exist);
            return await _contex.SaveChangesAsync();
        }

        public async Task<IEnumerable<PaymentMoad>> GetAll()
        {
            var data = await _contex.PaymentMoads.ToListAsync();
            return data;
        }

        public async Task<IEnumerable<PaymentMoad>> GetAll(int OrgId)
        {
            var data = await _contex.PaymentMoads.Where(p=>p.OrgId == OrgId).ToListAsync();
            return data;
        }

        public async Task<PaymentMoad> GetById(int id)
        {
            return await _contex.PaymentMoads.Where(p => p.PaymentMoadId == id).FirstOrDefaultAsync();
        }

        public async Task<int> Update(PaymentMoad entity)
        {
            _contex.PaymentMoads.Update(entity);
            return await _contex.SaveChangesAsync();
        }
    }
}
