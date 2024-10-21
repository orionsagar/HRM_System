using Domains.Models;
using MediatR;
using System.Collections.Generic;

namespace Application.Tasks.Queries.QMainModule
{
    public class GetMenuModuleQuery : IRequest<List<MainModule>>
    {
        public int RoleID { get; set; }
    }
}
