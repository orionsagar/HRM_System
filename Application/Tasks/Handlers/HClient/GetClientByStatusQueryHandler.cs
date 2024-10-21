using Application.Tasks.Queries.QClient;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HClient
{
    public class GetClientByStatusQueryHandler : IRequestHandler<GetClientByStatusQuery, List<ClientViewModel>>
    {
        private readonly IUnitOfWork _work;

        public GetClientByStatusQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<List<ClientViewModel>> Handle(GetClientByStatusQuery request, CancellationToken cancellationToken)
        {
            var result = await _work.DashBoard.GetClientByStatus(request.Status);
            return result;
        }
    }
}
