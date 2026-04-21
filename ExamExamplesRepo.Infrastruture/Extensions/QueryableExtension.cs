using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExamplesRepo.Infrastruture.Extensions
{
    public static class QueryableExtension
    {
        public static IQueryable<T> ToPagedList<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
