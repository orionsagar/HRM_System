using Application.DomainEventFramework.Core;
using Domains.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.DomainEvents
{
    public class EmployeeEvents : IDomainEventModel
    {
        public DomainEventVM DomainEventVM { get; }
        public EmployeeEvents(DomainEventVM domainEvent)
        {
            DomainEventVM = domainEvent;
        }
    }
}
