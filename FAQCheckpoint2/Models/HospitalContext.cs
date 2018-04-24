using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace FAQCheckpoint2.Models
{
    public class HospitalContext : DbContext
    {
        public DbSet<FAQ> FAQS { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Volunteers> Volunteers { get; set; }
        public DbSet<Contact_messages> contactMessages { get; set; }
    }
}