using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nfconlab.Models;

namespace nfconlab.Controllers
{
    public class HomeController : Controller
    {
        private QuestionDb db = new QuestionDb();

        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(db.Questions.ToList());
        }

        //
        // GET: /Home/Details/5

        public ActionResult Details(int id = 0)
        {
            QuestionItem questionitem = db.Questions.Find(id);
            if (questionitem == null)
            {
                return HttpNotFound();
            }
            return View(questionitem);
        }

        //
        // GET: /Home/Questions/5

        public string Questions(int id = 0)
        {
            QuestionItem questionitem = db.Questions.Find(id);
            if (questionitem == null)
            {
                return "null";
            }
            JsonResult json = new JsonResult();
            if (questionitem.Image == null) questionitem.Image = "~/Images/noimg.png";
            json.Data = "{"
                                + "\"Date\":\"" + questionitem.Date + "\","
                                + "\"Question\":\"" + questionitem.Question + "\","
                                + "\"Answers\":"
                                + "{"
                                    + "\"Answer1\":\"" + questionitem.Answer1 + "\","
                                    + "\"Answer2\":\"" + questionitem.Answer2 + "\","
                                    + "\"Answer3\":\"" + questionitem.Answer3 + "\","
                                    + "\"Answer4\":\"" + questionitem.Answer4 + "\""
                                + "},"
                                + "\"Image\":\"" + questionitem.Image + "\""
                            + "}";
            return json.Data.ToString();
        }

        //
        // GET: /Home/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Home/Create

        [HttpPost]
        public ActionResult Create(QuestionItem questionitem)
        {
            if (ModelState.IsValid)
            {
                db.Questions.Add(questionitem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(questionitem);
        }

        //
        // GET: /Home/Edit/5

        public ActionResult Edit(int id = 0)
        {
            QuestionItem questionitem = db.Questions.Find(id);
            if (questionitem == null)
            {
                return HttpNotFound();
            }
            return View(questionitem);
        }

        //
        // POST: /Home/Edit/5

        [HttpPost]
        public ActionResult Edit(QuestionItem questionitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(questionitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(questionitem);
        }

        //
        // GET: /Home/Delete/5

        public ActionResult Delete(int id = 0)
        {
            QuestionItem questionitem = db.Questions.Find(id);
            if (questionitem == null)
            {
                return HttpNotFound();
            }
            return View(questionitem);
        }

        //
        // POST: /Home/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            QuestionItem questionitem = db.Questions.Find(id);
            db.Questions.Remove(questionitem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}