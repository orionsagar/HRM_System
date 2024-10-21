using Application.Tasks.Queries.QUserInfo;
using AutoMapper;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HUserInfo
{
    public class CheckUsernameExistsHandler : IRequestHandler<CheckUsernameExistsQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CheckUsernameExistsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CheckUsernameExistsQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserInfos.CheckUsernameExists(request.username, request.password);
            return result;
        }
    }
}
