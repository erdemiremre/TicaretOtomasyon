using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasyon.Models.Siniflar;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class CariController : Controller
    {
        // GET: Cari
        Context c = new Context();
        public ActionResult Index()
        {
            var degerler = c.Carilers.Where(x => x.Durum == true).ToList();
            return View(degerler);
        }
        [HttpGet]
        public ActionResult CariEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CariEkle(Cariler p)
        {
            p.Durum = true;
            c.Carilers.Add(p);
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult CariSil(int id)
        {
            var cr = c.Carilers.Find(id);
            cr.Durum = false;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult CariGetir(int id)
        {
            var cari = c.Carilers.Find(id);
            return View("CariGetir", cari);
        }
        public ActionResult CariGuncelle(Cariler d)
        {
            if (!ModelState.IsValid)
            {
                return View("CariGetir");
            }
            var crl = c.Carilers.Find(d.Cariid);
            crl.CariAd = d.CariAd;
            crl.CariSoyad = d.CariSoyad;
            crl.CariSehir = d.CariSehir;
            crl.CariMail = d.CariMail;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult MusteriSatıs(int id)
        {
            var degerler = c.SatisHarekets.Where(x => x.Cariid == id).ToList();
            var cr = c.SatisHarekets.Where(x => x.Cariid== id).Select(y => y.Cariler.CariAd + " " + y.Cariler.CariSoyad).FirstOrDefault();
            ViewBag.cari = cr;
            return View(degerler);
        }
    }
}