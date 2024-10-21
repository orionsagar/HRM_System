using Application.Tasks.Queries.QUserRole;
using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HUserRole
{
    public class GetUserRoleByIdQueryHandler : IRequestHandler<GetUserRoleByIdQuery, UserRoleViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserRoleByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserRoleViewModel> Handle(GetUserRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserRoles.GetById(request.RoleID);
            return _mapper.Map<UserRoleViewModel>(result);
        }
    }
}
