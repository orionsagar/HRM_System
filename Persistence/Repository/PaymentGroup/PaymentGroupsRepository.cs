using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.PaymentGroups
{
    public interface IPaymentGroupRepo : IGenericRepository<PaymentGroup>
    {
        Task<IEnumerable<SelectListItemModel>> Dropdown(int OrgId, int ClientId);
        Task<IEnumerable<PaymentGroup>> GetAll(int OrgId);
    }
    public class PaymentGroupsRepository : IPaymentGroupRepo
    {
        private IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDb;

        public PaymentGroupsRepository(IApplicationReadDbConnection readDb, IApplicationDbContext db)
        {
            _readDb = readDb;
            _db = db;
        }

        public async Task<int> Add(PaymentGroup entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.PaymentGroup.Add(entity);
                await _db.SaveChangesAsync();

                transaction.Commit();
                return entity.PayGroupId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<int> Delete(int id)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                var exist = await GetById(id);
                if (exist == null) return 0;

                _db.PaymentGroup.Remove(exist);
                await _db.SaveChangesAsync();
                transaction.Commit();
                return id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<SelectListItemModel>> Dropdown(int OrgId, int ClientId)
        {
            _db.Connection.Open();
            string sql = $"Select {nameof(PaymentGroup.PayGroupId)} Value, {nameof(PaymentGroup.PayGroup)} Text  from {nameof(_db.LeaveType)} where {nameof(PaymentGroup.OrgId)}=@OrgId";
            var data = await _readDb.QueryAsync<SelectListItemModel>(sql, new { OrgId });
            _db.Connection.Close();
            return data;
        }

        public async Task<IEnumerable<PaymentGroup>> GetAll()
        {
            try
            {
                //_db.Connection.Open();
                //string sql = $"Select {nameof(LeaveType.LeaveTypeId)} LeaveTypeId, {nameof(LeaveType.LTypeName)} LTypeName  from {nameof(_db.LeaveType)}";
                //var data = await _readDb.QueryAsync<LeaveType>(sql);
                //_db.Connection.Close();
                //return data;
                var data = await _db.PaymentGroup.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<PaymentGroup> GetById(int id)
        {
            return await _db.PaymentGroup.Where(a => a.PayGroupId == id).FirstOrDefaultAsync();
        }

        public async Task<int> Update(PaymentGroup entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.PaymentGroup.Update(entity);
                await _db.SaveChangesAsync();
                transaction.Commit();
                return entity.PayGroupId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<PaymentGroup>> GetAll(int OrgId)
        {
            try
            {
                //_db.Connection.Open();
                //string sql = $"Select {nameof(LeaveType.LeaveTypeId)} LeaveTypeId, {nameof(LeaveType.LTypeName)} LTypeName  from {nameof(_db.LeaveType)}";
                //var data = await _readDb.QueryAsync<LeaveType>(sql);
                //_db.Connection.Close();
                //return data;
                var data = await _db.PaymentGroup.Where(lt => lt.OrgId == OrgId).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
