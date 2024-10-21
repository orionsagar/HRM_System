using Application.SupportiveBL.Email;
using Application.SupportiveBL.UserInvitation;
using AutoMapper;
using Domains.Models;
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
    public class OrganisationEventHandlers
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly InviteFactory inviteFactory;
        private readonly IMapper mapper;
        private readonly IEmailSender emailSender;
        private readonly HttpUtil httpUtil;
        private readonly string baseInviteLink = "https://www.worker";

        public OrganisationEventHandlers(IUnitOfWork unitOfWork, InviteFactory inviteFactory, IMapper mapper, IEmailSender emailSender, HttpUtil httpUtil)
        {
            this.unitOfWork = unitOfWork;
            this.inviteFactory = inviteFactory;
            this.mapper = mapper;
            this.emailSender = emailSender;
            this.httpUtil = httpUtil;
        }

        public async Task OrganisationCreatedHandler(OrganisationCreatedDomainEvent organisationCreated, string createdBy = "")
        {
            if (organisationCreated.UserId != 0)
            {
                var org = await unitOfWork.OrgRepo.GetById(organisationCreated.OrgId);
                var user = await unitOfWork.UserInfos.GetById(organisationCreated.UserId);

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
                    model.UserId = organisationCreated.UserId;
                    model.InviteLink = inviteUrl;
                    model.InviteFrom = Enums.InviteSendFrom.Organisation.ToString();

                    await unitOfWork.InvitationRepo.Add(model);

                    // send email
                    var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    string filePath = Path.GetFullPath(Path.Combine(dir, @"EmailContents\OrgInvitation.txt"));
                    var orgname = "";
                    if(org != null)
                    {
                        orgname = org.OrgName;
                    }
                    else
                    {
                        orgname = "OrgName";
                    }
                    Dictionary<string, string> placeHolders = new()
                    {
                        { "OrgName", orgname },
                        { "inviteLink", inviteUrl },
                        { "SupportPhone", GlobalFunctions.SupportPhone },
                        { "SupportMail", GlobalFunctions.SupportMail },
                    };
                    await emailSender.SendEmailUsingFile(filePath, Subjects.CreateClient, placeHolders, new List<string> { model.Email });
                }
            }
        }
    }
}
