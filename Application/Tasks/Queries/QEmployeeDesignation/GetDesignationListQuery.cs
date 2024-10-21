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

namespace Application.Tasks.Queries.QEmployeeDesignation
{
    public class GetDesignationListQuery : IRequest<List<DesignationVM>>
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
    public class GetDesignationListQueryHandler : IRequestHandler<GetDesignationListQuery, List<DesignationVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDesignationListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<DesignationVM>> Handle(GetDesignationListQuery request, CancellationToken cancellationToken)
        {
            if(request.RoleType == Enums.RoleType.Platform_Role.ToString())
            {
                var result = _unitOfWork.Designations.GetDesignationList(request.DisplayLength, request.DisplayStart, request.SortCol, request.SortDir, request.Search, request.ComId,request.OrgId,request.ClientId);
                return result;
            }
            else
            {
                var result = _unitOfWork.Designations.GetDesignationList(request.DisplayLength, request.DisplayStart, request.SortCol, request.SortDir, request.Search, request.ComId, request.OrgId, request.ClientId);
                return result;
            }
            
        }
    }
}
