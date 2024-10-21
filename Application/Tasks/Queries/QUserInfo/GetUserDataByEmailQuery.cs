using Domains.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QUserInfo
{
    public class GetUserDataByEmailQuery : IRequest<UserInfo>
    {
        public string Email { get; set; }
    }
}
