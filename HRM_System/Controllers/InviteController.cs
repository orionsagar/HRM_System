using Application.Tasks.Commands.CClient;
using Application.Tasks.Commands.CInvitation;
using Application.Tasks.Queries.QInvitation;
using Domains.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using UKHRM.Helper;
using Utility;

namespace UKHRM.Controllers
{
    [Authorize]
    public class InviteController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<InviteController> _logger;
        private readonly IReCaptchaClass _captchaClass;
        private readonly IGlobalHelper _global;

        public InviteController(IMediator mediator, ILogger<InviteController> logger, IReCaptchaClass captchaClass, IGlobalHelper global)
        {
            _mediator = mediator;
            _logger = logger;
            _captchaClass = captchaClass;
            _global = global;
        }

        [Route("/invite/{hash}")]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string hash)
        {
            PasswordInvitationViewModel passwordInvitation = new PasswordInvitationViewModel();

            if (string.IsNullOrEmpty(hash))
            {
                //Error Messages
            }

            var linkInfo = await _mediator.Send(new GetClientInvitationByHashQuery { Hash = hash });
            if (linkInfo == null)
            {
                return RedirectToAction("Invite", "InvalidInvatition");
            }
            else
            {
                var expireAt = linkInfo?.ExpireAt;

                if (expireAt > DateTime.MinValue)
                {
                    return RedirectToAction("Invite", "InvalidInvatition");
                }
                var linkemail = linkInfo.Email;
                var linkuserid = linkInfo.UserId;
                passwordInvitation.Email = linkemail;
                passwordInvitation.UserID = linkuserid.ToString();
                passwordInvitation.InviteFrom = linkInfo.InviteFrom;
            }
            
            passwordInvitation.Hashkey = hash;

            return View(passwordInvitation);
        }

        [HttpPost]
        [Route("/invite/InvitePost")]
        [AllowAnonymous]
        public async Task<IActionResult> InvitePost(PasswordInvitationViewModel invitationViewModel)
        {
            var EncodedResponse = Request.Form["g-Recaptcha-Response"];
            var IsCaptchaValid = _captchaClass.Validate(EncodedResponse) == "true" ? true : false;
            try
            {
                if (IsCaptchaValid)
                {
                    if (ModelState.IsValid)
                    {
                        await _mediator.Send(new InvitationUpdateCommand { InviteViewModel = invitationViewModel });
                        if(invitationViewModel.InviteFrom == Enums.InviteSendFrom.PasswordReset.ToString())
                        {
                            
                            return RedirectToAction("passwordreset", "Invite");
                        }
                        else
                        {
                            return RedirectToAction("Success", "Invite");
                        }
                        
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. " +
                           "Try again, and if the problem persists " +
                           "see your system administrator.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Unexpected error calling reCAPTCHA api.");
                    return View();
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator.");

            }
            return View();
        }

        [Route("/invite/success")]
        [AllowAnonymous]
        public IActionResult Success()
        {
            return View();
        }
        [Route("/invite/passwordreset")]
        [AllowAnonymous]
        public IActionResult PasswordReset()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult InvalidInvatition()
        {
            return View();
        }

        [Route("/externalinvite/{hash}")]
        [AllowAnonymous]
        public async Task<IActionResult> InviteClient(string hash)
        {
            ClientViewModel client = new ClientViewModel();

            if (string.IsNullOrEmpty(hash))
            {
                //Error Messages
            }

            var linkInfo = await _mediator.Send(new GetClientInvitationByHashQuery { Hash = hash });
            var expireAt = linkInfo.ExpireAt;

            if (expireAt > DateTime.MinValue)
            {
                return RedirectToAction("Invite", "InvalidInvatition");
            }

            var linkemail = linkInfo.Email;
            var linkuserid = linkInfo.CreatedBy;

            client.ClientSAEmail = linkemail;
            client.Email1 = linkemail;

            return View(client);
        }
        [HttpPost]
        [Route("/invite/InviteClient")]
        [AllowAnonymous]
        public async Task<IActionResult> InviteClient(ClientViewModel client)
        {
            //var EncodedResponse = Request.Form["g-Recaptcha-Response"];
            //var IsCaptchaValid = _captchaClass.Validate(EncodedResponse) == "true" ? true : false;
            client.UserId = Convert.ToInt32(_global.GetUserID());
            try
            {
                if (ModelState.IsValid)
                {
                    client.ClientType = client.ClientType == "Others" ? client.ClientType1 : client.ClientType;
                    await _mediator.Send(new AddClientByInvitationCommand { Client = client });

                    return RedirectToAction("Success", "Invite");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                       "Try again, and if the problem persists " +
                       "see your system administrator.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator.");

            }
            return View();
        }
    }
}
