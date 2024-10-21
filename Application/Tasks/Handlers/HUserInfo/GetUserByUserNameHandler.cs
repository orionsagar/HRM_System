using Application.Tasks.Queries.QUserInfo;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HUserInfo
{
    public class GetUserByUserNameHandler : IRequestHandler<GetUserByUsernameQuery, UserInfo>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUserByUserNameHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserInfo> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserInfos.GetUserByUsername(request.username, request.password);
            //return _mapper.Map<UserInfo>(result);
            return result;
        }
    }
}
