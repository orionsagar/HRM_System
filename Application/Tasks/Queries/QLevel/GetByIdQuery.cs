using Domains.Models;
using Domains.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QLevel
{
    public class GetByIdQuery : IRequest<LevelViewModels>
    {
        public int ID { get; set; }
    }
}
