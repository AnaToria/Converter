using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Converter.Controllers
{
    public class RecordController : Controller
    {
        private CurrencyConverterDBEntities dbContext = new CurrencyConverterDBEntities();

        // GET: Record
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            return Json(dbContext.Records.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSus()
        {
            var result = dbContext.Records.Where(r => r.givenAmount >= 10000 && r.givenCur == "GEL").Select(r => r).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDate(string from, string to)
        {
            DateTime start = DateTime.MinValue;
            DateTime end = DateTime.MaxValue;
            DateTime.TryParse(from, out start);
            DateTime.TryParse(to, out end);

            var result = dbContext.Records.Where(r => r.date >= start && r.date <= end).Select(r => r).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}