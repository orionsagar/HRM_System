using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QUserInfo
{
    public class SP_Dt_UserListQuery : IRequest<List<UserInfoViewModel>>
    {
        public int DisplayLength { get; set; }
        public int DisplayStart { get; set; }
        public int SortCol { get; set; }
        public string SortDir { get; set; }
        public string Search { get; set; }
        public int ComId { get; set; }
        public int OrgId { get; set; }
        public int ClientId { get; set; }
        public string RolelType { get; set; }
    }

    public class SP_Dt_UserListQueryHandler : IRequestHandler<SP_Dt_UserListQuery, List<UserInfoViewModel>>
    {
        private readonly IUnitOfWork _work;

        public SP_Dt_UserListQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<List<UserInfoViewModel>> Handle(SP_Dt_UserListQuery request, CancellationToken cancellationToken)
        {
            var result =await _work.UserInfos.SP_Dt_UserList(request.DisplayLength, request.DisplayStart, request.SortCol, request.SortDir, request.Search, request.ComId, request.OrgId, request.ClientId, request.RolelType);
            return result;
        }
    }
}
