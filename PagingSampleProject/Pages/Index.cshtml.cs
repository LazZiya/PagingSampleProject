using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PagingSampleProject.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
            //this will act as the main data source for our project
            Cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        }
        private CultureInfo[] Cultures { get; set; }

        //page number variable
        [BindProperty(SupportsGet = true)]
        public int P { get; set; } = 1;

        //page size variable
        [BindProperty(SupportsGet = true)]
        public int S { get; set; } = 10;

        //total number of records
        public int TotalRecords { get; set; } = 0;

        //variable for text search
        [BindProperty(SupportsGet = true)]
        public string Q { get; set; } = string.Empty;

        //variable for culture types search filter
        [BindProperty(SupportsGet = true)]
        public CultureTypes CT { get; set; } = CultureTypes.AllCultures;

        //object that contains only displayed fields
        public class CultureItem
        {
            public int LCID { get; set; }
            public string EnglishName { get; set; }
            public string NativeName { get; set; }
            public CultureTypes CultureTypes { get; set; }
        }

        //return list of CultureItem
        public IList<CultureItem> CulturesList { get; set; }

        public void OnGet()
        {
            var query = Cultures.Select(x => new CultureItem
            {
                LCID = x.LCID,
                EnglishName = x.EnglishName,
                NativeName = x.NativeName,
                CultureTypes = x.CultureTypes
            });

            //if the search text is not empty, then apply where clause
            if (!string.IsNullOrWhiteSpace(Q))
            {
                var _keyWords = Q.Split(new[] { ' ', ',', ':' }, StringSplitOptions.RemoveEmptyEntries).Distinct();

                //null check is not required in our case for this sample, 
                //added just for demonstration in case the search will be done in nullable database fields
                query = query.Where(x => _keyWords.Any(kw =>
                    (x.EnglishName != null && x.EnglishName.Contains(kw, StringComparison.OrdinalIgnoreCase)) ||
                    (x.NativeName != null && x.NativeName.Contains(kw, StringComparison.OrdinalIgnoreCase))));
            }

            //if search filter for culture type is specified, then add where clause
            if (CT != CultureTypes.AllCultures)
            {
                query = query.Where(x => x.CultureTypes.HasFlag(CT));
            }

            //count records that returns after the search
            TotalRecords = query.Count();

            CulturesList = query

                //make sure to order items before paging
                .OrderBy(x => x.EnglishName)

                //skip items before current page
                .Skip((P - 1) * S)

                //take only 10 (page size) items
                .Take(S)
                .ToList();
        }
    }
}
