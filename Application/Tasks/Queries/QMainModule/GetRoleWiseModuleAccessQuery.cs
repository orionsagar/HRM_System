using Domains.Models;
using Domains.ViewModels;
using MediatR;
using System.Collections.Generic;

namespace Application.Tasks.Queries.QMainModule
{
    public class GetRoleWiseModuleAccessQuery : IRequest<List<RoleWiseUserAccessToolsVM>>
    {
        public int ModuleID { get; set; }
        public int SubModuleID { get; set; }
        public int RoleId { get; set; }
    }
}
