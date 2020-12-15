using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MvcOnlineTicariOtomasyon.Models.Enums
{
    public enum EnumUrunler
    {
        [Description("Stok Durumu Mevcut")]
        Stokta = 1,
        [Description("Stokta Yok")]
        StoktaDegil = 2,
        [Description("Satışa Kapalı")]
        SatisaKapali = 3,
    }
}