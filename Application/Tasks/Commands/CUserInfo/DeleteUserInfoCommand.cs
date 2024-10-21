using MediatR;

namespace Application.Tasks.Commands.CUserInfo
{
    public class DeleteUserInfoCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}
