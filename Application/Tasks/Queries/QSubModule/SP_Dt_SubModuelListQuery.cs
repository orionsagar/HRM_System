using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QSubModule
{
    public class SP_Dt_SubModuelListQuery : IRequest<List<SubModuleVM>>
    {
        public int DisplayLength { get; set; }
        public int DisplayStart { get; set; }
        public int SortCol { get; set; }
        public string SortDir { get; set; }
        public string Search { get; set; }
        public int ModuleId { get; set; }
        public int ComId { get; set; }
    }
    public class SP_Dt_SubModuelListQueryHandler : IRequestHandler<SP_Dt_SubModuelListQuery, List<SubModuleVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SP_Dt_SubModuelListQueryHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<List<SubModuleVM>> Handle(SP_Dt_SubModuelListQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.SubModule.SP_Dt_SubModuelList(request.DisplayLength, request.DisplayStart, request.SortCol, request.SortDir, request.Search, request.ModuleId, request.ComId);
            return result.ToList();
        }
    }
}
