using Application.Tasks.Queries.QCompany;
using Application.Tasks.Queries.QInvitation;
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
    public class GetClientInvitationByHashQueryHandler : IRequestHandler<GetClientInvitationByHashQuery, Invitation>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientInvitationByHashQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Invitation> Handle(GetClientInvitationByHashQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.InvitationRepo.GetInvitationInfoByHash(request.Hash);
            return _mapper.Map<Invitation>(result);
        }
    }
}
