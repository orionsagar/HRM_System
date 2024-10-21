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
    public class GetMenuByIdQuery : IRequest<UserAccessTools>
    {
        public int uatoolid { get; set; }
    }
    public class GetMenuByIdQueryHandler : IRequestHandler<GetMenuByIdQuery, UserAccessTools>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMenuByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserAccessTools> Handle(GetMenuByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ModuleMenu.GetById(request.uatoolid);
            return result;
        }
    }
}
