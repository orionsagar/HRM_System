﻿using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.Leave
{
    public interface ILeaveTypeRepo : IGenericRepository<LeaveType>
    {
        Task<IEnumerable<SelectListItemModel>> Dropdown(int compId, int OrgId);
        Task<List<LeaveType>> GetAll(int orgId);

    }
    public class LeaveTypeRepository : ILeaveTypeRepo
    {
        private IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDb;

        public LeaveTypeRepository(IApplicationReadDbConnection readDb, IApplicationDbContext db)
        {
            _readDb = readDb;
            _db = db;
        }
        public async Task<int> Add(LeaveType entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.LeaveType.Add(entity);
                await _db.SaveChangesAsync();

                transaction.Commit();
                return entity.LeaveTypeId;
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

                _db.LeaveType.Remove(exist);
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

        public async Task<IEnumerable<LeaveType>> GetAll()
        {
            try
            {
                var data= await _db.LeaveType.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
        public async Task<LeaveType> GetById(int id)
        {
            return await _db.LeaveType.Where(a => a.LeaveTypeId == id).FirstOrDefaultAsync();
        }

        public async Task<int> Update(LeaveType entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.LeaveType.Update(entity);
                await _db.SaveChangesAsync();
                transaction.Commit();
                return entity.LeaveTypeId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<SelectListItemModel>> Dropdown(int compId, int OrgId)
        {
            _db.Connection.Open();
            string sql = $"Select {nameof(LeaveType.LeaveTypeId)} Value, {nameof(LeaveType.LTypeName)} Text  from {nameof(_db.LeaveType)} where {nameof(LeaveType.CompId)}=@compId and OrgId = @OrgId";
            var data = await _readDb.QueryAsync<SelectListItemModel>(sql, new { compId, OrgId });
            _db.Connection.Close();
            return data;
        }

        public async Task<List<LeaveType>> GetAll(int orgId)
        {
            try
            {
                var data = await _db.LeaveType.Where(l => l.OrgId == orgId).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}