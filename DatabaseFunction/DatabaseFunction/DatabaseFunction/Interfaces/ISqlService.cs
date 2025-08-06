using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFunction.Interfaces
{
    internal interface ISqlService<L,R>
    {
        List<(L, R)> CrossJoin(List<L> left, List<R> right);
        List<(L, R)> InnerJoin(List<L> left, List<R> right, Func<L, R, bool> keySelector);
        List<(L, R)> LeftJoin(List<L> left, List<R> right, Func<L, R, bool> keySelector);
        List<L> Where(List<L> Source, Func<L, bool> predicate);
        int Aggregate(List<L> Source, Func<L, int> selector, string operation);
    }
}
