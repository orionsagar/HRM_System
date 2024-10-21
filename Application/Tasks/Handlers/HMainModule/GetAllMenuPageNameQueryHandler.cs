using Application.Tasks.Queries.QMainModule;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HMainModule
{
    public class GetAllMenuPageNameQueryHandler : IRequestHandler<GetAllMenuPageNameQuery, List<UserAccessTools>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllMenuPageNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<UserAccessTools>> Handle(GetAllMenuPageNameQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserAccess.GetAllMenuPageName();
            return _mapper.Map<List<UserAccessTools>>(result.ToList());
        }
    }
}
