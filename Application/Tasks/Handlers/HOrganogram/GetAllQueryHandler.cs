using Application.Tasks.Queries.QLevel;
using Application.Tasks.Queries.QUserInfo;
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
    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, List<Level>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllQueryHandler(IUnitOfWork unitOfWork = null, IMapper mapper = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<Level>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork._levelRepository.GetAllLevel(request.OrgID);
            return result.ToList();
        }
    }
}
