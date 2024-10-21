using Application.Tasks.Handlers.HCompany;
using AutoMapper;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HClient
{
    public class SP_Dt_ClientListQuery : IRequest<List<ClientViewModel>>
    {
        public int DisplayLength { get; set; }
        public int Start { get; set; }
        public int SortCol { get; set; }
        public string SortDir { get; set; }
        public string Search { get; set; }
        public int ClientId { get; set; }
        public string RoleType { get; set; }
    }
    public class SP_Dt_ClientListCommandHandler : IRequestHandler<SP_Dt_ClientListQuery, List<ClientViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SP_Dt_ClientListCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ClientViewModel>> Handle(SP_Dt_ClientListQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ClientRepo.SP_Dt_ClientList(request.DisplayLength, request.Start, request.SortCol, request.SortDir, request.Search, request.ClientId,request.RoleType);
            //var data = _mapper.Map<List<CompanyViewModel>>(result);
            return result.ToList();
        }
    }
}
