using Application.Tasks.Queries.QCompany;
using Application.Tasks.Queries.QUserInfo;
using Application.Tasks.Queries.QUserRole;
using Domains.ViewModels;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using Application.Tasks.Commands.CClient;
using Domains.Models;
using Application.Tasks.Commands.CUserInfo;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace UKHRM.Controllers
{
    [Authorize]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly CommonDropdown _common;
        private readonly ICustomRepository _custom;
        private readonly ICookieManager _cookieManager;
        private readonly IAuthBL _authbl;
        private readonly ICompanyBL _combl;
        private readonly IReCaptchaClass _captchaClass;
        private readonly IGlobalHelper _global;


        public AuthController(IMediator mediator, IConfiguration configuration,
            CommonDropdown common, ICustomRepository custom, IAuthBL authbl,
            ICompanyBL combl, IReCaptchaClass reCaptchaClass, IGlobalHelper global)
        {
            _mediator = mediator;
            _configuration = configuration;
            _common = common;
            this._custom = custom;
            _authbl = authbl;
            _combl = combl;
            _captchaClass = reCaptchaClass;
            _global = global;
        }

        [AllowAnonymous]
        public IActionResult Login1(string returnUrl)
        {
            GetCompany();
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            GetCompany();
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {

            var EncodedResponse = Request.Form["g-Recaptcha-Response"];
            var IsCaptchaValid = _captchaClass.Validate(EncodedResponse) == "true" ? true : false;
            if (IsCaptchaValid)
            {
                try
                {
                    GetCompany();
                    var companyQ = await _mediator.Send(new GetAllCompanyQuery());
                    if (companyQ != null)
                    {
                        foreach (var item in companyQ)
                        {
                            ViewBag.CompanyLogo = !string.IsNullOrEmpty(item.Logo) ? item.Logo : "";
                            ViewBag.CompanyName = item.Name;
                            ViewBag.Address = item.Address;
                            ViewBag.WebSiteLink = item.Website;
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        //if (!this.IsCaptchaValid("Captcha is not valid"))
                        //{
                        //    ModelState.AddModelError("", "Error: captcha is not valid.");
                        //    return View(new LoginViewModel());
                        //}
                        var userExists = await _mediator.Send(new CheckUsernameExistsQuery() { username = loginViewModel.Username, password = loginViewModel.Password });
                        if (!userExists)
                        {
                            ModelState.AddModelError("", "Invalid Credentails");
                            ViewBag.ErrorMessage = "Invalid Creadentials";
                            //return View(loginViewModel);
                            return View("Login", loginViewModel);
                        }


                        var getUser = await _mediator.Send(new GetUserByUsernameQuery() { username = loginViewModel.Username, password = loginViewModel.Password });
                        var company = await _combl.GetCompanyByComId((int)getUser.CompId);
                        //AesAlgorithm aesAlgorithm = new AesAlgorithm();
                        //var usermasterModel = _iUserMaster.GetUserByUsername(loginViewModel.Username);
                        //var storedpassword = aesAlgorithm.DecryptString(_password.GetPasswordbyUserId(usermasterModel.UserId));

                        //Console.WriteLine("Password:" + storedpassword);
                        if (getUser != null)
                        {
                            CookieOptions option = new CookieOptions();
                            option.Expires = DateTime.Now.AddDays(1);

                            // Claim
                            var userClaims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, getUser.FirstName.ToString()),
                            new Claim(ClaimTypes.Email, getUser.Email.ToString()),
                        };

                            var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });

                            var authProperties = new AuthenticationProperties
                            {
                                //AllowRefresh = <bool>,
                                // Refreshing the authentication session should be allowed.

                                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                                // The time at which the authentication ticket expires. A 
                                // value set here overrides the ExpireTimeSpan option of 
                                // CookieAuthenticationOptions set with AddCookie.

                                //IsPersistent = true,
                                // Whether the authentication session is persisted across 
                                // multiple requests. When used with cookies, controls
                                // whether the cookie's lifetime is absolute (matching the
                                // lifetime of the authentication ticket) or session-based.

                                //IssuedUtc = <DateTimeOffset>,
                                // The time at which the authentication ticket was issued.

                                //RedirectUri = <string>
                                // The full path or absolute URI to be used as an http 
                                // redirect response value.
                            };
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(userPrincipal), authProperties);

                            /// Session
                            HttpContext.Session.SetString("UserID", DataEncryption.EncryptString(getUser.UserID.ToString()));
                            HttpContext.Session.SetString("ClientId", DataEncryption.EncryptString(getUser.ClientId.ToString()));

                            HttpContext.Session.SetString("Username", DataEncryption.EncryptString(getUser.FirstName.ToString().Trim()));
                            HttpContext.Session.SetString("CompID", DataEncryption.EncryptString(companyQ.FirstOrDefault().CompID.ToString()));
                            HttpContext.Session.SetString("ComName", DataEncryption.EncryptString(companyQ.FirstOrDefault().Name.ToString()));

                            HttpContext.Session.SetString("FullName", DataEncryption.EncryptString(getUser.FirstName.ToString() + " " + getUser.LastName?.ToString()));
                            HttpContext.Session.SetString("Email", DataEncryption.EncryptString(getUser.Email?.ToString()));


                            /// Cookies
                            HttpContext.Response.Cookies.Append("UserID", DataEncryption.EncryptString(getUser.UserID.ToString()), option);
                            HttpContext.Response.Cookies.Append("OrgId", DataEncryption.EncryptString(getUser.OrgId.ToString()), option);
                            HttpContext.Response.Cookies.Append("ClientId", DataEncryption.EncryptString(getUser.ClientId.ToString()), option);

                            HttpContext.Response.Cookies.Append("Username", DataEncryption.EncryptString(getUser.FirstName.ToString().Trim()), option);
                            HttpContext.Response.Cookies.Append("CompID", DataEncryption.EncryptString(companyQ.FirstOrDefault().CompID.ToString()), option);
                            HttpContext.Response.Cookies.Append("ComName", companyQ.FirstOrDefault().Name.ToString(), option);

                            HttpContext.Response.Cookies.Append("FullName", DataEncryption.EncryptString(getUser.FirstName.ToString() + " " + getUser.LastName?.ToString()));
                            HttpContext.Response.Cookies.Append("Email", DataEncryption.EncryptString(getUser.Email?.ToString()));

                            // Get the Cookie object
                            //CookieUserInfo objFromCookie = _cookieManager.Get<CookieUserInfo>("Key");

                            // Set the myCookie object
                            //CookieUserInfo userInfo = new CookieUserInfo()
                            //{
                            //    CusID = isValid.CusID,
                            //    Email = isValid.Email,
                            //    Name = isValid.Name
                            //};
                            //_cookieManager.Set("siteKey", userInfo, 60);


                            //Session["UserID"] = usermasterModel.UserId;
                            //Session["Username"] = usermasterModel.UserName;

                            var getRole = await _mediator.Send(new GetUserRoleByIdQuery() { RoleID = (int)getUser.RoleID });

                            if (getRole != null)
                            {
                                HttpContext.Session.SetString("RoleType", DataEncryption.EncryptString(getRole.RoleType));
                                HttpContext.Response.Cookies.Append("RoleType", DataEncryption.EncryptString(getRole.RoleType), option);
                                string landingPage = "";
                                // 1 is SuperAdmin
                                //if (getRole.RoleID == Convert.ToInt32(_configuration.GetSection("RoleSettings").GetSection("SuperAdminRolekey").Value))
                                //{
                                //    HttpContext.Session.SetString("RoleID", DataEncryption.EncryptString(getRole.RoleID.ToString()));
                                //    HttpContext.Session.SetString("Role", getRole.RoleName.ToString());

                                //    HttpContext.Response.Cookies.Append("RoleID", DataEncryption.EncryptString(getRole.RoleID.ToString()), option);
                                //    //===================================Please Decrypt this data and encrypt is when ready ===================================================
                                //    HttpContext.Response.Cookies.Append("Role", getRole.RoleName.ToString());
                                //    //=========================================================================================================================================
                                //    landingPage = getRole.LandingPage.ToString() != null ? getRole.LandingPage.ToString() : "/Dashboard/index";


                                //    if (!String.IsNullOrEmpty(returnUrl))
                                //        returnUrl = HttpContext.Request.Query["ReturnUrl"];
                                //    else
                                //        returnUrl = landingPage;

                                //    return RedirectToLocal(returnUrl);
                                //    //return RedirectToAction("Dashboard", "SuperDashboard");
                                //}

                                //// 2 is User
                                //if (getRole.RoleID == Convert.ToInt32(_configuration.GetSection("RoleSettings").GetSection("UserRolekey").Value))
                                //{
                                //    HttpContext.Session.SetString("RoleID", DataEncryption.EncryptString(getRole.RoleID.ToString()));
                                //    HttpContext.Session.SetString("Role", getRole.RoleName.ToString());

                                //    HttpContext.Response.Cookies.Append("RoleID", DataEncryption.EncryptString(getRole.RoleID.ToString()), option);
                                //    //===================================Please Decrypt this data and encrypt is when ready ===================================================
                                //    HttpContext.Response.Cookies.Append("Role", getRole.RoleName.ToString());
                                //    landingPage = getRole.LandingPage ?? "/Dashboard/index";


                                //    if (!String.IsNullOrEmpty(returnUrl))
                                //        returnUrl = HttpContext.Request.Query["ReturnUrl"];
                                //    else
                                //        returnUrl = landingPage;

                                //    return RedirectToLocal(returnUrl);
                                //    //return RedirectToAction("Dashboard", "UserDashboard");
                                //}

                                //// 3 is Admin
                                //if (getRole.RoleID == Convert.ToInt32(_configuration.GetSection("RoleSettings").GetSection("AdminRolekey").Value))
                                //{
                                //    HttpContext.Session.SetString("RoleID", DataEncryption.EncryptString(getRole.RoleID.ToString()));
                                //    HttpContext.Session.SetString("Role", getRole.RoleName.ToString());

                                //    HttpContext.Response.Cookies.Append("RoleID", DataEncryption.EncryptString(getRole.RoleID.ToString()), option);
                                //    //===================================Please Decrypt this data and encrypt is when ready ===================================================
                                //    HttpContext.Response.Cookies.Append("Role", getRole.RoleName.ToString());
                                //    landingPage = getRole.LandingPage ?? "Dashboard/Index";


                                //    if (!String.IsNullOrEmpty(returnUrl))
                                //        returnUrl = HttpContext.Request.Query["ReturnUrl"];
                                //    else
                                //        returnUrl = landingPage;

                                //    return RedirectToLocal(returnUrl);
                                //    // return RedirectToAction("Dashboard", "AdminDashboard");
                                //}


                                HttpContext.Session.SetString("RoleID", DataEncryption.EncryptString(getRole.RoleID.ToString()));
                                HttpContext.Session.SetString("Role", getRole.RoleName.ToString());

                                HttpContext.Response.Cookies.Append("RoleID", DataEncryption.EncryptString(getRole.RoleID.ToString()), option);
                                //===================================Please Decrypt this data and encrypt is when ready ===================================================
                                HttpContext.Response.Cookies.Append("Role", getRole.RoleName.ToString());
                                var RoleID = !string.IsNullOrEmpty(HttpContext.Request.Cookies["RoleID"]) ? DataEncryption.DecryptString(HttpContext.Request.Cookies["RoleID"]) : "";
                                landingPage = getRole.LandingPage ?? "Dashboard/Index";
                                if (!String.IsNullOrEmpty(HttpContext.Request.Query["ReturnUrl"]))
                                    returnUrl = HttpContext.Request.Query["ReturnUrl"];
                                else
                                    returnUrl = landingPage;
                                return RedirectToLocal(returnUrl);
                            }
                            else
                            {
                                ModelState.AddModelError("", "Access Not Assigned");
                                return View(loginViewModel);
                            }
                            //return RedirectToAction("index", "Dashboard");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Credentails");
                            return View(loginViewModel);
                        }

                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Invalid Creadentials";
                        return View("login", loginViewModel);
                    }

                }
                catch (Exception ex)
                {
                    GetCompany();

                    ModelState.AddModelError("", ex.Message.ToString());
                    return View(loginViewModel);
                }
            }
            else
            {
                ModelState.AddModelError("", "Unexpected error calling reCAPTCHA api.");
                return View(loginViewModel);
            }





            //string recaptchaResponse = this.Request.Form["g-recaptcha-response"];
            //var client = httpClientFactory.CreateClient();

            //    var paramesters = new Dictionary<string, string>
            //{
            //    {"secret", this.configuration["reCAPTCHA:SecretKey"]},
            //    {"response", recaptchaResponse},
            //    {"remoteip", this.HttpContext.Connection.RemoteIpAddress.ToString()}
            //};

            //    HttpResponseMessage response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(parameters));
            //    response.EnsureSuccessStatusCode();

            //string apiResponse = await response.Content.ReadAsStringAsync();
            //dynamic apiJson = JObject.Parse(apiResponse);
            //if (apiJson.success != true)
            //{
            //    this.ModelState.AddModelError(string.Empty, "There was an unexpected problem processing this request. Please try again.");
            //}



        }


        public void GetCompany()
        {
            var companyQ = _mediator.Send(new GetAllCompanyQuery());
            if (companyQ.Result != null)
            {
                foreach (var item in companyQ.Result)
                {
                    ViewBag.CompanyLogo = !string.IsNullOrEmpty(item.Logo) ? item.Logo : "";
                    ViewBag.CompanyName = item.Name;
                    ViewBag.Address = item.Address;
                    ViewBag.WebSiteLink = item.Website;
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {

            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                Response.Cookies.Delete("UserID");
                Response.Cookies.Delete("Username");
                Response.Cookies.Delete("Role");
                Response.Cookies.Delete("AID");
                Response.Cookies.Delete("BID");
                Response.Cookies.Delete("RID");
                Response.Cookies.Delete("RoleID");
                Response.Cookies.Delete("YearID");
                Response.Cookies.Delete("YearCode");
                Response.Cookies.Delete("StartDate");
                Response.Cookies.Delete("EndDate");
                Response.Cookies.Delete("FullName");
                Response.Cookies.Delete("Email");

                HttpContext.Session.Clear();
                HttpContext.Session.Remove("UserID");
                HttpContext.Session.Remove("Username");
                HttpContext.Session.Remove("Role");
                HttpContext.Session.Remove("RoleID");
                HttpContext.Session.Remove("AID");
                HttpContext.Session.Remove("BID");
                HttpContext.Session.Remove("RID");
                HttpContext.Session.Remove("YearID");
                HttpContext.Session.Remove("YearCode");
                HttpContext.Session.Remove("StartDate");
                HttpContext.Session.Remove("EndDate");
                HttpContext.Session.Remove("FullName");
                HttpContext.Session.Remove("Email");

                return RedirectToAction("Login", "Auth");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(ExternalClientVM client)
        {
            try
            {
                var UserId = _global.GetUserID();
                var ClientId = _global.GetClientId();
                var RoleType = _global.GetRoleType();
                client.UserId = Convert.ToInt32(UserId);
                client.ClientId = ClientId;

                var OrgCode = _global.GetOrgCode().Result;
                client.OrgCode = OrgCode.ToString();

                var message = await _mediator.Send(new ExternalClientAddCommand { ExternalVM = client });
                var error = false;
                if (message != "Registered Successfully")
                {
                    error = true;
                }
                return Json(new { message = message, error = error });
            }
            catch (Exception)
            {
                return Json(new { message = "", error = true });
            }

        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            //return RedirectToAction("Index", "Dashboard");
            return RedirectToAction("Index", "Dashboard");
        }
        [AllowAnonymous]
        public IActionResult PasswordReset()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordReset(string Email)
        {
            var data = new DomainEventVM { Email = Email };
            var message = await _mediator.Send(new PasswordResetCommand { DomainEvents = data });
            return Json(message);
        }


        [AllowAnonymous]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ForgetPassword(ForgetPasswordViewModel viewModel)
        {
            var UserId = _global.GetUserID();
            var ClientId = _global.GetClientId();
            var RoleType = _global.GetRoleType();

            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> CheckEmail(string Email)
        {
            var message = await _global.ResetEmailCheck(Email);
            return Json(message);
        }


        //[HttpGet("Captcha")]
        //public async Task<bool> GetreCaptchaResponse(string userResponse)
        //{
        //    var reCaptchaSecretKey = _configuration["reCaptcha:SecretKey"];

        //    if (reCaptchaSecretKey != null && userResponse != null)
        //    {
        //        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        //        {
        //            {"secret", reCaptchaSecretKey },
        //            {"response", userResponse }
        //        });
        //        var response = await _httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var result = await response.Content.ReadFromJsonAsync<reCaptchaResponse>();
        //            return result.Success;
        //        }
        //    }
        //    return false;
        //}

        public class reCaptchaResponse
        {
            public bool Success { get; set; }
            public string[] ErrorCodes { get; set; }
        }
    }
}
