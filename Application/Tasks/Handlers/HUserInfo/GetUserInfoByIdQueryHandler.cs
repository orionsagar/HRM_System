using Application.Tasks.Queries.QUserInfo;
using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HUserInfo
{
    public class GetUserInfoByIdQueryHandler : IRequestHandler<GetUserInfoByIdQuery, UserInfoViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUserInfoByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserInfoViewModel> Handle(GetUserInfoByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserInfos.GetById(request.UserID);
            return _mapper.Map<UserInfoViewModel>(result);
        }
    }
}
