using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using WebProj.Models;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Exceptions;

namespace WebProj.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {

            string email = Email.Text;
            string studentNumber = "";
            string domain = "";

            try
            {
                string[] splitEmail = email.Split('@');
                studentNumber = splitEmail[0];
                domain = splitEmail[1];

            }
            catch (Exception ce)
            {
                Debug.WriteLine(ce.Message);
            }

            //Validate Student Number

            Regex rex = new Regex(@"^[a-zA-Z]\d{8}");
            Regex mex = new Regex(@"[a-zA-Z]{4}(.[a-zA-Z]{7})(.[a-zA-Z]{2})");

            if (rex.IsMatch(studentNumber) && mex.IsMatch(domain))
            {
            }
            else
            {
                 ErrorMessage.Text = "Email not in the correct format of S00123456@mail.itsligo.ie";
                 return;
            }

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var user = new ApplicationUser() { UserName = studentNumber, Email = Email.Text };
            IdentityResult result = manager.Create(user, Password.Text);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                //string code = manager.GenerateEmailConfirmationToken(user.Id);
                //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                string code = manager.GenerateEmailConfirmationToken(user.Id);
                try
                {
                    string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                    manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");
                }catch(InvalidApiRequestException iar)
                {
                    Debug.WriteLine(iar.Message);
                }
                if (user.EmailConfirmed)
                {

                    signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                }else
                {
                    ErrorMessage.Text = "An email has been sent to your account. Please view the email and confirm your account to complete the registration process.";
                }
            }
            else 
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }
    }
}