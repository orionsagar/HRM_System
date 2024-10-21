using Application.DomainEventFramework.Listeners;
using Application.SupportiveBL.Email;
using Application.SupportiveBL.UserInvitation;
using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Application.Tasks.DomainEvents.EventHandlers
{
    public class GreetingsDomainEventsHandlers
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly InviteFactory inviteFactory;
        private readonly IMapper mapper;
        private readonly IEmailSender emailSender;
        private readonly HttpUtil httpUtil;
        private readonly string baseInviteLink = "https://www.worker";

        public GreetingsDomainEventsHandlers(IUnitOfWork unitOfWork, InviteFactory inviteFactory, IMapper mapper, IEmailSender emailSender, HttpUtil httpUtil)
        {
            this.unitOfWork = unitOfWork;
            this.inviteFactory = inviteFactory;
            this.mapper = mapper;
            this.emailSender = emailSender;
            this.httpUtil = httpUtil;
        }
        public async Task GreetingsHandler(GreetingsDomainEvents greetins)
        {
            // send email
            string filePath = "";
            string inviteUrl = "";
            string subject = "";
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (greetins.DomainEventVM.MailFor == Enums.MailFor.Client.ToString())
            {
                filePath = Path.GetFullPath(Path.Combine(dir, @"EmailContents\ClientGreeting.txt"));
            }
            if (greetins.DomainEventVM.MailFor == Enums.MailFor.Organisation.ToString())
            {
                filePath = Path.GetFullPath(Path.Combine(dir, @"EmailContents\OrgGreeting.txt"));
            }
            if (greetins.DomainEventVM.MailFor == Enums.MailFor.Employee.ToString())
            {
                filePath = Path.GetFullPath(Path.Combine(dir, @"EmailContents\InviteClient.txt"));
                // generate invitation here.
                var uInvitation = inviteFactory.ResolveInvitation(InvitationType.Password);
                var invite = uInvitation.GenerateInvitation(greetins.DomainEventVM.Email);

                if (invite.Success)
                {
                    inviteUrl = StringUtil.EnsureTrailingSlash(httpUtil.GetAppBaseUrl()) + "invite/" + invite.Invite.Hash;
                    var userdata = await unitOfWork.UserInfos.GetUserDataByEmail(greetins.DomainEventVM.Email);
                    var userid = 0;
                    if (userdata != null)
                    {
                        userid = userdata.UserID;
                    }
                    var model = mapper.Map<Invitation>(invite.Invite);
                    model.CreatedBy = greetins.DomainEventVM.UserId;
                    model.UserSpace = UserSpace.Client;
                    model.UserId = userid;
                    model.InviteLink = inviteUrl;
                    model.InviteFrom = Enums.InviteSendFrom.Employee_Create.ToString();
                    await unitOfWork.InvitationRepo.Add(model);
                    subject = Subjects.CreateEmployee;
                }
            }
            Dictionary<string, string> placeHolders = new()
                    {
                        { "OrgName", greetins.DomainEventVM.OrgName },
                        { "ClientName", greetins.DomainEventVM.ClientName },
                        { "SupportPhone", GlobalFunctions.SupportPhone },
                        { "SupportMail", GlobalFunctions.SupportMail },
                        { "inviteLink", inviteUrl },
                    };
            if (greetins.DomainEventVM.Email != null)
            {
                await emailSender.SendEmailUsingFile(filePath, Subjects.Greetings, placeHolders, new List<string> { greetins.DomainEventVM.Email });
            }


        }
    }
}
