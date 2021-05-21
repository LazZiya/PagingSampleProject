using System.Collections.Generic;

namespace PagingSampleProjectMVC.Data
{
    public class CulturesPagingModel : PagingModel
    {
        public IList<CultureItem> CulturesList { get; set; }
    }
}
