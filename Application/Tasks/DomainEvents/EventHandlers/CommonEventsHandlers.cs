using Application.DomainEventFramework.Listeners;
using Application.SupportiveBL.Email;
using Application.SupportiveBL.UserInvitation;
using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using Org.BouncyCastle.Tsp;
using Persistence.DAL;
using Persistence.Migrations;
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
    public class CommonEventsHandlers
    {
        private readonly IUnitOfWork work;
        private readonly IEmailSender emailSender;
        private readonly InviteFactory inviteFactory;
        private readonly IMapper mapper;
        private readonly HttpUtil httpUtil;
        public CommonEventsHandlers(IUnitOfWork work, IEmailSender emailSender, InviteFactory inviteFactory, HttpUtil httpUtil, IMapper mapper)
        {
            this.work = work;
            this.emailSender = emailSender;
            this.inviteFactory = inviteFactory;
            this.httpUtil = httpUtil;
            this.mapper = mapper;
        }
        public async Task CommonCommandHandler(DomainEventVM domainEvent)
        {
            if (domainEvent.MailFor != null)
            {
                // send email
                var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string filePath = "";
                string inviteUrl = "";
                string subject = "";

                if (domainEvent.MailFor == Enums.MailFor.Client.ToString())
                {
                    filePath = Path.GetFullPath(Path.Combine(dir, @"EmailContents\ClientAlert.txt"));
                    subject = Subjects.Alert;
                }
                if (domainEvent.MailFor == Enums.MailFor.Organisation.ToString())
                {
                    filePath = Path.GetFullPath(Path.Combine(dir, @"EmailContents\OrgGreeting.txt"));
                    subject = Subjects.Greetings;
                }
                if (domainEvent.MailFor == Enums.MailFor.SystemAdmin.ToString())
                {
                    filePath = Path.GetFullPath(Path.Combine(dir, @"EmailContents\SystemAdminAlert.txt"));
                    subject = Subjects.Alert;
                }
                if (domainEvent.MailFor == Enums.MailFor.PasswordReset.ToString())
                {
                    filePath = Path.GetFullPath(Path.Combine(dir, @"EmailContents\PasswordResetAlert.txt"));
                    // generate invitation here.
                    var uInvitation = inviteFactory.ResolveInvitation(InvitationType.Password);
                    var invite = uInvitation.GenerateInvitation(domainEvent.Email);

                    if (invite.Success)
                    {
                        inviteUrl = StringUtil.EnsureTrailingSlash(httpUtil.GetAppBaseUrl()) + "invite/" + invite.Invite.Hash;
                        var userdata = await work.UserInfos.GetUserDataByEmail(domainEvent.Email);
                        var userid = 0;
                        if(userdata != null)
                        {
                            userid = userdata.UserID;
                        }
                        var model = mapper.Map<Invitation>(invite.Invite);
                        model.CreatedBy = domainEvent.UserId;
                        model.UserSpace = UserSpace.Client;
                        model.UserId = userid;
                        model.InviteLink = inviteUrl;
                        model.InviteFrom = Enums.InviteSendFrom.PasswordReset.ToString();
                        await work.InvitationRepo.Add(model);
                        subject = Subjects.PasswordReset;
                    }
                    var delist = new List<DomainEventVM>();
                    delist.Add(domainEvent);
                    Dictionary<string, string> placeHolders = new()
                    {
                        { "UserName", domainEvent.UserName },
                        { "AdminName", domainEvent.AdminName },
                        { "OrgName", domainEvent.OrgName },
                        { "ClientName", domainEvent.ClientName },
                        { "Email", domainEvent.Email },
                        { "Phone", domainEvent.Phone },
                        { "Address", domainEvent.Address },
                        { "SupportMail", GlobalFunctions.SupportMail },
                        { "SupportPhone", GlobalFunctions.SupportPhone },
                        { "inviteLink", inviteUrl }
                    };
                    if (domainEvent.Email != null && filePath != "")
                    {
                        await emailSender.SendEmailUsingFile(filePath, subject, placeHolders, delist);
                    }
                }
            }
        }
    }
}
