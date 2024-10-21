using MediatR;

namespace Application.Tasks.Commands.CMainModule
{
    public class DeleteMainModuleCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}
