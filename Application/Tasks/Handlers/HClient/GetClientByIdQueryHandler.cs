using Application.Tasks.Queries.QClient;
using AutoMapper;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HClient
{
    public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, ClientViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetClientByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ClientViewModel> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ClientRepo.GetById(request.ClientID);
            var client = _mapper.Map<ClientViewModel>(result);
            var userdata = await _unitOfWork.UserInfos.GetUserBy_OrgId_RoleId_ClientId(request.OrgId,0,request.ClientID);
            client.ClientSAEmail = userdata.UserName;
            return client;
        }
    }
}
