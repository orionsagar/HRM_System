using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QLevel
{
    public class CheckExistsQuery : IRequest<bool>
    {
        public string levelname { get; set; }
    }
}
