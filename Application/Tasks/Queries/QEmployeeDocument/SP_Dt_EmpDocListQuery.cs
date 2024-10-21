using Application.Tasks.Queries.QEmployeeType;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployeeDocument
{
    public class SP_Dt_EmpDocListQuery : IRequest<List<SP_Dt_EmpDocList>>
    {
        public int DisplayLength { get; set; }
        public int DisplayStart { get; set; }
        public int SortCol { get; set; }
        public string SortDir { get; set; }
        public string Search { get; set; }
        public int ClientId { get; set; }
        public int OrgId { get; set; }
    }

    public class SP_Dt_EmpDocListQueryHandler : IRequestHandler<SP_Dt_EmpDocListQuery, List<SP_Dt_EmpDocList>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SP_Dt_EmpDocListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SP_Dt_EmpDocList>> Handle(SP_Dt_EmpDocListQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork._EmployeeDocument.SP_Dt_EmpDocList(request.DisplayLength, request.DisplayStart, request.SortCol, request.SortDir, request.Search, request.ClientId, request.OrgId);
            return result;
        }
    }
}
