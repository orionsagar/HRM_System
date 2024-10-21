using Application.Tasks.Commands.CClient;
using Application.Tasks.Commands.CInvitation;
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

namespace Application.Tasks.Handlers.HInvitation
{
    public class UpdateInvitationCommandHandler : IRequestHandler<InvitationUpdateCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UpdateInvitationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(InvitationUpdateCommand request, CancellationToken cancellationToken)
        {
            var comp = _mapper.Map<PasswordInvitationViewModel>(request.InviteViewModel);
            var result = await _unitOfWork.InvitationRepo.UpdatePasswordInvite(comp);
            return result;
        }
    }
}
