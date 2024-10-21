using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CLevel
{
    public class DeleteLevelCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}
