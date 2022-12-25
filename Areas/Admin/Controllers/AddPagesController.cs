using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace StudentTask.Areas.Admin.Controllers
{
    public class AddPagesController : Controller
    {
        // GET: Admin/AddPages
        private MyContext db = new MyContext();

        IAddPageRepository addPageRepository;
        public AddPagesController()
        {
            addPageRepository = new AddPageRepository(db);
        }
        public ActionResult Index()
        {
            return View(addPageRepository.GetAllStudent());
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(AddPage addPage)
        {
            if (ModelState.IsValid)
            {
                addPage.Address = Corroction(addPage);
                
                addPageRepository.InsertSt(addPage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(addPage);
}

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AddPage addPage = db.AddPages.Find(id);
            if (addPage == null)
            {
                return HttpNotFound();
            }
            return View(addPage);
        }
        [HttpPost]
        public ActionResult Edit(AddPage addPage)
        {
            addPage.Address = Corroction(addPage);

            addPageRepository.UpdateSt(addPage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            AddPage addPage = addPageRepository.GetStById(id);
            addPageRepository.DeleteSt(addPage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public static string Corroction(AddPage addPage)
        {
            string[] sWord = addPage.Address.Split(new char[] { ',', '-', '*', '#', '.', '_' });

            addPage.Address = string.Join(" ", sWord);

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            addPage.Address = regex.Replace(addPage.Address, " ");
            string rep = addPage.Address;

            if (rep.ToLower().Contains("avenue"))
                rep = rep.Replace("avenue", "AVE");


            if (rep.ToLower().Contains("apartment"))
                rep = rep.Replace("avenue", "APT");
            if (rep.ToLower().Contains("street"))
                rep = rep.Replace("avenue", "ST");
            if (rep.ToLower().Contains("road"))
                rep = rep.Replace("avenue", "RD");
            if (rep.ToLower().Contains("boulevard"))
                rep = rep.Replace("avenue", "BLVD");
            if (rep.ToLower().Contains("south"))
                rep = rep.Replace("avenue", "S");
            if (rep.ToLower().Contains("north"))
                rep = rep.Replace("avenue", "N");
            if (rep.ToLower().Contains("west"))
                rep = rep.Replace("avenue", "W");
            if (rep.ToLower().Contains("east"))
                rep = rep.Replace("avenue", "E");
            if (rep.ToLower().Contains("no"))
                rep = rep.Replace("avenue", "");
            if (rep.ToLower().Contains("lane"))
                rep = rep.Replace("avenue", "LN");

            addPage.Address = rep;
            return addPage.Address;
        }
            
    }
    
}

