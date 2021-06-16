using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Converter.Controllers
{
    public class RateController : Controller
    {
        private CurrencyConverterDBEntities dbContext = new CurrencyConverterDBEntities();

        // GET: Rate
        public ActionResult Index()
        {
            var rateList = from rate in dbContext.Rates select rate;
            rateList = rateList.OrderBy(x => x.id);

            var viewData = rateList.ToList();

            return View(viewData);
        }

        public ActionResult CreateRate()
        {
            var codeList = dbContext.Currencies.Where(c => !dbContext.Rates.Select(r => r.code).Contains(c.code));
            var viewData = codeList.Select(c => c.code).ToList();
            if (viewData != null)
            {
                ViewBag.data = viewData;
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateRate(Rate rate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbContext.Rates.Add(rate);
                    dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = string.Format("Enter valid currency code");
                }
            }
            catch (Exception)
            {
                ViewBag.Message = string.Format("Error, infromation is not valid or already exists");
                return View();
            }

            return View(rate);
        }

        public ActionResult EditRate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rate rate = dbContext.Rates.Find(id);
            if (rate == null)
            {
                return HttpNotFound();
            }
            return View(rate);
        }

        [HttpPost]
        public ActionResult EditRate(Rate rate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbContext.Entry(rate).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = string.Format("Enter valid currency rates");
                }
            }
            catch (Exception)
            {
                ViewBag.Message = string.Format("Error, try again");
            }
            return View(rate);
        }

        public ActionResult DeleteRate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rate rate = dbContext.Rates.Find(id);
            if (rate == null)
            {
                return HttpNotFound();
            }
            return View(rate);
        }

        [HttpPost, ActionName("DeleteRate")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Rate rate = dbContext.Rates.Find(id);
                dbContext.Rates.Remove(rate);
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                ViewBag.Message = string.Format("Error");
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}