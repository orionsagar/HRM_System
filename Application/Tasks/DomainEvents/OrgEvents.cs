using Application.DomainEventFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.DomainEvents
{
    public class OrganisationCreatedDomainEvent : IDomainEventModel
    {
        public OrganisationCreatedDomainEvent(int orgId, int userId)
        {
            OrgId = orgId;
            UserId = userId;
        }

        public int OrgId { get; }
        public int UserId { get; }
    }
}
