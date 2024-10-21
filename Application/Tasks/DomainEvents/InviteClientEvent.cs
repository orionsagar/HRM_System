using Application.DomainEventFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.DomainEvents
{
    public class InviteClientEvent : IDomainEventModel
    {
        public InviteClientEvent(string email, string userName)
        {
            Email = email;
            UserName = userName;
        }

        public string Email { get; }
        public string UserName { get; }
    }
}
