using Application.DomainEventFramework.Listeners;
using Application.SupportiveBL.Context;
using Application.SupportiveBL.Email;
using Application.SupportiveBL.UserInvitation;
using Application.Tasks.Commands.CUserInfo;
using Application.Tasks.Common;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using Persistence.Migrations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Utility;

namespace Application.Tasks.Handlers.HUserInfo
{
    public class CreateUserInfoCommandHandler : IRequestHandler<CreateUserInfoCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactable transactable;
        private readonly IAuthorizationContext authContext;
        private readonly InviteFactory inviteFactory;
        private readonly IEmailSender emailSender;
        private readonly HttpUtil httpUtil;

        public CreateUserInfoCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthorizationContext authContext, ITransactable transactable, 
            InviteFactory inviteFactory, IEmailSender emailSender, HttpUtil httpUtil)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this.authContext = authContext;
            this.transactable = transactable;
            this.inviteFactory = inviteFactory;
            this.emailSender = emailSender;
            this.httpUtil = httpUtil;
        }

        public async Task<int> Handle(CreateUserInfoCommand request, CancellationToken cancellationToken)
        {
            var comp = _mapper.Map<UserInfo>(request.userInfoViewModel);
            try
            {
                // start transaction
                using var trans = await transactable.BeginNewTransationAsync();

                //var roleId = await _unitOfWork.UserRoles.GetRoleIDBy_U_RoleName(Constants.UserPlatformAdminUniqueRoleName);

                // create a user for client super admin, if provided
                int userId = 0;
                UserInfo userInfo = new()
                {
                    FirstName = request.userInfoViewModel.FirstName,
                    LastName = request.userInfoViewModel.LastName,
                    UserName = request.userInfoViewModel.Email,
                    Email = request.userInfoViewModel.Email,
                    UserStatus = UserStatus.Active.ToString(),
                    RoleID = request.userInfoViewModel.RoleID,
                    AddedBy = authContext.UserId,
                    AddedDate = DateTime.UtcNow,
                    CompId = 1,
                    OrgId = request.userInfoViewModel.OrgId,
                    ClientId = request.userInfoViewModel.ClientId
                };
                var result = await _unitOfWork.UserInfos.AddWithoutTransaction(userInfo);
                userId = userInfo.UserID;


                // generate invitation here.
                var uInvitation = inviteFactory.ResolveInvitation(InvitationType.Password);
                var invite = uInvitation.GenerateInvitation(request.userInfoViewModel.Email);
                string createdBy = "";

                if (invite.Success)
                {
                    string inviteUrl = StringUtil.EnsureTrailingSlash(httpUtil.GetAppBaseUrl()) + "invite/" + invite.Invite.Hash;
                    var model = _mapper.Map<Invitation>(invite.Invite);
                    _ = int.TryParse(createdBy, out int addedBy);
                    model.CreatedBy = addedBy;
                    model.UserSpace = UserSpace.Client;
                    model.UserId = userId;
                    model.InviteLink = inviteUrl;

                    await _unitOfWork.InvitationRepo.Add(model);

                    // send email
                    var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    string filePath = Path.GetFullPath(Path.Combine(dir, @"EmailContents\ClientInvitation.txt"));
                    Dictionary<string, string> placeHolders = new()
                    {
                        { "ClientName", request.userInfoViewModel.FirstName },
                        { "inviteLink", inviteUrl },
                        { "SupportMail", GlobalFunctions.SupportMail },
                        { "SupportPhone", GlobalFunctions.SupportPhone }
                    };
                    await emailSender.SendEmailUsingFile(filePath, Subjects.CreateClient, placeHolders, new List<string> { model.Email });
                }
                await trans.FinishTransactionAsync();
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }

            //comp.Company.Email1 = request.userInfoViewModel.Email;
            //comp.Company.Phone1 = request.userInfoViewModel.Phone;
            //var result = await _unitOfWork.UserInfos.AddWithoutTransaction(comp);

        }
    }
}
