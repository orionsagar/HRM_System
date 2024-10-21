using Domains.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CTax
{
    public class AddTaxCommand : IRequest<int>
    {
        public TaxMaster TaxMaster { get; set; }
    }
}
