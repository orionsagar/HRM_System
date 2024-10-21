using Domains.ViewModels;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public interface IDashBoard
    {
        Task<int> GetTotalActiveEmployee(string effectdate);
        Task<int> GetTotalPresentEmployee(string effectdate);
        Task<int> GetTotalAbsentEmployee(string effectdate);
        Task<int> GetTotalLeaveEmployee(string effectdate);
        Task<List<MonthlyAttendanceVM>> MonthlyAttendanceForChart(string attfrom, string attto);
        Task<List<StatusChartVM>> EmployeeStatusChart();
        Task<CardVM> Count_Org_And_Client();
        Task<List<ClientViewModel>> GetClientByStatus(string Status);
    }
    public class DashBoardReporitory : IDashBoard
    {
        private IApplicationDbContext _db;

        private IApplicationReadDbConnection _readDb;

        public DashBoardReporitory(IApplicationDbContext db, IApplicationReadDbConnection readDb)
        {
            _db = db;
            _readDb = readDb;

        }
        public async Task<int> GetTotalActiveEmployee(string effectdate)
        {
            try
            {
                _db.Connection.Open();
                string sql = $@"select count(*) from vw_Employees where EmpStatusName like '%Active%'";
                var data = await _readDb.QuerySingleAsync<int>(sql);
                _db.Connection.Close();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<int> GetTotalPresentEmployee(string effectdate)
        {
            try
            {
                _db.Connection.Open();
                string sql = $@"select count(*) from Attendance where AttDate = '{effectdate}'";
                var data = await _readDb.QuerySingleAsync<int>(sql);
                _db.Connection.Close();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<int> GetTotalAbsentEmployee(string effectdate)
        {
            try
            {
                _db.Connection.Open();
                string sql = $@"SELECT DBO.FN_CountAbsent ('{effectdate}')";
                var data = await _readDb.QuerySingleAsync<int>(sql);
                _db.Connection.Close();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<int> GetTotalLeaveEmployee(string effectdate)
        {
            try
            {
                _db.Connection.Open();
                string sql = $@"select count(*) from LeaveDays where LeaveDate = '{effectdate}'";
                var data = await _readDb.QuerySingleAsync<int>(sql);
                _db.Connection.Close();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<MonthlyAttendanceVM>> MonthlyAttendanceForChart(string attfrom, string attto)
        {
            try
            {
                _db.Connection.Open();
                string sql = $@"exec Prc_Chart_MonthlyAttendance '{attfrom}','{attto}'";
                var data = await _readDb.QueryAsync<MonthlyAttendanceVM>(sql);
                _db.Connection.Close();
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<StatusChartVM>> EmployeeStatusChart()
        {
            try
            {
                _db.Connection.Open();
                string sql = $@"exec EmpStatusChart";
                var data = await _readDb.QueryAsync<StatusChartVM>(sql);
                _db.Connection.Close();
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CardVM> Count_Org_And_Client()
        {
            _db.Connection.Open();
            string sql = $@"select * from vw_Count_Org_And_Client";
            var data = await _readDb.QueryFirstOrDefaultAsync<CardVM>(sql);
            _db.Connection.Close();
            return data;
        }

        public async Task<List<ClientViewModel>> GetClientByStatus(string Status)
        {
            _db.Connection.Open();
            string sql = $@"select *,[dbo].[NumberOfOrganisation](ClientId) Number_Of_Org from Clients where Status = @Status";
            var data = await _readDb.QueryAsync<ClientViewModel>(sql, new {Status});
            _db.Connection.Close();
            return data.ToList();
        }
    }
}
