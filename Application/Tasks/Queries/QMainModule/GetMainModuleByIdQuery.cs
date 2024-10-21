using Domains.Models;
using MediatR;

namespace Application.Tasks.Queries.QMainModule
{
    public class GetMainModuleByIdQuery : IRequest<MainModule>
    {
        public int ModuleID { get; set; }
    }
}
