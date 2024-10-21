using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QMenu
{
    public class SP_Dt_ModuelMenuListQuery : IRequest<List<UserAccessToolsVM>>
    {
        public int DisplayLength { get; set; }
        public int DisplayStart { get; set; }
        public int SortCol { get; set; }
        public string SortDir { get; set; }
        public string Search { get; set; }
        public int ModuleId { get; set; }
        public int? SubModuleId { get; set; }
        public int CompId { get; set; }
    }
    public class SP_Dt_ModuelMenuListQueryHandler : IRequestHandler<SP_Dt_ModuelMenuListQuery, List<UserAccessToolsVM>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SP_Dt_ModuelMenuListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<UserAccessToolsVM>> Handle(SP_Dt_ModuelMenuListQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ModuleMenu.SP_Dt_ModuelMenuList(request.DisplayLength, request.DisplayStart, request.SortCol, request.SortDir, request.Search, request.ModuleId, request.SubModuleId, request.CompId);
            ///var data = _mapper.Map<UserAccessTools>(result);
            return result.ToList();
        }
    }
}
