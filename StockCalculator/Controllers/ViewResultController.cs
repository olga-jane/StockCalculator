﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockCalculator.Controllers
{
    public class ViewResultController : Controller
    {
        // GET: ViewResult
        public ActionResult Index()
        {
            return View("ViewResult");
        }
    }
}