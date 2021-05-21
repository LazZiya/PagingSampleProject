using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PagingSampleProjectMVC
{
    public static class GenericWhereList
    {
        public static IQueryable<T> WhereList<T>(this IQueryable<T> source, List<Expression<Func<T, bool>>> predications)
            where T : class
        {
            if (predications != null)
            {
                foreach (var p in predications)
                {
                    source = source.Where(p);
                }
            }

            return source;
        }
    }
}
