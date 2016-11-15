using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAssignment.Account
{
    public partial class View : System.Web.UI.Page
    {
        protected string ErrorMessage
        {
            get;
            private set;
        }

        public string PageName { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();

            var pageKey = Request.QueryString["p"];

            if(pageKey != null)
            {
                string key = pageKey.Substring(0, 1);
                Debug.WriteLine("KEY letter: " + key);

                switch(key)
                {
                    case "c":
                        //Case Club
                        var query = from club in db.Clubs
                                    where club.PageKey == key
                                    select club;

                        foreach(Club c in query)
                        {
                            PageName = c.Name;
                        }

                        break;

                    case "s":
                        //Case Society
                        Debug.WriteLine("CASE SOCIETY");
                        var query2 = from society in db.Societies
                                    where society.PageKey == key
                                    select society;

                        foreach(Society s in query2)
                        {
                            PageName = s.Name;
                        }
                        break;

                    default:
                        PageName = "404 - Error";
                        ErrorMessage = "The requested page does not exist!";
                        errorMessage.Visible = !String.IsNullOrEmpty(ErrorMessage);
                        break;
                }

            }else
            {
                PageName = "404 - Error";
                ErrorMessage = "The requested page does not exist!";
                errorMessage.Visible = !String.IsNullOrEmpty(ErrorMessage);
            }
        }
    }
}