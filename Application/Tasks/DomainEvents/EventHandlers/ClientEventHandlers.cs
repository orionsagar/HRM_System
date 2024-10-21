using Application.SupportiveBL.Email;
using Application.SupportiveBL.UserInvitation;
using AutoMapper;
using Domains.Models;
using Persistence.DAL;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Utility;

namespace Application.Tasks.DomainEvents.EventHandlers
{
    public class ClientEventHandlers
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly InviteFactory inviteFactory;
        private readonly IMapper mapper;
        private readonly IEmailSender emailSender;
        private readonly HttpUtil httpUtil;
        private readonly string baseInviteLink = "https://www.worker";

        public ClientEventHandlers(IUnitOfWork unitOfWork, InviteFactory inviteFactory, IMapper mapper, IEmailSender emailSender, HttpUtil httpUtil)
        {
            this.unitOfWork = unitOfWork;
            this.inviteFactory = inviteFactory;
            this.mapper = mapper;
            this.emailSender = emailSender;
            this.httpUtil = httpUtil;
        }

        public async Task ClientCreatedHandler(ClientCreatedDomainEvent clientCreated, string createdBy = "")
        {
            if (clientCreated.UserId != 0)
            {
                var client = await unitOfWork.ClientRepo.GetById(clientCreated.ClientId);
                var user = await unitOfWork.UserInfos.GetById(clientCreated.UserId);

                // generate invitation here.
                var uInvitation = inviteFactory.ResolveInvitation(InvitationType.Password);
                var invite = uInvitation.GenerateInvitation(user.Email);
                
                if (invite.Success)
                {
                    string inviteUrl = StringUtil.EnsureTrailingSlash(httpUtil.GetAppBaseUrl()) + "invite/" + invite.Invite.Hash;
                    var model = mapper.Map<Invitation>(invite.Invite);
                    _ = int.TryParse(createdBy, out int addedBy);
                    model.CreatedBy = addedBy;
                    model.UserSpace = UserSpace.Client;
                    model.UserId = clientCreated.UserId;
                    model.InviteLink = inviteUrl;
                    model.InviteFrom = Enums.InviteSendFrom.Client.ToString();

                    await unitOfWork.InvitationRepo.Add(model);

                    // send email
                    var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    string filePath = Path.GetFullPath(Path.Combine(dir, @"EmailContents\ClientInvitation.txt"));
                    var clientname = "";
                    if(client != null)
                    {
                        clientname = client.Name;
                    }
                    else
                    {
                        clientname = "clientName";
                    }
                    Dictionary<string, string> placeHolders = new()
                    {
                        { "ClientName", clientname },
                        { "inviteLink", inviteUrl },
                        { "SupportPhone", GlobalFunctions.SupportPhone },
                        { "SupportMail", GlobalFunctions.SupportMail },
                    };
                    await emailSender.SendEmailUsingFile(filePath, Subjects.CreateClient, placeHolders, new List<string> { model.Email});
                }
            }
        }
    }
}
