using Domains.Models;
using Utility;

namespace Application.SupportiveBL.UserInvitation
{
    public class PasswordInvitation : IUserInvitation
    {
        public PasswordInvitation()
        {
            
        }

        public InviteResult GenerateInvitation(string input)
        {
            InviteModel invite = new()
            {
                // generate a hash string
                Hash = StringUtil.GetHashString(input),
                Input = input,
                HasExpiry = false,
                IsExpired = false,
                InvitationType = InvitationType.Password
            };

            return new InviteResult(true, invite);
        }
    }
}
