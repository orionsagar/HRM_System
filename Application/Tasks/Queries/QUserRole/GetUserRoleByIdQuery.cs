using Domains.Models;
using Domains.ViewModels;
using MediatR;

namespace Application.Tasks.Queries.QUserRole
{
    public class GetUserRoleByIdQuery : IRequest<UserRoleViewModel>
    {
        public int RoleID { get; set; }
    }
}
