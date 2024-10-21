using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.PaymentTypes
{
    public interface IPaymentTypeRepo : IGenericRepository<PaymentType>
    {
        Task<IEnumerable<PaymentType>> GetAll(int OrgId);
    }
    public class PaymentTypeRepository : IPaymentTypeRepo
    {
        private readonly IApplicationDbContext _context;

        public PaymentTypeRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Add(PaymentType entity)
        {
            _context.PaymentTypes.Add(entity);
            return await _context.SaveChangesAsync();
        }


        public async Task<int> Delete(int id)
        {
            var exist = await GetById(id);
            if (exist == null) return 0;
            _context.PaymentTypes.Remove(exist);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PaymentType>> GetAll()
        {
            var data = await _context.PaymentTypes.ToListAsync();
            return data;
        }

        public async Task<IEnumerable<PaymentType>> GetAll(int OrgId)
        {
            var data = await _context.PaymentTypes.Where(p => p.OrgId == OrgId).ToListAsync();
            return data;
        }

        public async Task<PaymentType> GetById(int id)
        {
            return await _context.PaymentTypes.Where(p => p.PaymentTypeId == id).FirstOrDefaultAsync();
        }

        public async Task<int> Update(PaymentType entity)
        {
            _context.PaymentTypes.Update(entity);
            return await _context.SaveChangesAsync();
        }
    }
}
