using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QMenu
{
    public class GetMenuByCategoryQuery : IRequest<List<UserAccessTools>>
    {
        public string CatName { get; set; }
    }

    public class GetMenuByCategoryQueryHandler : IRequestHandler<GetMenuByCategoryQuery, List<UserAccessTools>>
    {
        private readonly IUnitOfWork _work;

        public GetMenuByCategoryQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public Task<List<UserAccessTools>> Handle(GetMenuByCategoryQuery request, CancellationToken cancellationToken)
        {
            var result = _work.UserAccess.GetMenuByCategory(request.CatName);
            return result;
        }
    }
}
