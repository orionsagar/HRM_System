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

namespace Application.Tasks.Queries.QUserRole
{
    public class GetAllUserRoleWiseSubModuleQuery : IRequest<List<UserRolewiseSubModule>>
    {
        public int RoleId { get; set; }
    }

    public class GetAllUserRoleWiseSubModuleQueryHandler : IRequestHandler<GetAllUserRoleWiseSubModuleQuery, List<UserRolewiseSubModule>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUserRoleWiseSubModuleQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<UserRolewiseSubModule>> Handle(GetAllUserRoleWiseSubModuleQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserRoles.GetUserWiseSubModule(request.RoleId);
            return result.ToList();
        }
    }
}
