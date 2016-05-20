using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockCalculator.Controllers
{
    public class EnterDetailsController : Controller
    {
        // GET: EnterDetails
        public ActionResult Index()
        {
            return View("EnterDetails");
        }

        // GET: EnterDetails/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EnterDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EnterDetails/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: EnterDetails/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EnterDetails/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: EnterDetails/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EnterDetails/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
