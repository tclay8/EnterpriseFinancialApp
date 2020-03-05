using EnterpriseFinancialApp.Helpers;
using EnterpriseFinancialApp.Models;
using EnterpriseFinancialApp.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static EnterpriseFinancialApp.Models.CustomAttributes;

namespace EnterpriseFinancialApp.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AuthorizeHouseholdRequired]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            Models.EmailModel model = new Models.EmailModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email From: <bold>(0)</bold>" +
                        "({1})</p><p>Message:</p><p>{2}</p>";
                    model.Body = "This is a message from your blog site.  The name and" + "the email of the contacting person is above.";

                    var svc = new EmailService();
                    var msg = new IdentityMessage()
                    {
                        Subject = "Contact From Portfolio Site",
                        Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
                        Destination = "jtara95@gmail.com"
                    };

                    await svc.SendAsync(msg);
                    return View();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                    return View(model);
                }
            }
            else { return View(model); }
        }

        private bool ValidInvite(Guid? code, ref string message)
        {
            if ((DateTime.Now - db.Invites.FirstOrDefault(i => i.HHToken == code).InviteDate).TotalDays < 6)
            {
                bool result = db.Invites.FirstOrDefault(i => i.HHToken == code).HasBeenUsed;
                if (result)
                {
                    message = "invalid";
                }
                else
                {
                    message = "valid";
                }

                return !result;
            }
            else
            {
                message = "expired";
                return false;
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateHousehold(HouseholdVM vm)
        {
            Household hh = new Household();
            hh.Name = vm.HHName;
            db.Households.Add(hh);
            db.SaveChanges();

            var user = db.Users.Find(User.Identity.GetUserId());
            hh.Members.Add(user);
            db.SaveChanges();

            await ControllerContext.HttpContext.RefreshAuthentication(user);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult CreateJoinHousehold(Guid? code)
        {
            if (User.Identity.IsInHousehold())
            {
                return RedirectToAction("Index", "Home");
            }

            HouseholdVM vm = new HouseholdVM();

            if (code != null)
            {
                string msg = "";
                if (ValidInvite(code, ref msg))
                {
                    Invite result = db.Invites.FirstOrDefault(i => i.HHToken == code);

                    vm.IsJoinHouse = true;
                    vm.HHId = result.HouseholdId;
                    vm.HHName = result.Household.Name;

                    result.HasBeenUsed = true;

                    ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                    user.InviteEmail = result.Email;
                    db.SaveChanges();
                }
                else
                {
                    return RedirectToAction("InviteError", new { errMsg = msg });
                }
            }
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> JoinHousehold(HouseholdVM vm)
        {
            Household hh = db.Households.Find(vm.HHId);
            var user = db.Users.Find(User.Identity.GetUserId());

            hh.Members.Add(user);
            db.SaveChanges();

            await ControllerContext.HttpContext.RefreshAuthentication(user);

            return RedirectToAction("Index", "Home");
        }

        [ChildActionOnly]
        public PartialViewResult _SideNav()
        {

            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                return PartialView(user);
            }
            return PartialView();
        }

    }
}