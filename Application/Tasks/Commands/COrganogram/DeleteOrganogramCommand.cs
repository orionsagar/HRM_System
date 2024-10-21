using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.COrganogram
{
    public class DeleteOrganogramCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}
