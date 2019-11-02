using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTesting.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebTesting.Controllers
{
   // [Authorize(Roles ="admin")]
   //i want to make custom authorization method becouse of batter understanding
   [Authorize]
    public class AdministrationController : Controller
    {      
        // GET: Test
        private TestingDbContext db = new TestingDbContext();
        private ApplicationDbContext ap = new ApplicationDbContext();
        private UserManager<ApplicationUser> um;
        private RoleManager<IdentityRole> rm;

        public AdministrationController()
        {
            um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ap));
            rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ap));
        }
        public ActionResult Index()
        {
            if (!CheckAuthorizaton())
                return RedirectToAction("Login", "Account");


            ViewBag.Questions = db.Questions.ToList();
            return View(db.Tests.ToList());
        }

        [Authorize(Roles ="admin")]
        public PartialViewResult _ReturnQuestions(int id=0)
        {
            if (id < 0)
                return null;

            IQueryable<Question> questions = db.Questions;

            if(id>0)
            {
                questions = questions.Where(q => q.TestId == id).Select(q => q);
            }


            return PartialView(questions.ToList());
        }
        //get
        public ActionResult Create()
        {
            if (!CheckAuthorizaton())
                return RedirectToAction("Login", "Account");

            return View();
        }
           
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Test t)
        {
            if (!CheckAuthorizaton())
                return RedirectToAction("Login", "Account");

            
            if(ModelState.IsValid)
            {
                try
                {
                    db.Entry(t).State = EntityState.Added;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Administration");
                }
                catch (Exception xcp)
                {
                    db.Entry(t).State = EntityState.Detached;
                    ViewBag.error = xcp.Message;
                    return View("Error");
                }
            }

            return View(t);
        }

        public ActionResult Edit(int? testId)
        {
            if (!CheckAuthorizaton())
                return RedirectToAction("Login", "Account");

            if (testId == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Test t = db.Tests.Find(testId);
            if(t==null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);

            return View(t);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Test t)
        {
            if (!CheckAuthorizaton())
                return RedirectToAction("Login", "Account");

            if(ModelState.IsValid)
            {
                try
                {
                    db.Entry(t).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Administration");
                }
                catch (Exception xcp)
                {
                    db.Entry(t).State = EntityState.Unchanged;
                    ViewBag.error = xcp.Message;
                    return View("Error");
                }
            }
            return View(t);
        }

        public ActionResult Delete(int? testId)
        {
            if (!CheckAuthorizaton())
                return RedirectToAction("Login", "Account");

            if (testId == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Test t = db.Tests.Find(testId);
            if (t == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);

            return View(t);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Test t)
        {
            if (!CheckAuthorizaton())
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(t).State = EntityState.Deleted;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Administration");
                }
                catch (Exception xcp)
                {
                    db.Entry(t).State = EntityState.Unchanged;
                    ViewBag.error = xcp.Message;
                    return View("Error");
                }
            }
            return View(t);
        }
        //get
        public ActionResult AddQuestion()
        {
            if (!CheckAuthorizaton())
                return RedirectToAction("Login", "Account");

            ViewBag.Tests = db.Tests.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestion(Question q)
        {
            if (!CheckAuthorizaton())
                return RedirectToAction("Login", "Account");

            if(ModelState.IsValid)
            {
                int questionNumber = db.Questions.Max(q1 => q1.QuestionNumber);
                q.QuestionNumber = ++questionNumber;
                try
                {
                    db.Entry(q).State = EntityState.Added;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Administration");
                }
                catch (Exception xcp)
                {
                    db.Entry(q).State = EntityState.Detached;
                    ViewBag.error = xcp.Message;
                    return View("Error");
                }
            }

            return View(q);
        }

        //get
        public ActionResult EditQuestion(int? id)
        {
            if (!CheckAuthorizaton())
                return RedirectToAction("Login", "Account");

            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Question q = db.Questions.Find(id);
            if(q==null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);

            ViewBag.Tests=db.Tests.ToList();
            ViewBag.Id = id;
            ViewBag.CorrectAnswer = q.CorrectAnswer;
            return View(q);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuestion(Question q)
        {
            if (!CheckAuthorizaton())
                return RedirectToAction("Login", "Account");

            if(ModelState.IsValid)
            {
                try
                {
                    db.Entry(q).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Administration");
                }
                catch (Exception xcp)
                {
                    ViewBag.error = xcp.Message;
                    return View("Error");
                }
            }

            return View(q);
        }


        public ActionResult DeleteQuestion(int?id)
        {
            if (!CheckAuthorizaton())
                return RedirectToAction("Login", "Account");

            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Question q = db.Questions.Find(id);
            if (q == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);

            return View(q);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteQuestion(Question q)
        {
            if (!CheckAuthorizaton())
                return RedirectToAction("Login", "Account");
            
                try
                {
                    db.Entry(q).State = EntityState.Deleted;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Administration");
                }
                catch (Exception xcp)
                {
                    db.Entry(q).State = EntityState.Unchanged;
                    ViewBag.error = xcp.Message;
                    return View("Error");
                }
                      
        }
        private bool CheckAuthorizaton()
        {
            var user = User.Identity;
            var roles = um.GetRoles(user.GetUserId());

            string role = roles.SingleOrDefault(r => r == "admin");

            if (role != null)
                return true;

            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
