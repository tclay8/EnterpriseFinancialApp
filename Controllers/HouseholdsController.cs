using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EnterpriseFinancialApp.Helpers;
using EnterpriseFinancialApp.Models;
using Microsoft.AspNet.Identity;

namespace EnterpriseFinancialApp.Controllers
{
    [RequireHttps]
    [Authorize]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Households
        public ActionResult Index()
        {
            return View(db.Households.ToList());
        }

        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {

                db.Households.Add(household);
                db.SaveChanges();

                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);

                user.HouseholdId = household.Id;

                return RedirectToAction("Index");
            }

            return View(household);
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }


        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Invite()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Invite(string email)
        {
            var code = Guid.NewGuid();
            var callbackUrl = Url.Action("CreateJoinHousehold", "Home", new { code = code }, protocol: Request.Url.Scheme);

            EmailService ems = new EmailService();
            IdentityMessage msg = new IdentityMessage();

            msg.Body = "Please join my household..... And bring ALL of your money!!!" + Environment.NewLine + "Please click the following link to join <a href=\"" + callbackUrl + "\">JOIN</a>";
            msg.Destination = email;
            msg.Subject = "Invite to Household";

            await ems.SendMailAsync(msg);

            Invite model = new Invite();
            model.Email = email;
            model.HHToken = code;
            model.HouseholdId = User.Identity.GetHouseholdId().Value;
            model.InviteDate = DateTime.Now;
            model.InvitedById = User.Identity.GetUserId();

            db.Invites.Add(model);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");

        }

        public ActionResult LeaveHousehold()
        {
            Household model = db.Households.Find(User.Identity.GetHouseholdId().Value);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> LeaveHousehold(Household model)
        {
            Household hh = db.Households.Find(model.Id);
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            hh.Members.Remove(user);
            db.SaveChanges();

            await ControllerContext.HttpContext.RefreshAuthentication(user);

            return RedirectToAction("Index", "Home");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
