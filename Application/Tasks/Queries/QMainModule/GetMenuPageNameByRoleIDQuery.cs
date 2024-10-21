using Domains.Models;
using MediatR;
using System.Collections.Generic;

namespace Application.Tasks.Queries.QMainModule
{
    public class GetMenuPageNameByRoleIDQuery : IRequest<List<UserAccessTools>>
    {
        public int RoleID { get; set; }
        public int ModuleID { get; set; }
    }
}
