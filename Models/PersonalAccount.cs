using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnterpriseFinancialApp.Models
{
    public class PersonalAccount
    {
        public PersonalAccount()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        [Display(Name = "Reconciled Balanced")]
        public int ReconciledBalance { get; set; }
        public string CreatedById { get; set; }
        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }


        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual Household Household { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
    }
}