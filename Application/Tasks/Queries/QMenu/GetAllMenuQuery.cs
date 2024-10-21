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

namespace Application.Tasks.Queries.QMenu
{

    public class GetAllMenuQuery : IRequest<List<UserAccessTools>>
    {
    }

    public class GetAllMenuQueryHandler : IRequestHandler<GetAllMenuQuery, List<UserAccessTools>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllMenuQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<UserAccessTools>> Handle(GetAllMenuQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ModuleMenu.GetAll();
            return _mapper.Map<List<UserAccessTools>>(result.ToList());
        }
    }
}
