using Domains.Models;
using MediatR;
using System.Collections.Generic;

namespace Application.Tasks.Queries.QUserRole
{
    public class GetAllUserRoleQuery : IRequest<List<UserRole>>
    {
    }
}
