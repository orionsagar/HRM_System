using Domains.Models;
using MediatR;
using System.Collections.Generic;

namespace Application.Tasks.Commands.CUserRole
{
    public class CreateUserRoleWiseModuleCommand : IRequest<int>
    {
        public int RoleId { get; set; }
        public List<UserRolewiseModule> UserRolewiseModule { get; set; }
    }
}
