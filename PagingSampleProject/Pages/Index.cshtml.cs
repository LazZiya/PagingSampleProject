﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PagingSampleProject.Data;

namespace PagingSampleProject.Pages
{
    public class IndexModel : PageModel
    {
        private ApplicationDbContext Context;

        public IndexModel(ApplicationDbContext context)
        {
            Context = context;
        }

        // Current page number
        [BindProperty(SupportsGet = true)]
        public int P { get; set; } = 1;

        // Page size, number of records to be displayed
        [BindProperty(SupportsGet = true)]
        public int S { get; set; } = 10;

        // Total number of records
        public int TotalRecords { get; set; } = 0;

        // Search text
        [BindProperty(SupportsGet = true)]
        public string Q { get; set; } = string.Empty;

        // Variable for culture types search filter
        [BindProperty(SupportsGet = true)]
        public CultureTypes CT { get; set; } = CultureTypes.AllCultures;

        // The list of CultureItem to be displayed
        public IList<CultureItem> CulturesList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var searchExp = new List<Expression<Func<CultureItem, bool>>>() { };

            // If the search text is not empty, then apply where clause
            if (!string.IsNullOrWhiteSpace(Q))
            {
                searchExp.Add(x => x.EnglishName.Contains(Q) || x.NativeName.Contains(Q) || x.Name.Contains(Q));
            }

            //if search filter for culture type is specified, then add where clause
            if (CT != CultureTypes.AllCultures)
            {
                searchExp.Add(x => x.CultureTypes.HasFlag(CT));
            }

            //count records that returns after the search
            TotalRecords = await Context.Set<CultureItem>()
                                        .AsNoTracking()
                                        .WhereList(searchExp)
                                        .CountAsync();

            CulturesList = await Context.Set<CultureItem>()
                                        .AsNoTracking()
                                        .WhereList(searchExp)
                                        .OrderBy(x => x.EnglishName)
                                        .Skip((P - 1) * S)
                                        .Take(S)
                                        .ToListAsync();
            return Page();
        }
    }
}
