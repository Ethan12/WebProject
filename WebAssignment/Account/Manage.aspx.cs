using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using WebAssignment.Models;
using System.Diagnostics;

namespace WebAssignment.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected string SuccessMessage
        {
            get;
            private set;
        }

        private bool HasPassword(ApplicationUserManager manager)
        {
            return manager.HasPassword(User.Identity.GetUserId());
        }

        public bool HasPhoneNumber { get; private set; }

        public bool TwoFactorEnabled { get; private set; }

        public string SocietyNames { get; set; }

        public bool hasSocieties { get; private set; }

        public string ClubNames { get; set; }

        public bool hasClubs { get; set; }

        public bool TwoFactorBrowserRemembered { get; private set; }

        public int LoginsCount { get; set; }

        protected void Page_Load()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            HasPhoneNumber = String.IsNullOrEmpty(manager.GetPhoneNumber(User.Identity.GetUserId()));

            // Enable this after setting up two-factor authentientication
            //PhoneNumber.Text = manager.GetPhoneNumber(User.Identity.GetUserId()) ?? String.Empty;

            TwoFactorEnabled = manager.GetTwoFactorEnabled(User.Identity.GetUserId());

            LoginsCount = manager.GetLogins(User.Identity.GetUserId()).Count;

            string societies = null;

            try
            {
                societies = (from s in db.AspNetUsers
                             where s.Id == User.Identity.GetUserId()
                             select s.Societies).FirstOrDefault().ToString();
            }
            catch (Exception qe)
            {
                Debug.WriteLine(qe.Message);
            }

            if (!String.IsNullOrEmpty(societies))
            {
                string removedComma = societies.Substring(0, societies.Length - 1);
                string[] splitSocieties = removedComma.Split(',');

                for (int x = 0; x < splitSocieties.Count(); x++)
                {
                    string societyName = "";
                    try
                    {
                        societyName = (from n in db.Societies
                                       where n.Id == Convert.ToInt32(splitSocieties[x])
                                       select n.Name).FirstOrDefault().ToString();

                    }
                    catch (Exception se)
                    {
                        Debug.WriteLine(se.Message);
                    }

                    if (!(x == splitSocieties.Count() - 1))
                    {
                        SocietyNames = SocietyNames + societyName + ", ";
                    }
                    else
                    {
                        SocietyNames = SocietyNames + societyName;
                    }
                }

                hasSocieties = true;

            }
            else
            {
                hasSocieties = false;
            }

            string clubs = null;

            try
            {
                clubs = (from s in db.AspNetUsers
                         where s.Id == User.Identity.GetUserId()
                         select s.Clubs).FirstOrDefault().ToString();
            }
            catch (Exception qe)
            {
                Debug.WriteLine(qe.Message);
            }

            if (!String.IsNullOrEmpty(clubs))
            {
                string removedComma = clubs.Substring(0, clubs.Length - 1);
                string[] splitClubs = removedComma.Split(',');

                for (int x = 0; x < splitClubs.Count(); x++)
                {
                    string clubName = "";
                    try
                    {
                        clubName = (from n in db.Clubs
                                    where n.Id == Convert.ToInt32(splitClubs[x])
                                    select n.Name).FirstOrDefault().ToString();

                    }
                    catch (Exception se)
                    {
                        Debug.WriteLine(se.Message);
                    }

                    if (!(x == splitClubs.Count() - 1))
                    {
                        ClubNames = ClubNames + clubName + ", ";
                    }
                    else
                    {
                        ClubNames = ClubNames + clubName;
                    }
                }

                hasClubs = true;

            }
            else
            {
                hasClubs = false;
            }

            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            if (!IsPostBack)
            {
                // Determine the sections to render
                if (HasPassword(manager))
                {
                    ChangePassword.Visible = true;
                }
                else
                {
                    CreatePassword.Visible = true;
                    ChangePassword.Visible = false;
                }

                // Render success message
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Strip the query string from action
                    Form.Action = ResolveUrl("~/Account/Manage");

                    SuccessMessage =
                        message == "ChangePwdSuccess" ? "Your password has been changed."
                        : message == "SetPwdSuccess" ? "Your password has been set."
                        : message == "RemoveLoginSuccess" ? "The account was removed."
                        : message == "AddPhoneNumberSuccess" ? "Phone number has been added"
                        : message == "RemovePhoneNumberSuccess" ? "Phone number was removed"
                        : String.Empty;
                    successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                }
            }
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // Remove phonenumber from user
        protected void RemovePhone_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var result = manager.SetPhoneNumber(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return;
            }
            var user = manager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                Response.Redirect("/Account/Manage?m=RemovePhoneNumberSuccess");
            }
        }

        // DisableTwoFactorAuthentication
        protected void TwoFactorDisable_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            manager.SetTwoFactorEnabled(User.Identity.GetUserId(), false);

            Response.Redirect("/Account/Manage");
        }

        //EnableTwoFactorAuthentication 
        protected void TwoFactorEnable_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            manager.SetTwoFactorEnabled(User.Identity.GetUserId(), true);

            Response.Redirect("/Account/Manage");
        }
    }
}