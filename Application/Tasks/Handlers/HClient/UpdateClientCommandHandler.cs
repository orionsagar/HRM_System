using Application.Tasks.Commands.CClient;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;
using Utility;

namespace Application.Tasks.Handlers.HClient
{
    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UpdateClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            request.ClientViewModel.ClientType = request.ClientViewModel.ClientType == "Others" ? request.ClientViewModel.ClientType1 : request.ClientViewModel.ClientType;

            var comp = _mapper.Map<Client>(request.ClientViewModel);
            comp.State = Enums.Status.Active.ToString();
            var result = await _unitOfWork.ClientRepo.Update(comp);
            return result;
        }
    }
}
