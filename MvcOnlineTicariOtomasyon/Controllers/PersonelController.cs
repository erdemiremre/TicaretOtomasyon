using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasyon.Models.Helper;
using MvcOnlineTicariOtomasyon.Models.Siniflar;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class PersonelController : Controller
    {
        // GET: Personel
        Context c = new Context();
        public ActionResult Index(string p)
        {
            var personeller = from x in c.Personels select x;
            if (!string.IsNullOrEmpty(p))
            {
                personeller = personeller.Where(y => y.PersonelAd.Contains(p));
            }
            return View(personeller.ToList());
            //var degerler = c.Personels.ToList();
            //return View(degerler);
        }
        [HttpGet]
        public ActionResult PersonelEkle()
        {
            List<SelectListItem> deger1 = (from x in c.Departmans.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.DepartmanAd,
                                               Value = x.Departmanid.ToString()
                                           }).ToList();
            ViewBag.dgr1 = deger1;
            return View();
        }
        [HttpPost]
        public ActionResult PersonelEkle(Personel p)
        {
            if (Request.Files.Count > 0)
            {
                string dosyaadi = Path.GetFileName(Request.Files[0].FileName);
                string uzanti = Path.GetExtension(Request.Files[0].FileName);
                string yol = "~/Image/" + dosyaadi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(yol));
                p.PersonelGorsel = "/Image/" + dosyaadi + uzanti;
            }
            c.Personels.Add(p);
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult PersonelGetir(int id)
        {
            List<SelectListItem> deger2 = (from x in c.Departmans.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.DepartmanAd,
                                               Value = x.Departmanid.ToString()
                                           }).ToList();
            ViewBag.dgr2 = deger2;
            var prs = c.Personels.Find(id);
            return View("PersonelGetir", prs);
        }
        public ActionResult PersonelGuncelle(Personel p)
        {
            if (Request.Files.Count > 0)
            {
                string dosyaadi = Path.GetFileName(Request.Files[0].FileName);
                string uzanti = Path.GetExtension(Request.Files[0].FileName);
                string yol = "~/Image/" + dosyaadi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(yol));
                p.PersonelGorsel = "/Image/" + dosyaadi + uzanti;
            }
            var prsn = c.Personels.Find(p.Personelid);
            prsn.PersonelAd = p.PersonelAd;
            prsn.PersonelSoyad = p.PersonelSoyad;
            prsn.PersonelGorsel = p.PersonelGorsel;
            prsn.Departmanid = p.Departmanid;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult PersonelListe()
        {
            var sorgu = c.Personels.ToList();
            return View(sorgu);
        }

        [HttpPost]
        public JsonResult PersonelEkleJson()
        {
            List<SelectListItem> deger1 = (from x in c.Departmans.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.DepartmanAd,
                                               Value = x.Departmanid.ToString()
                                           }).ToList();
            return Json(deger1);


        }
        [HttpPost]
        public JsonResult PersonelEklemeYapJson(Personel person)
        {
            var personelVarMi = c.Personels.Where(x => x.PersonelAd == person.PersonelAd && x.PersonelSoyad == person.PersonelSoyad).FirstOrDefault();
            

            if (personelVarMi != null)
            {
                return Json(new ResultStatusUI()
                {
                    FeedBack = "Bu personelin kayıtı mevcut.",
                    Object = null,
                    Result = false,
                });
            }
            if (Request.Files.Count > 0)
            {
                string dosyaadi = Path.GetFileName(Request.Files[0].FileName);
                string uzanti = Path.GetExtension(Request.Files[0].FileName);
                string yol = "~/Image/" + dosyaadi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(yol));
                person.PersonelGorsel = "/Image/" + dosyaadi + uzanti;
            }
            c.Personels.Add(person);
            c.SaveChanges();

            var personelObjesi = new Models.DTO.PersonelDTO()
            {
                DepartmanAdi = c.Departmans.Where(x => x.Departmanid == person.Departmanid).Select(x => x.DepartmanAd).FirstOrDefault(),
                Departmanid = person.Departmanid,
                PersonelAd = person.PersonelAd,
                PersonelGorsel = person.PersonelGorsel,
                Personelid = person.Personelid,
                PersonelSoyad = person.PersonelSoyad,
            };
            return Json(new ResultStatusUI()
            {
                FeedBack = "Yeni Personel Eklendi",
                Object = personelObjesi,
                Result = true,
            });
        }
    }
}