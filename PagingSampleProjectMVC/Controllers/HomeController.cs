using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PagingSampleProjectMVC.Data;
using PagingSampleProjectMVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PagingSampleProjectMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(
            [FromQuery] int p = 1, 
            [FromQuery] int s = 10, 
            [FromQuery] string q = "", 
            [FromQuery] CultureTypes ct = CultureTypes.AllCultures)
        {
            var searchExp = new List<Expression<Func<CultureItem, bool>>>() { };

            // If the search text is not empty, then apply where clause
            if (!string.IsNullOrWhiteSpace(q))
            {
                searchExp.Add(x => x.EnglishName.Contains(q) || x.NativeName.Contains(q) || x.Name.Contains(q));
            }

            //if search filter for culture type is specified, then add where clause
            if (ct != CultureTypes.AllCultures)
            {
                searchExp.Add(x => x.CultureTypes.HasFlag(ct));
            }

            CulturesPagingModel model = new()
            {
                P = p,
                S = s
            };

            //count records that returns after the search
            model.TotalRecords = await _context.Set<CultureItem>()
                                               .AsNoTracking()
                                               .WhereList(searchExp)
                                               .CountAsync();

            model.CulturesList = await _context.Set<CultureItem>()
                                               .AsNoTracking()
                                               .WhereList(searchExp)
                                               .OrderBy(x => x.EnglishName)
                                               .Skip((p - 1) * s)
                                               .Take(s)
                                               .ToListAsync();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
