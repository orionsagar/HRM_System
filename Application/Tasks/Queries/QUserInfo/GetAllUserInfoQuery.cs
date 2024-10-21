using Domains.Models;
using MediatR;
using System.Collections.Generic;

namespace Application.Tasks.Queries.QUserInfo
{
    public class GetAllUserInfoQuery : IRequest<List<UserInfo>>
    {
    }
}
