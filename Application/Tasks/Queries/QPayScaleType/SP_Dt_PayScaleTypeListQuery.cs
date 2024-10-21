using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QPayScaleType
{
    public class SP_Dt_PayScaleTypeListQuery : IRequest<List<PayScaleType>>
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
    public class SP_Dt_PayScaleTypeListQueryHandler : IRequestHandler<SP_Dt_PayScaleTypeListQuery, List<PayScaleType>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SP_Dt_PayScaleTypeListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PayScaleType>> Handle(SP_Dt_PayScaleTypeListQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PayScales.SP_Dt_PayScaleTypeList(request.DisplayLength, request.DisplayStart, request.SortCol, request.SortDir, request.Search, request.ComId, request.OrgId, request.ClientId);
            return result.ToList();
        }
    }
}
