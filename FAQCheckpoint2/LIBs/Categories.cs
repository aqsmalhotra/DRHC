using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAQCheckpoint2.LIBs
{
    public class Categories
    {
        public SelectList Services()
        {
            SelectList j = new SelectList(
                            new List<SelectListItem>
                            {
                                        new SelectListItem {Text = "Recruitment", Value = "Recruitment"},
                                        new SelectListItem {Text = "General", Value = "General"},
                                        new SelectListItem {Text = "Patient Care", Value = "Patient Care"},
                                        new SelectListItem {Text = "Policies", Value = "Policies"},
                                        new SelectListItem {Text = "Visitors & Families", Value = "Visitors & Families"},
                                        new SelectListItem {Text = "Billing", Value = "Billing"},
                                        new SelectListItem {Text = "Miscellaneous", Value = "Miscellaneous"},

                            }, "Value", "Text");
            return j;
        }

        public SelectList User_Roles()
        {
            SelectList u = new SelectList(
                    new List<SelectListItem>
                    {
                                        new SelectListItem {Text = "Admin", Value = "admin"},
                                        new SelectListItem {Text = "Staff", Value = "staff"},
                    }, "Value", "Text");
            return u;
        }
    }
}