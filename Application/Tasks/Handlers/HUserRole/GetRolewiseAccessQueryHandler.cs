using Application.Tasks.Queries.QUserRole;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HUserRole
{
    public class GetRolewiseAccessQueryHandler : IRequestHandler<GetRolewiseAccessQuery, UserAccessList>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRolewiseAccessQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserAccessList> Handle(GetRolewiseAccessQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserRoles.GetRolewiseAccess(request.UAPage, request.RoleID);
            //return _mapper.Map<UserAccessList>(result);
            return  result.FirstOrDefault();
        }
    }
}
