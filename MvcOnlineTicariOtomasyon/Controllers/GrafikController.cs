using MvcOnlineTicariOtomasyon.Models.DTO;
using MvcOnlineTicariOtomasyon.Models.Helper;
using MvcOnlineTicariOtomasyon.Models.Siniflar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class GrafikController : Controller
    {
        // GET: Grafik
        Context c = new Context();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2()
        {
            var grafikciz = new Chart(600, 600);
            grafikciz.AddTitle("Kategori-Ürün Stok Sayısı").AddLegend("Stok").AddSeries
                ("Degerler", xValue: new[] { "Mobilya", "Ofis Eşyaları", "Bilgisayar" }, yValues: new[] {85,
                66,98}).Write();
            return File(grafikciz.ToWebImage().GetBytes(), "image/jpeg");
        }
        public ActionResult Index3()
        {
            ArrayList xvalue = new ArrayList();
            ArrayList yvalue = new ArrayList();
            var sonuclar = c.Uruns.ToList();
            sonuclar.ToList().ForEach(x => xvalue.Add(x.UrunAd));
            sonuclar.ToList().ForEach(y => yvalue.Add(y.Stok));
            var grafik = new Chart(width: 500, height: 500).AddTitle("Stoklar")
                .AddSeries(chartType: "Pie", name: "Stok", xValue: xvalue, yValues: yvalue);
            return File(grafik.ToWebImage().GetBytes(), "image/jpeg");
        }

        public ActionResult Index4()
        {
            return View();
        }
        public ActionResult VisualizeUrunResult()
        {
            return Json(UrunListesi(), JsonRequestBehavior.AllowGet);
        }
        public List<Sinif1> UrunListesi()
        {
            List<Sinif1> snf = new List<Sinif1>();
            snf.Add(new Sinif1()
            {
                urunad = "Bilgisayar",
                stok = 120
            });
            snf.Add(new Sinif1()
            {
                urunad = "Beyaz Eşya",
                stok = 150
            });
            snf.Add(new Sinif1()
            {
                urunad = "Mobilya",
                stok = 70
            });
            snf.Add(new Sinif1()
            {
                urunad = "Telefon",
                stok = 50
            });
            return snf;
        }
        public ActionResult Index5()
        {
            return View();
        }
        public ActionResult VisualizeUrunResult2()
        {
            return Json(UrunListesi2(), JsonRequestBehavior.AllowGet);
        }
        public List<Sinif2> UrunListesi2()
        {
            List<Sinif2> snf = new List<Sinif2>();
            using (var c = new Context())
            {
                snf = c.Uruns.Select(x => new Sinif2
                {
                    urn = x.UrunAd,
                    stk = x.Stok


                }).ToList();
            }
            return snf;
        }
        public ActionResult Index6()
        {
            return View();
        }
        public ActionResult Index7()
        {
            return View();
        }
        public JsonResult VisualizeUrunResult3()
        {
            return Json(PersonelDptListe(), JsonRequestBehavior.AllowGet);
        }
        public List<DepartmanDTO> PersonelDptListe()
        {
            List<DepartmanDTO> snf = new List<DepartmanDTO>();
            using (var c = new Context())
            {
                snf = c.Uruns.Select(x => new DepartmanDTO
                {
                    Marka=x.Marka,
                    Stk=x.Stok
                }).OrderByDescending(x=>x.Stk).ToList();
            }
            return snf;
        }

        public ActionResult StokRaporu()
        {
            return View();
        }

        public JsonResult StokRaporDataGetir()
        {
            List<DepartmanDTO> snf = new List<DepartmanDTO>();
            using (var c = new Context())
            {
                snf = c.Uruns.Select(x => new DepartmanDTO
                {
                    Marka = x.Marka,
                    Stk = x.Stok
                }).OrderByDescending(x => x.Stk).ToList();
            }
            return Json(new ResultStatusUI()
            {
                Object = snf
            });
        }
        public ActionResult UrunRaporuHighCart()
        {
            return View();
        }
        public JsonResult UrunRaporuDataGetir()
        {
            var grafikRaporModel = new GrafikRaporModel();
            using (var c=new Context())
            {
                var urun = c.Uruns.Select(x => new UrunDto
                {
                    UrunIsmi = x.UrunAd,
                    Stok = x.Stok
                }).OrderByDescending(x => x.Stok).ToList();
                grafikRaporModel.UrunDTOs.AddRange(urun);

                var departmanlar = c.Departmans.ToList();
                var personeller = c.Personels.ToList();
                var listDepartman = new List<DepartmanDTO>();

                foreach (var item in departmanlar)
                {
                    if (item.Personels != null)
                    {
                        if (item.Personels.Count() != 0)
                        {
                            var departman = new DepartmanDTO();
                            departman.Marka = item.DepartmanAd;
                            departman.Stk = (short)item.Personels.Count();
                            listDepartman.Add(departman);
                        }
                    }
                }
                grafikRaporModel.DepartmanDTOs.AddRange(listDepartman);
            }
            return Json(new ResultStatusUI()
            {
                Object = grafikRaporModel
            });
        }
    }
}