using Application.Tasks.Commands.CLevel;
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
    public class UpdateLevelCommandHandler : IRequestHandler<UpdateLevelCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateLevelCommandHandler(IUnitOfWork unitOfWork = null, IMapper mapper = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateLevelCommand request, CancellationToken cancellationToken)
        {
            var comp = _mapper.Map<Level>(request.levelViewModel);
            var result = await _unitOfWork._levelRepository.Update(comp);
            return result;
        }
    }
}
