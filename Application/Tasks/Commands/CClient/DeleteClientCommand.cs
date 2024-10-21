using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CClient
{
    public class DeleteClientCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}
