using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcOnlineTicariOtomasyon.Models.DTO
{
    public class GrafikRaporModel
    {
        public GrafikRaporModel()
        {
            DepartmanDTOs = new List<DepartmanDTO>();
            UrunDTOs = new List<UrunDto>();
        }
        public List<DepartmanDTO> DepartmanDTOs { get; set; }
        public List<UrunDto> UrunDTOs  { get; set; }
    }
}