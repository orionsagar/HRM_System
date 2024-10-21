using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QSection
{
    public class SP_Dt_SectionListQuery : IRequest<List<SP_Dt_SectionList>>
    {
        public int DisplayLength { get; set; }
        public int DisplayStart { get; set; }
        public int SortCol { get; set; }
        public string SortDir { get; set; }
        public string Search { get; set; }
        public int ComId { get; set; }
        public int OrgId { get; set; }
    }
    public class SP_Dt_SectionListQueryHandler : IRequestHandler<SP_Dt_SectionListQuery, List<SP_Dt_SectionList>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SP_Dt_SectionListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<List<SP_Dt_SectionList>> Handle(SP_Dt_SectionListQuery request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Sections.SP_Dt_SectionList(request.DisplayLength, request.DisplayStart, request.SortCol, request.SortDir, request.Search, request.ComId, request.OrgId);
            return result;
        }
    }
}
