using Converter.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Converter.Controllers
{
    public class ConvertController : Controller
    {
        private CurrencyConverterDBEntities dbContext = new CurrencyConverterDBEntities();

        // GET: Convert
        public ActionResult Index()
        {
            var codeList = dbContext.Rates.Select(c => c.code).ToList();
            ViewBag.data = codeList;
            if (TempData["result"] != null)
            {
                ViewBag.result = TempData["result"].ToString();
                TempData.Remove("result");
            }
            
            return View();
        }

        [HttpPost]
        public ActionResult Index(ConvertViewModel data)
        {
            if(data.TakenCur == data.GivenCur)
            {
                TempData["result"] = data.TakenAmount;
                SaveRecord(data, data.TakenAmount);
            }
            else
            {
                var result = Calculate(data.TakenCur, data.GivenCur) * data.TakenAmount;
                TempData["result"] = result;
                SaveRecord(data, result);
            }      
            
            return RedirectToAction("Index");
        }

        private decimal Calculate(string TakenCur, string GivenCur)
        {
            var from = (from rate in dbContext.Rates
                       where rate.code == TakenCur
                       select rate.sRate).Single();
            var to = (from rate in dbContext.Rates
                     where rate.code == GivenCur
                     select rate.bRate).Single();
            return Math.Round(from / to, 4);
        }

        private void SaveRecord(ConvertViewModel data, decimal money)
        {
            Record record = new Record()
            {
                takenCur = data.TakenCur,
                givenCur = data.GivenCur,
                takenAmount = data.TakenAmount,
                givenAmount = money,
                date = DateTime.Now,
                comment = data.Comment
            };

            try
            {
                dbContext.Records.Add(record);
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
            }
        }
    }
}