using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Converter.Controllers
{
    public class CurrencyController : Controller
    {
        private CurrencyConverterDBEntities dbContext = new CurrencyConverterDBEntities();

        // GET: Currency
        public ActionResult Index()
        {
            var currencyList = from currency in dbContext.Currencies select currency;
            currencyList = currencyList.OrderBy(x => x.orderNum  == null).ThenBy(x => x.orderNum).ThenBy(x => x.code);
            
            var viewData = currencyList.ToList();

            return View(viewData);
        }

        public ActionResult CreateCurrency()
        {
            var codeList = dbContext.CurrencyLists.Where(c => !dbContext.Currencies.Select(r => r.code).Contains(c.code));
            var viewData = codeList.Select(c => c.code).ToList();
            if (viewData != null)
            {
                ViewBag.data = viewData;
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateCurrency(Currency currency)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbContext.Currencies.Add(currency);
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
            
            return View(currency);
        }
    
        public ActionResult EditCurrency(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Currency currency = dbContext.Currencies.Find(id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        [HttpPost]
        public ActionResult EditCurrency(Currency currency)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbContext.Entry(currency).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ViewBag.Message = string.Format("Error, try again");
            }
            return View(currency);
        }

        public ActionResult DeleteCurrency(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Currency currency = dbContext.Currencies.Find(id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        [HttpPost, ActionName("DeleteCurrency")]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                Currency currency = dbContext.Currencies.Find(id);
                dbContext.Currencies.Remove(currency);
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