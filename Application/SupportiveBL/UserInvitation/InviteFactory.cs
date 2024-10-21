using Domains.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Application.SupportiveBL.UserInvitation
{
    public class InviteFactory
    {
        private readonly IServiceProvider provider;

        public InviteFactory(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public IUserInvitation ResolveInvitation(InvitationType type)
        {
            return type switch
            {
                InvitationType.Password => provider.GetService<PasswordInvitation>(),
                _ => provider.GetService<AccountInvitation>(),
            };
        }
    }
}
