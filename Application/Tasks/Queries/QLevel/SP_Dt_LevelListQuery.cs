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

namespace Application.Tasks.Queries.QLevel
{
    public class SP_Dt_LevelListQuery : IRequest<List<SP_Dt_LevelList>>
    {
        public int DisplayLength { get; set; }
        public int DisplayStart { get; set; }
        public int SortCol { get; set; }
        public string SortDir { get; set; }
        public string Search { get; set; }
        public int OrgId { get; set; }
    }
    public class SP_Dt_LevelListQueryHandler : IRequestHandler<SP_Dt_LevelListQuery, List<SP_Dt_LevelList>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public SP_Dt_LevelListQueryHandler(IUnitOfWork unitOfWork = null)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SP_Dt_LevelList>> Handle(SP_Dt_LevelListQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork._levelRepository.SP_Dt_LevelList(request.DisplayLength, request.DisplayStart, request.SortCol, request.SortDir, request.Search, request.OrgId);
            return result;
        }
    }
}
