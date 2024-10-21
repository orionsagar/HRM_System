using Domains.Models;
using MediatR;
using System.Collections.Generic;

namespace Application.Tasks.Commands.CMainModule
{
    public class CreateRoleWiseModuleAccessCommand : IRequest<int>
    {
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public int? SubModuleId { get; set; }
        public List<UserAccessList> UserAccessList { get; set; }
    }
}
