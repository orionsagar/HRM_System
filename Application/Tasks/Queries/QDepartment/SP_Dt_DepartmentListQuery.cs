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

namespace Application.Tasks.Queries.QDepartment
{
    public class SP_Dt_DepartmentListQuery : IRequest<List<SP_Dt_DepartmentList>>
    {
        //public int DisplayLength { get; set; }
        //public int DisplayStart { get; set; }
        //public int SortCol { get; set; }
        //public string SortDir { get; set; }
        //public string Search { get; set; }
        //public int ComId { get; set; }
        public DataTableParamVM param { get; set; } 
    }
    public class SP_Dt_DepartmentListQueryHandler : IRequestHandler<SP_Dt_DepartmentListQuery, List<SP_Dt_DepartmentList>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SP_Dt_DepartmentListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<SP_Dt_DepartmentList>> Handle(SP_Dt_DepartmentListQuery request, CancellationToken cancellationToken)
        {
            if(request.param.RoleType == Enums.RoleType.Platform_Role.ToString())
            {
                request.param.OrgId = 0;
                request.param.ClientId = 0;
                var result = _unitOfWork.Departments.SP_Dt_DepartmentList(request.param);
                return result;
            }
            else
            {
                var result = _unitOfWork.Departments.SP_Dt_DepartmentList(request.param);
                return result;
            }
            
        }
    }
}
