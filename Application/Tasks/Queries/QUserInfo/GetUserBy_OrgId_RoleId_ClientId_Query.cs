using Domains.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QUserInfo
{
    public class GetUserBy_OrgId_RoleId_ClientId_Query : IRequest<UserInfo>
    {
        public int OrgId { get; set; }
        public int RoleId { get; set; }
        public int ClientId { get; set; }
    }
}
