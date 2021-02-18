using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagingSampleProject.Data;

namespace PagingSampleProject.Pages
{
    public class AjaxPagingModel : PageModel
    {
        private readonly ApplicationDbContext Context;

        public AjaxPagingModel(ApplicationDbContext context)
        {
            Context = context;
        }

        // Current page number
        [BindProperty(SupportsGet = true)]
        public int P { get; set; } = 1;

        // Page size, items to be displayed
        [BindProperty(SupportsGet = true)]
        public int S { get; set; } = 10;

        // Total number of records
        public int TotalRecords { get; set; } = 0;

        // Text search
        [BindProperty(SupportsGet = true)]
        public string Q { get; set; } = string.Empty;

        // Culture types search filter
        [BindProperty(SupportsGet = true)]
        public CultureTypes CT { get; set; } = CultureTypes.AllCultures;

        // List of CultureItem to be returned
        public ICollection<CultureItem> CulturesList { get; set; }

        public void OnGet()
        {
            (TotalRecords, CulturesList) = GetData();
        }

        public IActionResult OnGetAjaxPaging()
        {

            (TotalRecords, CulturesList) = GetData();

            return new PartialViewResult()
            {
                ViewName = "ItemsPartial",
                ViewData = this.ViewData
            };
        }

        private (int totalRecods, ICollection<CultureItem> items) GetData()
        {
            var query = Context.Set<CultureItem>().Select(x => new CultureItem
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
            var t = query.Count();

            var i = query

                //make sure to order items before paging
                .OrderBy(x => x.EnglishName)

                //skip items before current page
                .Skip((P - 1) * S)

                //take only 10 (page size) items
                .Take(S)
                .ToList();

            return (t, i);
        }
    }
}
