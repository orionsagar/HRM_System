using Domains.Models;
using Domains.ViewModels;
using MediatR;

namespace Application.Tasks.Queries.QUserInfo
{
    public class GetUserInfoByIdQuery : IRequest<UserInfoViewModel>
    {
        public int UserID { get; set; }
    }
}
