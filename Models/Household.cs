using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnterpriseFinancialApp.Models
{
    public class Household
    {
        public Household()
        {
            Categories = new HashSet<Category>();
            Budgets = new HashSet<Budget>();
            Accounts = new HashSet<PersonalAccount>();
            Members = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<PersonalAccount> Accounts { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }

    }
}