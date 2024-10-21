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
    public class GetAllUserRoleQueryHandler : IRequestHandler<GetAllUserRoleQuery, List<UserRole>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUserRoleQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<UserRole>> Handle(GetAllUserRoleQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserRoles.GetAll();
            return result.ToList();
            //return _mapper.Map<List<UserRole>>(result.ToList());
        }
    }
}
