using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility;

namespace Application.Tasks.Queries.QEmployee
{
    public class GetEmployeeInfoQuery : IRequest<CardVM>
    {
        public int OrgId { get; set; }
        public int ClientId { get; set; }
        public string RoleType { get; set; }
    }

    public class GetEmployeeInfoQueryHandler : IRequestHandler<GetEmployeeInfoQuery, CardVM>
    {
        private readonly IUnitOfWork _work;

        public GetEmployeeInfoQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<CardVM> Handle(GetEmployeeInfoQuery request, CancellationToken cancellationToken)
        {
            if(request.RoleType == Enums.RoleType.Platform_Role.ToString())
            {
                var result = await _work.DashBoard.Count_Org_And_Client();
                return result;
            }
            if (request.ClientId != 0)
            {
                var result = await _work.Employees.GetEmployeeInfoByClientId(request.ClientId);
                return result;
            }
            else
            {
                var result = await _work.Employees.GetEmployeeInfoByOrgId(request.OrgId);
                return result;
            }
        }
    }
}
