using Domains.Models;
using MediatR;
using System.Collections.Generic;

namespace Application.Tasks.Queries.QUserRole
{
    public class GetAllUserRoleWiseModuleQuery : IRequest<List<UserRolewiseModule>>
    {
        public int RoleID { get; set; }
    }
}
