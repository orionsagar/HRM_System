using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QPayScale
{
    public class SP_Dt_PayScaleListQuery : IRequest<List<SP_Dt_PayScaleList>>
    {
        public int DisplayLength { get; set; }
        public int DisplayStart { get; set; }
        public int SortCol { get; set; }
        public string SortDir { get; set; }
        public string Search { get; set; }
        public int ComId { get; set; }
        public int OrgId { get; set; }
        public int ClientId { get; set; }
    }
    public class SP_Dt_PayScaleListQueryHandler : IRequestHandler<SP_Dt_PayScaleListQuery, List<SP_Dt_PayScaleList>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SP_Dt_PayScaleListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SP_Dt_PayScaleList>> Handle(SP_Dt_PayScaleListQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PayScales.SP_Dt_PayScaleList(request.DisplayLength, request.DisplayStart, request.SortCol, request.SortDir, request.Search, request.ComId, request.OrgId, request.ClientId);
            return result.ToList();
        }
    }
}
