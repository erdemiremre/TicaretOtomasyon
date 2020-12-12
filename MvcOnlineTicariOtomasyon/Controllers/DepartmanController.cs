using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasyon.Models.Helper;
using MvcOnlineTicariOtomasyon.Models.Siniflar;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class DepartmanController : Controller
    {
        // GET: Departman
        Context c = new Context();
        public ActionResult Index()
        {
            var degerler = c.Departmans.Where(x => x.Durum == true).ToList();
            return View(degerler);
        }
        [HttpGet]
        public ActionResult DepartmanEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DepartmanEkle(Departman d)
        {
            c.Departmans.Add(d);
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DepartmanSil(int id)
        {
            var depart = c.Departmans.Find(id);
            depart.Durum = false;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DepartmanGetir(int id)
        {
            var dpt = c.Departmans.Find(id);
            return View("DepartmanGetir", dpt);
        }
        public ActionResult DepartmanGuncelle(Departman d)
        {
            var dp = c.Departmans.Find(d.Departmanid);
            dp.DepartmanAd = d.DepartmanAd;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DepartmanDetay(int id)
        {
            var degerler = c.Personels.Where(x => x.Departmanid == id).ToList();
            var dpt = c.Departmans.Where(x => x.Departmanid == id).Select(y => y.DepartmanAd).FirstOrDefault();
            ViewBag.d = dpt;
            return View(degerler);
        }
        public ActionResult DepartmanPersonelSatis(int id)
        {
            var degerler = c.SatisHarekets.Where(x => x.Personelid == id).ToList();
            var dpersatis = c.SatisHarekets.Where(x => x.Personelid == id).Select(y => y.Personel.PersonelAd+" "+y.Personel.PersonelSoyad).FirstOrDefault();
            ViewBag.dpers = dpersatis;
            return View(degerler);
        }

        [HttpPost]
        public JsonResult DepartmanEkleJson(Departman departman)
        {
            var hasDepartman = c.Departmans.Where(x => x.DepartmanAd == departman.DepartmanAd).FirstOrDefault();
            if (hasDepartman!=null)
            {
                return Json(new ResultStatusUI()
                {
                    FeedBack = "Aynı kategori mevcut.",
                    Object = null,
                    Result = false,
                });
            }
            c.Departmans.Add(departman);
            departman.Durum = true;
            c.SaveChanges();
            return Json(new ResultStatusUI() 
            {
                FeedBack="Departman eklendi.",
                Object=departman,
                Result=true,           
            });
        }

        [HttpPost]
        public JsonResult DepartmanGuncelleJson(Departman departman)
        {
            var name = c.Departmans.Where(x => x.DepartmanAd == departman.DepartmanAd).FirstOrDefault();
            if (name != null)
            {
                if (name.Departmanid != departman.Departmanid)
                {
                    return Json(new ResultStatusUI()
                    {
                        FeedBack = "Böyle bir departman mevcut.",
                        Object = departman,
                        Result = false,
                    });
                }
            }
            var dp = c.Departmans.Find(departman.Departmanid);
            dp.DepartmanAd = departman.DepartmanAd;
            c.SaveChanges();

            return Json(new ResultStatusUI()
            {
                FeedBack = "Departman güncellendi.",
                Object = departman,
                Result = true,
            });
        }
    } 
}
