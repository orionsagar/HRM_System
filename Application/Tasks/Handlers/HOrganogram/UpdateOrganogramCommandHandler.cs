using Application.Tasks.Commands.CLevel;
using Application.Tasks.Commands.COrganogram;
using Application.Tasks.Commands.CUserRole;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HOrganogram
{
    public class UpdateOrganogramCommandHandler : IRequestHandler<UpdateOrganogramCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateOrganogramCommandHandler(IUnitOfWork unitOfWork = null, IMapper mapper = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateOrganogramCommand request, CancellationToken cancellationToken)
        {
            var comp = _mapper.Map<OrgHierarchy>(request.OrgHierarchy);
            var result = await _unitOfWork._organogramRepository.Update(comp);
            return result;
        }
    }
}
