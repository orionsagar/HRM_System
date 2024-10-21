using MediatR;

namespace Application.Tasks.Queries.QUserInfo
{
    public class CheckUsernameExistsQuery : IRequest<bool>
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
