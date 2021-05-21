using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PagingSampleProjectMVC.Data
{
    //object that contains only displayed fields
    public class CultureItem
    {
        [Key]
        public int ID { get; set; }
        public int LCID { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public string NativeName { get; set; }
        public CultureTypes CultureTypes { get; set; }
    }
}
