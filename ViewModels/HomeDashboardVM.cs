using EnterpriseFinancialApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnterpriseFinancialApp.ViewModels
{
    public class HomeDashboardVM
    {
        public ICollection<PersonalAccount> PersonalAccounts { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Budget> Budgets { get; set; }

        public Transaction Transaction { get; set; }

    }
}