using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PagingSampleProjectMVC.Data
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using(var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Cultures.Any())
                {
                    return;
                }

                context.Set<CultureItem>().AddRange(Cultures);

                context.SaveChanges();
            }
        }

        private static IEnumerable<CultureItem> Cultures => 
            CultureInfo.GetCultures(CultureTypes.AllCultures)
            .Select(x => new CultureItem 
            {
                LCID = x.LCID,
                Name = x.Name,
                EnglishName = x.EnglishName,
                NativeName = x.NativeName,
                CultureTypes = x.CultureTypes
            })
            .AsEnumerable();
    }
}
