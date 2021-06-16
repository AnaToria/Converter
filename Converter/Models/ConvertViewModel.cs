using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Converter.Models
{
    public class ConvertViewModel
    {
        public string TakenCur { get; set; }
        public string GivenCur { get; set; }
        public decimal TakenAmount { get; set; }
        public string Comment { get; set; }
    }
}