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
    public class GetUserDataByEmailQueryHandler : IRequestHandler<GetUserDataByEmailQuery, UserInfo>
    {
        private readonly IUnitOfWork _work;

        public GetUserDataByEmailQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<UserInfo> Handle(GetUserDataByEmailQuery request, CancellationToken cancellationToken)
        {
            var result = await _work.UserInfos.GetUserDataByEmail(request.Email);
            return result;
        }
    }
}
