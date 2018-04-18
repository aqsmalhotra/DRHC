using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAQCheckpoint2.LIBs
{
    public class Categories
    {

        public string[] file_extensions = new []{"doc","docx", "pdf"};

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

        public SelectList Areas_Of_Interest()
        {
            SelectList a = new SelectList(
                    new List<SelectListItem>
                    {
                                        new SelectListItem {Text = "Clinic Office Help", Value = "Clinic Office Help"},
                                        new SelectListItem {Text = "Waiting room Support", Value = "Waiting room Support"},
                                        new SelectListItem {Text = "Volunteer Childcare Assistant", Value = "Volunteer Childcare Assistant"},
                                        new SelectListItem {Text = "Information Desk and Wayfinding", Value = "Information Desk and Wayfinding"},
                                        new SelectListItem {Text = "Event Organizing", Value = "Event Organizing"},
                    }, "Value", "Text");
            return a;
        }

        public SelectList Shifts()
        {
            SelectList s = new SelectList(
                    new List<SelectListItem>
                    {
                                        new SelectListItem {Text = "Monday Morning", Value = "Monday Morning"},
                                        new SelectListItem {Text = "Monday Afternoon", Value = "Monday Afternoon"},
                                        new SelectListItem {Text = "Monday Night", Value = "Monday Night"},

                                        new SelectListItem {Text = "Tuesday Morning", Value = "Tuesday Morning"},
                                        new SelectListItem {Text = "Tuesday Afternoon", Value = "Tuesday Afternoon"},
                                        new SelectListItem {Text = "Tuesday Night", Value = "Tuesday Night"},

                                        new SelectListItem {Text = "Wednesday Morning", Value = "Wednesday Morning"},
                                        new SelectListItem {Text = "Wednesday Afternoon", Value = "Wednesday Afternoon"},
                                        new SelectListItem {Text = "Wednesday Night", Value = "Wednesday Night"},

                                        new SelectListItem {Text = "Thursday Morning", Value = "Thursday Morning"},
                                        new SelectListItem {Text = "Thursday Afternoon", Value = "Thursday Afternoon"},
                                        new SelectListItem {Text = "Thursday Night", Value = "Thursday Night"},

                                        new SelectListItem {Text = "Friday Morning", Value = "Friday Morning"},
                                        new SelectListItem {Text = "Friday Afternoon", Value = "Friday Afternoon"},
                                        new SelectListItem {Text = "Friday Night", Value = "Friday Night"},

                                        new SelectListItem {Text = "Saturday Morning", Value = "Saturday Morning"},
                                        new SelectListItem {Text = "Saturday Afternoon", Value = "Saturday Afternoon"},
                                        new SelectListItem {Text = "Saturday Night", Value = "Saturday Night"},

                                        new SelectListItem {Text = "Sunday Morning", Value = "Sunday Morning"},
                                        new SelectListItem {Text = "Sunday Afternoon", Value = "Sunday Afternoon"},
                                        new SelectListItem {Text = "Sunday Night", Value = "Sunday Night"},
                                        new SelectListItem {Text = "Flexible", Value = "Flexible"},

                    }, "Value", "Text");
            return s;
        }


    }
}