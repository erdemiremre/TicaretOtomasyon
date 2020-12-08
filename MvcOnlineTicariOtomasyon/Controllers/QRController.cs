using MvcOnlineTicariOtomasyon.Models.Siniflar;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class QRController : Controller
    {
        // GET: QR
        Context c = new Context();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult QrCode(string kod)
        {
            using(MemoryStream ms=new MemoryStream())
            {
                QRCodeGenerator koduret = new QRCodeGenerator();
                QRCodeGenerator.QRCode karekod = koduret.CreateQrCode(kod, QRCodeGenerator.ECCLevel.Q);
                using(Bitmap resim = karekod.GetGraphic(10))
                {
                    resim.Save(ms, ImageFormat.Png);
                    var qrCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    return Json(qrCodeImage);
                }
            }
           
        }
    }
}