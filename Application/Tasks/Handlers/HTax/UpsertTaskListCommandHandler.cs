using Domains.Models;
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
    public class UpsertTaskListCommand : IRequest<int>
    {
        public TaxMaster TaxMaster{ get; set; }
    }
    public class UpsertTaskListCommandHandler : IRequestHandler<UpsertTaskListCommand, int>
    {
        private readonly IUnitOfWork _work;

        public UpsertTaskListCommandHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<int> Handle(UpsertTaskListCommand request, CancellationToken cancellationToken)
        {
            if (request.TaxMaster.TaxId > 0)
            {
                var result = await _work.Taxs.Update(request.TaxMaster);

                return result;
            }
            else
            {
                var data = await _work.Taxs.Add(request.TaxMaster);
                return data;
            }

        }
    }
}
