using Application.Tasks.Queries.QUserInfo;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HUserInfo
{
    public class GetUserBy_OrgId_RoleId_ClientId_QueryHandler : IRequestHandler<GetUserBy_OrgId_RoleId_ClientId_Query, UserInfo>
    {
        private readonly IUnitOfWork _work;

        public GetUserBy_OrgId_RoleId_ClientId_QueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<UserInfo> Handle(GetUserBy_OrgId_RoleId_ClientId_Query request, CancellationToken cancellationToken)
        {
            var userInfo = await _work.UserInfos.GetUserBy_OrgId_RoleId_ClientId(request.OrgId, request.RoleId, request.ClientId);
            return userInfo;
        }
    }
}
