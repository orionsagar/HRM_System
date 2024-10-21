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

namespace Application.Tasks.Commands.CMenu
{
    public class CreateMenuCommand : IRequest<int>
    {
        public UserAccessTools AccessTools { get; set; }
    }
    public class CreateMenuCommandHandler : IRequestHandler<CreateMenuCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateMenuCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
        {
            //var comp = _mapper.Map<UserAccessTools>(request.AccessTools);
            var result = await _unitOfWork.ModuleMenu.Add(request.AccessTools);
            return result;
        }
    }
}
