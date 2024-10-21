using Application.Tasks.Queries.QLevel;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QOrganogram
{
    public class SP_Dt_OrganogramListQuery : IRequest<List<SP_Dt_OrganogramList>>
    {
        public int DisplayLength { get; set; }
        public int DisplayStart { get; set; }
        public int SortCol { get; set; }
        public string SortDir { get; set; }
        public string Search { get; set; }
        public int OrgId { get; set; }
        public int ClientId { get; set; }
    }
    public class SP_Dt_OrganogramListQueryHandler : IRequestHandler<SP_Dt_OrganogramListQuery, List<SP_Dt_OrganogramList>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public SP_Dt_OrganogramListQueryHandler(IUnitOfWork unitOfWork = null)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SP_Dt_OrganogramList>> Handle(SP_Dt_OrganogramListQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork._organogramRepository.SP_Dt_OrganogramList(request.DisplayLength, request.DisplayStart, request.SortCol, request.SortDir, request.Search, request.OrgId, request.ClientId);
            return result;
        }
    }
}
