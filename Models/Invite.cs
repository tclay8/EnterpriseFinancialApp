using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnterpriseFinancialApp.Models
{
    public class Invite
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public string Email { get; set; }
        public Guid HHToken { get; set; }
        public DateTime InviteDate { get; set; }
        public string InvitedById { get; set; }
        public bool HasBeenUsed { get; set; }

        public virtual Household Household { get; set; }
        public virtual ApplicationUser InvitedBy { get; set; }

    }
}