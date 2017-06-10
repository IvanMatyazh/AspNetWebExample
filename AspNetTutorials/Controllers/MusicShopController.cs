using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AspNetTutorials.Models;
using IpSniffer;

namespace AspNetTutorials.Controllers
{
    public class MusicShopController : Controller
    {
        private MusicShopDbContext db = new MusicShopDbContext();

        // GET: MusicShop
        public ActionResult Index(string type, string searchString)
        {
            string ipAddress = IpGetter.GetIPAddress();
            IpRecord record = db.IpRecords.Where(ip => ip.IpAddress == ipAddress).SingleOrDefault();
            if(record == null)
            {
                IpRecord newRecord = new IpRecord()
                {
                    IpAddress = ipAddress,
                    TimeOfRecord = DateTime.UtcNow,
                    LastTimeOfIssue = DateTime.UtcNow
                };
                db.IpRecords.Add(newRecord);
                db.SaveChanges();
            }
            else
            {
                record.LastTimeOfIssue = DateTime.UtcNow;
                db.Entry(record).State = EntityState.Modified;
                db.SaveChanges();
            }

            InstrumentType insType = InstrumentType.Guitar;

            ViewBag.type = new SelectList(Enum.GetValues(typeof(InstrumentType)));

            var instruments = from ins in db.Instruments
                              select ins;

            if (!string.IsNullOrEmpty(searchString))
            {
                instruments = instruments.Where(s => (s.Manufacturer + " " +s.Model).Contains(searchString));
            }

            if(type!="All" && Enum.TryParse(type, out insType))
            {
                instruments = instruments.Where(s => s.Type == insType);
            }

            return View(instruments);
        }

        // GET: MusicShop/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instrument instrument = db.Instruments.Find(id);
            if (instrument == null)
            {
                return HttpNotFound();
            }
            return View(instrument);
        }

        // GET: MusicShop/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MusicShop/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,Manufacturer,Model,Price")] Instrument instrument)
        {
            if (ModelState.IsValid)
            {
                db.Instruments.Add(instrument);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(instrument);
        }

        // GET: MusicShop/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instrument instrument = db.Instruments.Find(id);
            if (instrument == null)
            {
                return HttpNotFound();
            }
            return View(instrument);
        }

        // POST: MusicShop/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type,Manufacturer,Model,Price")] Instrument instrument)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instrument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(instrument);
        }

        // GET: MusicShop/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instrument instrument = db.Instruments.Find(id);
            if (instrument == null)
            {
                return HttpNotFound();
            }
            return View(instrument);
        }

        // POST: MusicShop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instrument instrument = db.Instruments.Find(id);
            db.Instruments.Remove(instrument);
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
    }
}
