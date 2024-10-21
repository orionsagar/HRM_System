using Domains.ViewModels;
using MediatR;

namespace Application.Tasks.Commands.CUserRole
{
    public class CreateUserRoleCommand : IRequest<int>
    {
        public UserRoleViewModel UserRoleViewModel { get; set; }
    }
}
