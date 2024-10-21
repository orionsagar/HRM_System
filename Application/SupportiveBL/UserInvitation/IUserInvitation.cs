using System.Collections.Generic;

namespace Application.SupportiveBL.UserInvitation
{
    public interface IUserInvitation
    {
        InviteResult GenerateInvitation(string input);
    }
}
