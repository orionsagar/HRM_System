using Application.Tasks.Commands.CTax;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HTax
{
    public class AddTaxCommandHandler : IRequestHandler<AddTaxCommand, int>
    {
        private readonly IUnitOfWork _work;

        public AddTaxCommandHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<int> Handle(AddTaxCommand request, CancellationToken cancellationToken)
        {
            var result = await _work.Taxs.Add(request.TaxMaster);
            return result;
        }
    }
}
