using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseFunction.Interfaces;

namespace DatabaseFunction.Services
{
    public class SqlService<L, R> : ISqlService<L, R>
    {
        public int Aggregate(List<L> Source, Func<L, int> selector, string operation)
        {
            if (Source == null || !Source.Any()) return 0;

            int result = 0;
            switch (operation.ToLower())
            {
                case "sum":
                    foreach (var item in Source)
                    {
                        result += selector(item);
                    }
                    break;

                case "avg":
                    int sum = 0;
                    int count = 0;
                    foreach (var item in Source)
                    {
                        sum += selector(item);
                        count++;
                    }
                    result = count > 0 ? sum / count : 0;
                    break;

                case "max":
                    result = selector(Source[0]);
                    foreach (var item in Source)
                    {
                        var value = selector(item);
                        if (value > result)
                        {
                            result = value;
                        }
                    }
                    break;

                case "min":
                    result = selector(Source[0]);
                    foreach (var item in Source)
                    {
                        var value = selector(item);
                        if (value < result)
                        {
                            result = value;
                        }
                    }
                    break;
                case "count":
                    result = Source.Count;
                    break;

                default:
                    throw new ArgumentException("Unsupported aggregate operation");
            }

            return result;
        }

        public List<(L, R)> CrossJoin(List<L> left, List<R> right)
        {
            var result = new List<(L, R)>();

            foreach (var leftItem in left)
            {
                foreach (var rightItem in right)
                {
                    result.Add((leftItem, rightItem));
                }
            }

            return result;
        }

        public List<(L, R)> InnerJoin(List<L> left, List<R> right, Func<L, R, bool> keySelector)
        {
            var result = new List<(L, R)>();

            foreach (var leftItem in left)
            {
                foreach (var rightItem in right)
                {
                    if (keySelector(leftItem, rightItem))
                    {
                        result.Add((leftItem, rightItem));
                    }
                }
            }

            return result;
        }

        public List<(L, R)> LeftJoin(List<L> left, List<R> right, Func<L, R, bool> keySelector)
        {
            var result = new List<(L, R?)>();

            foreach (var leftItem in left)
            {
                bool hasMatch = false;
                foreach (var rightItem in right)
                {
                    if (keySelector(leftItem, rightItem))
                    {
                        result.Add((leftItem, rightItem));
                        hasMatch = true;
                    }
                }

                if (!hasMatch)
                {
                    result.Add((leftItem, default(R)));
                }
            }

            return result;
        }

        public List<L> Where(List<L> Source, Func<L, bool> predicate)
        {
            var result = new List<L>();

            foreach (var item in Source)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }
}
