using Application.DomainEventFramework.Core;
using Domains.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.DomainEvents
{
    public class CommonEvents : IDomainEventModel
    {
        public DomainEventVM DomainEventVM { get; }
        public CommonEvents(DomainEventVM domainEvent) 
        {
            DomainEventVM = domainEvent;
        }
    }
}
