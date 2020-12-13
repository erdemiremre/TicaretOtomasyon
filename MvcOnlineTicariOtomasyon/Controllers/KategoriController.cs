using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasyon.Models.Helper;
using MvcOnlineTicariOtomasyon.Models.Siniflar;
using PagedList;
using PagedList.Mvc;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class KategoriController : Controller
    {
        // GET: Kategori
        Context c = new Context();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult KategoriListele()
        {
            var degerler = c.Kategoris.ToList();
            return Json(new ResultStatusUI()
            {
                Result=true,
                Object=degerler,
            });
        }

        [HttpGet]
        public ActionResult KategoriEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KategoriEkle(Kategori k)
        {
            c.Kategoris.Add(k);
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult KategoriSil(int id)
        {
            var ktg = c.Kategoris.Find(id);
            c.Kategoris.Remove(ktg);
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult KategoriGetir(int id)
        {
            var kategori = c.Kategoris.Find(id);
            return View("KategoriGetir", kategori);
        }
        public ActionResult KategoriGuncelle(Kategori k)
        {
            var ktgr = c.Kategoris.Find(k.KategoriID);
            ktgr.KategoriAd = k.KategoriAd;
            c.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult KategoriEkleJson(Kategori kategori)
        {
            var hasCategory = c.Kategoris.Where(x => x.KategoriAd == kategori.KategoriAd).FirstOrDefault();
            if (hasCategory != null)
            {
                return Json(new ResultStatusUI()
                {
                    FeedBack = "Aynı kategori mevcut.",
                    Object = null,
                    Result = false,
                });
            }

            c.Kategoris.Add(kategori);
            c.SaveChanges();
            return Json(new ResultStatusUI()
            {
                FeedBack = "Kategori eklendi.",
                Object = kategori,
                Result = true,
            });
        }

        [HttpPost]
        public JsonResult KategoriGuncelleJson(Kategori kategori)
        {
            var name = c.Kategoris.Where(x => x.KategoriAd == kategori.KategoriAd).FirstOrDefault();
            if (name!=null)
            {
                if (name.KategoriID != kategori.KategoriID)
                {
                    return Json(new ResultStatusUI()
                    {
                        FeedBack = "Böyle bir kategori mevcut.",
                        Object=kategori,
                        Result=false,
                    });
                }
            }
            var kt = c.Kategoris.Find(kategori.KategoriID);
            kt.KategoriAd = kategori.KategoriAd;
            c.SaveChanges();
            return Json(new ResultStatusUI()
            {
                FeedBack = "Kategori güncellendi.",
                Object = kategori,
                Result = true,
            });
        }


        [HttpPost]
        public JsonResult KategoriSilJson(int id)
        {
            var UrunVarMi = c.Uruns.Where(x=>x.Kategoriid==id).ToList().Count();
            //var ÜrünVarMı = kategori.Uruns.Count();
            if (UrunVarMi != 0)
            {
                return Json(new ResultStatusUI()
                {
                    FeedBack = "Bu kategoriye ait ürünler mevcuttur.Silme işlemi yapılamaz.",
                    Object = "",
                    Result = false
                });
            }
            var kt = c.Kategoris.Find(id);
            c.Kategoris.Remove(kt);
            c.SaveChanges();
            return Json(new ResultStatusUI()
            {
                FeedBack = "Kategori silindi.",
                Object = id,
                Result = true,
            });
        }
    }
}