using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasyon.Models.Siniflar;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class FaturaController : Controller
    {
        // GET: Fatura
        Context c = new Context();
        public ActionResult Index()
        {
            var liste = c.Faturalars.ToList();
            return View(liste);
        }
        [HttpGet]
        public ActionResult FaturaEkle()
        {
            List<SelectListItem> deger1 = (from x in c.Personels.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.PersonelAd+" "+x.PersonelSoyad+"/"+x.Departman.DepartmanAd,
                                               Value = (x.PersonelAd+" "+x.PersonelSoyad).ToString()
                                           }).ToList();
            ViewBag.dgr1 = deger1;
            return View();
        }
        [HttpPost]
        public ActionResult FaturaEkle(Faturalar f)
        {
            c.Faturalars.Add(f);
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult FaturaGetir(int id)
        {
            var ftr = c.Faturalars.Find(id);
            return View("FaturaGetir", ftr);
        }
        public ActionResult FaturaGuncelle(Faturalar f)
        {
            var fatura = c.Faturalars.Find(f.Faturaid);
            fatura.FaturaSeriNo = f.FaturaSeriNo;
            fatura.FaturaSıraNo = f.FaturaSıraNo;
            fatura.VergiDairesi = f.VergiDairesi;
            fatura.Saat = f.Saat;
            fatura.Tarih = f.Tarih;
            fatura.TeslimAlan = f.TeslimAlan;
            fatura.TeslimEden = f.TeslimEden;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult FaturaDetay(int id)
        {
            var degerler = c.FaturaKalems.Where(x => x.Faturaid == id).ToList();
            var dpt = c.Faturalars.Where(x => x.Faturaid == id).Select(y => y.FaturaSeriNo+"/"+y.FaturaSıraNo+"/"+ y.VergiDairesi).FirstOrDefault();
            ViewBag.d = dpt;
            return View(degerler);
        }
        [HttpGet]
        public ActionResult YeniKalem()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniKalem(FaturaKalem p)
        {
            c.FaturaKalems.Add(p);
            c.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}