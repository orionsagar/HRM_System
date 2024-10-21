using Domains.Models;
using Domains.ViewModels;
using MediatR;

namespace Application.Tasks.Commands.CMainModule
{
    public class CreateMainModuleCommand : IRequest<int>
    {
        public MainModule MainModule { get; set; }
    }
}
