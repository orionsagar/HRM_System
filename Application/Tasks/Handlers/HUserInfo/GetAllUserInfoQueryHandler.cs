using Application.Tasks.Queries.QUserInfo;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HUserInfo
{
    public class GetAllUserInfoQueryHandler : IRequestHandler<GetAllUserInfoQuery, List<UserInfo>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllUserInfoQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<UserInfo>> Handle(GetAllUserInfoQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserInfos.GetAll();
            return result.ToList();
        }
    }
}
