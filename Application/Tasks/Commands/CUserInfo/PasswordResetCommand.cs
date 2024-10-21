using Domains.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CUserInfo
{
    public class PasswordResetCommand : IRequest<string>
    {
        public DomainEventVM DomainEvents { get; set; }
    }
}
