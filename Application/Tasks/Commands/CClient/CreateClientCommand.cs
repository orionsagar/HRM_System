using Domains.ViewModels;
using MediatR;

namespace Application.Tasks.Commands.CClient
{
    public class CreateClientCommand : IRequest<int>
    {
        public ClientViewModel ClientViewModel { get; set; }
    }
}
