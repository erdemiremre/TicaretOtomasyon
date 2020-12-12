using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcOnlineTicariOtomasyon.Models.DTO
{
    public class PersonelDTO
    {
        public int Personelid { get; set; }

        public string PersonelAd { get; set; }

        public string PersonelSoyad { get; set; }

        public string PersonelGorsel { get; set; }


        public int Departmanid { get; set; }
        public string DepartmanAdi { get; set; }
    }
}