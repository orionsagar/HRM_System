using Application.Tasks.Queries.QUserRole;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HUserRole
{
    public class GetAllUserRoleWiseModuleQueryHandler : IRequestHandler<GetAllUserRoleWiseModuleQuery, List<UserRolewiseModule>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUserRoleWiseModuleQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<UserRolewiseModule>> Handle(GetAllUserRoleWiseModuleQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserRoles.GetUserWiseModule(request.RoleID);
            return result.ToList();
            //return _mapper.Map<List<UserRolewiseModule>>(result.ToList());
        }
    }
}
