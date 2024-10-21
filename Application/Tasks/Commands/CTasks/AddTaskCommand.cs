using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CTasks
{
    public class AddTaskCommand : IRequest<int>
    {
        public Domains.Models.Tasks Tasks { get; set; }
    }
}
