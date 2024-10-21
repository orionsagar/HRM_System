using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployeeCategory
{
    public class SP_Dt_CategoryListQuery : IRequest<List<SP_Dt_CategoryList>>
    {
        public int DisplayLength { get; set; }
        public int DisplayStart { get; set; }
        public int SortCol { get; set; }
        public string SortDir { get; set; }
        public string Search { get; set; }
        public int ComId { get; set; }
        public int OrgId { get; set; }
        public int ClientId { get; set; }
        public string RoleType { get; set; }
    }

    public class SP_Dt_CategoryListQueryHandler : IRequestHandler<SP_Dt_CategoryListQuery, List<SP_Dt_CategoryList>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SP_Dt_CategoryListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<List<SP_Dt_CategoryList>> Handle(SP_Dt_CategoryListQuery request, CancellationToken cancellationToken)
        {
            if(request.RoleType == Utility.Enums.RoleType.Platform_Role.ToString())
            {
                var result = _unitOfWork.EmployeeCategorys.SP_Dt_CategoryList(request.DisplayLength, request.DisplayStart, request.SortCol, request.SortDir, request.Search, request.ComId);
                return result;
            }
            else
            {
                var result = _unitOfWork.EmployeeCategorys.SP_Dt_CategoryList(request.DisplayLength, request.DisplayStart, request.SortCol, request.SortDir, request.Search, request.ComId, request.OrgId, request.ClientId);
                return result;
            }
            
        }
    }
}
