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
    public class MenuDropdownByCategoryQuery : IRequest<List<SelectListItemModel>>
    {
        public string CatName { get; set; }
    }
    public class MenuDropdownByCategoryQueryHandler : IRequestHandler<MenuDropdownByCategoryQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _work;

        public MenuDropdownByCategoryQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<List<SelectListItemModel>> Handle(MenuDropdownByCategoryQuery request, CancellationToken cancellationToken)
        {
            var result = await _work.UserAccess.MenuDropdownByCategory(request.CatName);
            return result.ToList();
        }
    }

}
