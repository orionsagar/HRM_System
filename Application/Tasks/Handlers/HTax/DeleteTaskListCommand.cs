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
    public class DeleteTaskListCommand : IRequest<int>
    {
        public int TaxMasterId { get; set; }
    }
    public class DeleteTaskListCommandHandler : IRequestHandler<DeleteTaskListCommand, int>
    {
        private readonly IUnitOfWork _work;

        public DeleteTaskListCommandHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<int> Handle(DeleteTaskListCommand request, CancellationToken cancellationToken)
        {
            var data = await _work.Taxs.Delete(request.TaxMasterId);
            return data;
        }
    }
}
