using Domains.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QUserRole
{
    public class GetRolewiseAccessQuery : IRequest<UserAccessList>
    {
        public string UAPage { get; set; }
        public int RoleID { get; set; }
    }
}
