using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nfconlab.Models;
using System.Web.Security;

namespace nfconlab.Controllers
{
    public class HomeController : Controller
    {
        private QuestionDb db = new QuestionDb();

        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (User.IsInRole("admin"))
                return View(db.Questions.ToList());
            else
            {
                List<QuestionItem> list= db.Questions.ToList();
                foreach (var q in list)
                {
                    q.RightAnswer = "-----";
                }
                return View(list);
            }
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
            if (User.IsInRole("admin"))
                return View(questionitem);
            else{
                var q = questionitem;
                q.RightAnswer = "-----";
                return View(q);
            }
        }                            
        /*
        //
        // POST: /Home/Identify
        // Segédosztály felhasználó azonosításhoz
        public class myJSONforUser
        {
            public myJSONforUser(string id, string date)
            {
                UserID = id;
                Date = date;
            }
            public myJSONforUser()
            {
                UserID = "Semmi";
                Date = "1000-01-01";
            }
            public string UserID { get; set; }
            public string Date { get; set; }
        }
        
        //Felhasználó azonosítás POST függvény
        [HttpPost]
        public string Identify(myJSONforUser json)
        {
            //Ha nem null értéket kapott
            if (json != null)
            {
                //Felhasználó eltárolása
                if(json.UserID.Equals("Semmi"))
                    return "User identification failed: NOT VALID USER";
                int id = int.Parse(json.UserID);
                Session["UserID"] = json.UserID;
                Session["Date"] = json.Date;
                //Sikeres azonosítás
                return "User identification complete";
            }           
            //Azonosítási hiba
            return "User identification failed: NULL JSON";
        }
        */
        //
        // GET: /Home/Questions/5
        //Kérdés lekérése REST API-n keresztül
        public string Questions(int id = 0)
        {
            //Kérdés lekérése az adatbázisból
            QuestionItem questionitem = db.Questions.Find(id);
            //Azonosító elmentése
            Session["QuestionID"] = id;
            if (questionitem == null)
            {
                return "null";
            }
            //Kimeneti JSON megfelelően formázva
            JsonResult json = new JsonResult();
            if (questionitem.Image == null) questionitem.Image = "http://nfconlab.azurewebsites.net/Images/noimg.png";
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
        // POST: /Home/Questions
        //Segédosztály a válaszadáshoz
        public class myJSONforAnswer
        {
            public myJSONforAnswer(string str)
            {
                Answer = str;
            }
            public myJSONforAnswer()
            {
                Answer = "Semmi";
            }
            public string Answer { get; set; }            
        }

        //Válasz adás POST üzenet
        [HttpPost]
        public string Questions(myJSONforAnswer json)
        {
            int id = -1;
            //Ha nem null-t kapott
            if (json != null)
            {
                try
                {
                    //Ha a lekért kérdés létezik
                    id = (int)Session["QuestionID"];
                }
                catch
                {
                    return "NOT VALID QUESTION";
                }
                //Válasz eltárolása
                string answer = json.Answer;

                //Helyes válasz vizsgálata
                QuestionItem questionitem = db.Questions.Find(id);
                string code="false";
                //Ha helyes a válasz
                if (questionitem.RightAnswer.Equals(answer))
                {
                    //Vissza kell adni, hogy jó
                    code = "true";
                    //Hozzá kell adni a megválaszolt kérdésekhez
                    var controller = new PlayerController();
                    controller.AddPoint(questionitem, Session["UserID"]);
                }
                //Következő kérdés, ennek kell visszaadni a pozícióját
                QuestionItem nextQuestion = db.Questions.Find(id++);
                string nextPos = "0.0,0.0";
                if (nextQuestion != null)
                    nextPos = nextQuestion.Location;

                //Visszaadandó JSON a megfelelő formátumban
                JsonResult response = new JsonResult();
                response.Data = "{"
                                     + "\"Response\":\"" + code + "\","
                                     + "\"Position\":\"" + nextPos + "\""
                              + "}";
                return response.Data.ToString();
            }          
            return "NULL JSON";
        }

        //
        // GET: /Home/Create
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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