using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTesting.Models;
using System.Data.Entity;
using System.Web.SessionState;
using PagedList;
using PagedList.Mvc;

namespace WebTesting.Controllers
{
    public class HomeController : Controller
    {
        private TestingDbContext db = new TestingDbContext();
        private SessionIDManager mId = new SessionIDManager();
        public ActionResult Index()
        {
            Session["Id"] = mId.CreateSessionID(System.Web.HttpContext.Current);
            return View(db.Tests.ToList());
        }

        [Authorize]
        public ActionResult BeginTest(int? testId)
        {
            if (testId == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Test t = db.Tests.Find(testId);
            if (t == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);


            return View(t);
        }

        [HttpPost]
        [Authorize]
        public ActionResult StartTest(int? testId)
        {
            if (testId == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Test test = db.Tests.Find(testId);

            Question question = test.Questions.FirstOrDefault();

            if (question == null)
               return RedirectToAction("Index","Home");


            ViewBag.QuestionNumber = 1;
            return View("CreateQuestion",question);
        }

        //public ActionResult StartTest(int? testId,int? questionNumber)
        //{
        //    if (testId == null)
        //        return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

        //    Test test = db.Tests.Find(testId);

        //    List<Question> listOfQuestion = db.Questions.Where(q => q.TestId == testId).OrderBy(q=>q.QuestionId)
        //        .Select(q => q).ToList();


        //    ViewBag.Testid = testId;
        //    int pageNumber = 1;
        //    if(questionNumber!=null)
        //    {
        //        pageNumber = questionNumber.Value;

        //    }
        //    int pageSize = 1;
        //    return View("CreateQuestion", listOfQuestion.ToPagedList(pageNumber,pageSize));
        //}

        [HttpPost]
        [Authorize]
        public ActionResult CreateQuestion(int? testId,int questionNumber=1,int answer=0)
        {
            if (testId == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Test test = db.Tests.Find(testId);
            if(test==null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            List<Question> questions = test.Questions.ToList();  //all questions from this test
            string sessionId = Session["Id"].ToString();

            Question currentQuestions = questions[questionNumber - 1];

            int result = SaveAnswer(currentQuestions, questionNumber, answer);
            if (result == -1)
                return View("ErrorMsg");

            int numberOfQuestions = questions.Count;

            if(questionNumber==numberOfQuestions)
            {
                decimal points = SaveTestResults(test);
                if(points==-1)
                    return View("ErrorMsg");

                ViewBag.Points = points;

                IQueryable<Testing> testData = db.Testings.Where(ts => ts.SessionId == sessionId).Select(ts => ts);

                return View("FinishLine", testData);
            }
            else
            {
                Question nextQuestion = questions[questionNumber];
                ViewBag.QuestionNumber = ++questionNumber;  //it will first increment
                return View(nextQuestion);
            }
            
          
        }


        private int SaveAnswer(Question currentQuestion,int questionNumber=1,int answer=0)
        {
            string sessionId = Session["Id"].ToString();
            bool rez = false;
            if (currentQuestion.CorrectAnswer == answer)
                rez = true;

            Testing t1 = new Testing
            {
                SessionId = sessionId,
                QuestionId = currentQuestion.QuestionId,
                UserAnswer=answer,
                AnswerCorrectly = rez,
            };

            try
            {
                db.Testings.Add(t1);
                db.SaveChanges();
                return 0;
            }
            catch (Exception)
            {
                db.Entry(t1).State = EntityState.Detached;
                return -1;
            }
        }

        private decimal SaveTestResults(Test test)
        {
            //this method is called when the test is finished

            string sessionId = Session["Id"].ToString();
            List<Question> questions = test.Questions.ToList();
            int nubmerOfQuestions = questions.Count;
            IQueryable<Testing> testData = db.Testings.Where(t => t.SessionId == sessionId)
                .Select(t => t);

            int numberOfCorrectAnswers = testData.Count(t1 => t1.AnswerCorrectly == true);
            decimal points = (numberOfCorrectAnswers / (decimal)nubmerOfQuestions) * 100;

            TestResult tr = new TestResult
            {
                SessionId=sessionId,
                TestId=test.TestId,
                Username="Anonymous",
                FinishTime=DateTime.Now,
                WonPoints=points
            };
            try
            {
                db.TestResults.Add(tr);
                db.SaveChanges();
                return points;
            }
            catch (Exception)
            {
                db.Entry(tr).State = EntityState.Detached;
                return -1;             
            }
           
        }
        public ActionResult About()
        {
            ViewBag.Message = "Onlite objective technichal test.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Reach out to me here.";

            return View();
        }
    }
}