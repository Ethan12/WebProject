using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAssignment.Account
{
    public partial class Join : System.Web.UI.Page
    {
        public DataClasses1DataContext db;
        protected string SuccessMessage
        {
            get;
            private set;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            db = new DataClasses1DataContext();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int clubVal = Convert.ToInt32(ddClubs.SelectedValue);
            int socVal = Convert.ToInt32(ddSocieties.SelectedValue);

            if(clubVal > 0 && socVal >0)
            {
                var query = from student in db.AspNetUsers
                            where student.Id == User.Identity.GetUserId()
                            select student;

                foreach(AspNetUser s in query)
                {
                    s.Societies = string.Format("{0},", socVal);
                    s.Clubs = string.Format("{0},", clubVal);
                }

                try
                {
                    db.SubmitChanges();
                }catch(Exception te)
                {
                    Debug.WriteLine(te.Message);
                }


            }else
            {
                SuccessMessage = "Sorry, something went wrong! Try again!";
            }
        }
    }
}