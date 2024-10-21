using Domains.ViewModels;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public interface IDashboardBL
    {
        Task<int> GetTotalActiveEmployee(string effectivedate);
        Task<int> GetTotalPresentEmployee(string effectivedate);
        Task<int> GetTotalAbsentEmployee(string effectivedate);
        Task<int> GetTotalLeaveEmployee(string effectivedate);
        Task<List<MonthlyAttendanceVM>> MonthlyAttendanceForChart(string dtfrom, string dtto);
        Task<List<StatusChartVM>> EmployeeStatusChart();
    }
    public class DashboardBL : IDashboardBL
    {
        private IUnitOfWork _unitOfWork;

        public DashboardBL(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> GetTotalActiveEmployee(string effectivedate)
        {
            var activemember = await _unitOfWork.DashBoard.GetTotalActiveEmployee(effectivedate);
            return activemember;
        }

        public async Task<int> GetTotalPresentEmployee(string effectivedate)
        {
            var activemember = await _unitOfWork.DashBoard.GetTotalPresentEmployee(effectivedate);
            return activemember;
        }

        public async Task<int> GetTotalAbsentEmployee(string effectivedate)
        {
            var absentemember = await _unitOfWork.DashBoard.GetTotalAbsentEmployee(effectivedate);
            return absentemember;
        }

        public async Task<int> GetTotalLeaveEmployee(string effectivedate)
        {
            var leavemember = await _unitOfWork.DashBoard.GetTotalLeaveEmployee(effectivedate);
            return leavemember;
        }
        public async Task<List<MonthlyAttendanceVM>> MonthlyAttendanceForChart(string dtfrom, string dtto)
        {
            var leavemember = await _unitOfWork.DashBoard.MonthlyAttendanceForChart(dtfrom,dtto);
            return leavemember;
        }
        public async Task<List<StatusChartVM>> EmployeeStatusChart()
        {
            var statusdata = await _unitOfWork.DashBoard.EmployeeStatusChart();
            return statusdata;
        }

    }
}
