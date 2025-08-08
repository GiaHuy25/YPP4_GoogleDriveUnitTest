using DatabaseFunction.Interfaces;

namespace DatabaseFunction.Services
{
    public class SqlService<L, R> : ISqlService<L, R>
    {
        public int Aggregate(IEnumerable<L> Source, Func<L, int> selector, string operation)
        {
            if (Source == null) return 0;

            using (var enumerator = Source.GetEnumerator())
            {
                if (!enumerator.MoveNext()) return 0;

                int result = 0;
                switch (operation.ToLower())
                {
                    case "sum":
                        result = selector(enumerator.Current);
                        while (enumerator.MoveNext())
                        {
                            result += selector(enumerator.Current);
                        }
                        break;

                    case "avg":
                        int sum = selector(enumerator.Current);
                        int count = 1;
                        while (enumerator.MoveNext())
                        {
                            sum += selector(enumerator.Current);
                            count++;
                        }
                        result = count > 0 ? sum / count : 0;
                        break;

                    case "max":
                        result = selector(enumerator.Current);
                        while (enumerator.MoveNext())
                        {
                            var value = selector(enumerator.Current);
                            if (value > result)
                            {
                                result = value;
                            }
                        }
                        break;

                    case "min":
                        result = selector(enumerator.Current);
                        while (enumerator.MoveNext())
                        {
                            var value = selector(enumerator.Current);
                            if (value < result)
                            {
                                result = value;
                            }
                        }
                        break;
                    case "count":
                        result = 1;
                        while (enumerator.MoveNext())
                        {
                            result++;
                        }
                        break;

                    default:
                        throw new ArgumentException("Unsupported aggregate operation");
                }

                return result;
            }
        }

        public List<(L, R)> CrossJoin(IEnumerable<L> left, IEnumerable<R> right)
        {
            var result = new List<(L, R)>();
            var leftEnumerator = left.GetEnumerator();
            var rightEnumerator = right.GetEnumerator();

            // Materialize left into a list for nested iteration
            var leftList = new List<L>();
            while (leftEnumerator.MoveNext())
            {
                leftList.Add(leftEnumerator.Current);
            }

            // Materialize right into a list for nested iteration
            var rightList = new List<R>();
            if (rightEnumerator.MoveNext())
            {
                rightList.Add(rightEnumerator.Current);
                while (rightEnumerator.MoveNext())
                {
                    rightList.Add(rightEnumerator.Current);
                }
            }

            foreach (var leftItem in leftList)
            {
                foreach (var rightItem in rightList)
                {
                    result.Add((leftItem, rightItem));
                }
            }

            return result;
        }

        public List<(L, R)> InnerJoin(IEnumerable<L> left, IEnumerable<R> right, Func<L, R, bool> keySelector)
        {
            var result = new List<(L, R)>();
            var leftEnumerator = left.GetEnumerator();
            var rightEnumerator = right.GetEnumerator();

            // Materialize left into a list for nested iteration
            var leftList = new List<L>();
            while (leftEnumerator.MoveNext())
            {
                leftList.Add(leftEnumerator.Current);
            }

            // Materialize right into a list for nested iteration
            var rightList = new List<R>();
            while (rightEnumerator.MoveNext())
            {
                rightList.Add(rightEnumerator.Current);
            }

            foreach (var leftItem in leftList)
            {
                foreach (var rightItem in rightList)
                {
                    if (keySelector(leftItem, rightItem))
                    {
                        result.Add((leftItem, rightItem));
                    }
                }
            }

            return result;
        }

        public List<(L, R)> LeftJoin(IEnumerable<L> left, IEnumerable<R> right, Func<L, R, bool> keySelector)
        {
            var result = new List<(L, R?)>();
            var leftEnumerator = left.GetEnumerator();
            var rightEnumerator = right.GetEnumerator();
            var leftList = new List<L>();
            while (leftEnumerator.MoveNext())
            {
                leftList.Add(leftEnumerator.Current);
            }
            var rightList = new List<R>();
            while (rightEnumerator.MoveNext())
            {
                rightList.Add(rightEnumerator.Current);
            }

            foreach (var leftItem in leftList)
            {
                bool hasMatch = false;
                foreach (var rightItem in rightList)
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

        public List<L> Where(IEnumerable<L> Source, Func<L, bool> predicate)
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
        public Dictionary<TKey, List<T>> GroupBy<T, TKey>(IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            var result = new Dictionary<TKey, List<T>>();

            foreach (var item in source)
            {
                var key = keySelector(item);
                if (!result.ContainsKey(key))
                {
                    result[key] = new List<T>();
                }
                result[key].Add(item);
            }

            return result;
        }

        public List<T> OrderBy<T, TKey>(IEnumerable<T> source, Func<T, TKey> keySelector, bool descending = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            var result = source.ToList();
            result.Sort((a, b) =>
            {
                var keyA = keySelector(a);
                var keyB = keySelector(b);

                if (descending)
                {
                    return Comparer<TKey>.Default.Compare(keyB, keyA);
                }
                return Comparer<TKey>.Default.Compare(keyA, keyB);
            });

            return result;
        }
    }
}
