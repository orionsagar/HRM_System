using Application.DomainEventFramework.Core;
using Application.DomainEventFramework.Listeners;
using Application.Tasks.Commands.CClient;
using Application.Tasks.Common;
using Application.Tasks.DomainEvents;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using Persistence.Repository.Organisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility;

namespace Application.Tasks.Handlers.HClient
{
    public class ExternalClientAddCommandHandler : IRequestHandler<ExternalClientAddCommand, string>
    {
        private readonly IUnitOfWork _work;
        private readonly ITransactable transactable;
        private readonly IDomainEventPublish publisher;

        public ExternalClientAddCommandHandler(IUnitOfWork work, ITransactable transactable, IDomainEventPublish publisher)
        {
            _work = work;
            this.transactable = transactable;
            this.publisher = publisher;
        }

        public async Task<string> Handle(ExternalClientAddCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var roletype = "";
                var clientid = 0;
                using var trans = await transactable.BeginNewTransationAsync();
                if (request.ExternalVM.RegType == "Solicitor")
                {
                    roletype = Constants.ClientSuperAdminUniqueRoleName;
                    var client = new Client
                    {
                        Name = request.ExternalVM.FirstName + " " + request.ExternalVM.LastName,
                        Phone1 = request.ExternalVM.Phone,
                        Email1 = request.ExternalVM.Email,
                        SRACode = request.ExternalVM.SRACode,
                        ClientType = "Individual Solicitor",
                        BusinessType = "Private",
                        CountryCode = request.ExternalVM.CountryCode,
                        IsChangeOrg = false,
                        IsFacedPenalty = false,
                    };
                    var result = await _work.ClientRepo.Add(client);
                    clientid = client.ClientID;
                }
                var orgid = 0;
                if (request.ExternalVM.RegType == "Organisation")
                {
                    roletype = Constants.OrgSuperAdminUniqueRoleName;
                    clientid = 9;// this is default client id for system admin
                    var org = new HrmOrganisation
                    {
                        OrgName = request.ExternalVM.OrgName,
                        OrgCode = request.ExternalVM.OrgCode,
                        ClientId = clientid,
                        AddedBy = request.ExternalVM.UserId.ToString(),
                        AddedDate = DateTime.Now,
                        CountryCode = request.ExternalVM.CountryCode,
                        Status = Enums.Status.Active.ToString(),
                        PackageId = 1
                    };
                    await _work.OrgRepo.Add(org);
                    orgid = org.OrgId;
                }
                // ================ Greetings Mail ================== //
                if (request.ExternalVM.RegType == "Organisation" || request.ExternalVM.RegType == "Solicitor")
                {
                    var greetin = new DomainEventVM();
                    if (request.ExternalVM.RegType == "Solicitor")
                    {
                        greetin = new DomainEventVM
                        {
                            ClientName = request.ExternalVM.FirstName + " " + request.ExternalVM.LastName,
                            MailFor = Enums.MailFor.Client.ToString(),
                            Email = request.ExternalVM.Email
                        };
                    }
                    if (request.ExternalVM.RegType == "Organisation")
                    {
                        greetin = new DomainEventVM
                        {
                            OrgName = request.ExternalVM.OrgName,
                            MailFor = Enums.MailFor.Organisation.ToString(),
                            Email = request.ExternalVM.Email,
                            SupportPhone = GlobalFunctions.SupportPhone,
                            SupportMail = GlobalFunctions.SupportMail
                        };
                    }

                    var greetings = new GreetingsDomainEvents(greetin);
                    publisher.Publish(EventNames.Greetings, greetings, request.ExternalVM.UserId.ToString());
                }
                // =========================== Alert Mail ====================== //
                var sysadminroleId = await _work.UserRoles.GetRoleIDBy_U_RoleName(Constants.UserPlatformAdminUniqueRoleName);
                var orgdata = await _work.UserInfos.GetUserBy_OrgId_RoleId_ClientId(0, sysadminroleId, clientid);
                if (request.ExternalVM.RegType == "Organisation" || request.ExternalVM.RegType == "Solicitor")
                {

                    if (request.ExternalVM.RegType == "Organisation")
                    {

                        var domainevent = new DomainEventVM
                        {
                            OrgName = request.ExternalVM.OrgName,
                            ClientName = request.ExternalVM.FirstName + " " + request.ExternalVM.LastName,
                            MailSubject = "Client Alert",
                            MailFor = Enums.MailFor.Client.ToString(),
                            Email = orgdata != null ? orgdata.Email : request.ExternalVM.Email,
                            Phone = request.ExternalVM.Phone,
                            CountryCode = request.ExternalVM.CountryCode,
                            SupportPhone = GlobalFunctions.SupportPhone,
                            SupportMail = GlobalFunctions.SupportMail
                        };
                        var greetings = new CommonEvents(domainevent);

                        if (domainevent.Email != null)
                        {
                            publisher.Publish(EventNames.Alert, greetings, request.ExternalVM.UserId.ToString());
                        }
                    }
                    if (request.ExternalVM.ClientId != 0)
                    {
                        var clientdata = await _work.UserInfos.GetById(orgdata.UserID);
                        if (clientdata != null)
                        {
                            var domainevent1 = new DomainEventVM
                            {
                                OrgName = request.ExternalVM.OrgName,
                                ClientName = request.ExternalVM.FirstName + " " + request.ExternalVM.LastName,
                                MailSubject = "System Admin Alert",
                                MailFor = Enums.MailFor.SystemAdmin.ToString(),
                                Email = orgdata.Email,
                                Phone = request.ExternalVM.Phone,
                                CountryCode = request.ExternalVM.CountryCode,
                                SupportPhone = GlobalFunctions.SupportPhone,
                                SupportMail = GlobalFunctions.SupportMail
                            };
                            var greetings1 = new CommonEvents(domainevent1);
                            if (domainevent1.Email != null)
                            {
                                publisher.Publish(EventNames.Alert, greetings1, request.ExternalVM.UserId.ToString());
                            }
                        }
                    }
                }

                // ================= User Entry ===================== //
                var roleId = await _work.UserRoles.GetRoleIDBy_U_RoleName(roletype);
                UserInfo userInfo = new()
                {
                    FirstName = request.ExternalVM.FirstName,
                    LastName = request.ExternalVM.LastName,
                    UserName = request.ExternalVM.Email,
                    Email = request.ExternalVM.Email,
                    UserStatus = UserStatus.Active.ToString(),
                    Phone = request.ExternalVM.Phone,
                    CountryCode = request.ExternalVM.CountryCode,
                    RoleID = roleId,
                    AddedBy = request.ExternalVM.UserId.ToString(),
                    AddedDate = DateTime.UtcNow,
                    CompId = 1,
                    ClientId = clientid,
                    Password = request.ExternalVM.Password,
                    OrgId = orgid
                };
                await _work.UserInfos.AddWithoutTransaction(userInfo);

                if (request.ExternalVM.RegType == "Organisation")
                {
                    // =============== PayScale Ente ============== //
                    var sbd = new SalaryBreakDown
                    {
                        CompId = 1,
                        ConveyanceRate = 10,
                        MedicalRate = 10,
                        FoodingRate = 10,
                        SalaryRate = 50,
                        HouseRentRate = 20,
                        RuleID = 21,
                        OrgId = orgid,
                        ClientId = clientid
                    };
                    var payscale = new PayScale
                    {
                        PayScaleTypeId = 6,
                        GradeOrScale = 1,
                        InitialPay = 100,
                        ByGross = true,
                        AddedBy = request.ExternalVM.UserId.ToString(),
                        AddedDate = DateTime.Now,
                        CompId = 1,
                        OrgId = orgid,
                        SalaryBreakDown = sbd

                    };
                    await _work.PayScales.AddPayScaleWithSalaryBreakDown(payscale);
                }
                

                await trans.FinishTransactionAsync();


                return "Registered Successfully";
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return ex.InnerException.Message;
                }
                else
                {
                    return ex.Message;
                }

            }

        }
    }
}
