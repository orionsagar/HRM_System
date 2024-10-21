using Domains.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CClient
{
    public class AddClientByInvitationCommand : IRequest<string>
    {
        public ClientViewModel Client { get; set; }
    }
}
