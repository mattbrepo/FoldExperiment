using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoldTest
{
	class Program
	{
		static void Main(string[] args)
		{
			List<int> nums = Enumerable.Range(5, 1000).ToList();

			//--------------------------------------------------------------- fold
			DateTime dt1 = DateTime.Now;
			int test1 = Fold((a, b) => { return a + b; }, 0, nums);
			DateTime dt2 = DateTime.Now;
			int test1b = nums.Aggregate((a, b) => { return a + b; });
			DateTime dt3 = DateTime.Now;

			double diff1 = (dt2 - dt1).TotalMilliseconds;
			double diff2 = (dt3 - dt2).TotalMilliseconds;
			bool test1Ok = test1 == test1b;

			//--------------------------------------------------------------- map
			//haskell mymap:
			//mymap f xs = foldr g [] xs
			//		where g x v = (f x):v
			dt1 = DateTime.Now;
			List<int> test2 = MyMap(v => { return v + 1; }, nums).ToList();
			dt2 = DateTime.Now;
			List<int> test2b = nums.Select(v => v + 1).ToList();
			dt3 = DateTime.Now;

			diff1 = (dt2 - dt1).TotalMilliseconds;
			diff2 = (dt3 - dt2).TotalMilliseconds;
			bool test2Ok = test2.SequenceEqual(test2b);

			//--------------------------------------------------------------- ...scramble...
			List<int> test3 = MyScramble(nums);
		}

		//==================================================================================================== Fold
		//------------------------------------------------------------------------ primo tentativo
		//static T Fold<T>(Func<T,T,T> f, T x, IEnumerable<T> xs)
		//{
		//    T res = x;
		//    foreach (var v in xs) res = f(res, v);
		//    return res;
		//}

		//------------------------------------------------------------------------ secondo tentativo
		//static T Fold<T>(Func<T,T,T> f, T x, IEnumerable<T> xs)
		//{
		//    if (!xs.Any()) return x;
		//    T res = f(xs.First(), Fold(f, x, xs.Skip(1)));
		//    return res;
		//}

		//------------------------------------------------------------------------ terzo tentativo
		//static T1 Fold<T1, T2>(Func<T2, T1, T1> f, T1 x, IEnumerable<T2> xs)
		//{
		//    if (!xs.Any()) return x;
		//    T1 res = f(xs.First(), Fold(f, x, xs.Skip(1)));
		//    return res;
		//}

		//------------------------------------------------------------------------ quarto tentativo (più performante)
		static T1 Fold<T1, T2>(Func<T2, T1, T1> f, T1 x, IEnumerable<T2> xs)
		{
			return Fold0(f, x, xs, 0, xs.Count());
		}

		static T1 Fold0<T1, T2>(Func<T2, T1, T1> f, T1 x, IEnumerable<T2> xs, int index, int xsCount)
		{
			if (xsCount == 0) return x;
			T1 res = f(xs.ElementAt(index), Fold0(f, x, xs, index + 1, xsCount-1));
			return res;
		}

		//static T1 FoldL0<T1, T2>(Func<T1, T2, T1> f, T1 x, IEnumerable<T2> xs, int index, int xsCount)
		//{
		//    // foldl f z []     = z
		//    // foldl f z (x:xs) = foldl f (f z x) xs
		//    if (xsCount == 0) return x;
		//    T1 res = FoldL0(f, f(x, xs.ElementAt(index)), xs, index + 1, xsCount - 1);
		//    return res;
		//}

		//==================================================================================================== Map
		//------------------------------------------------------------------------ primo tentativo
		//static List<T> MyMap<T>(Func<T, T> f, List<T> xs)
		//{
		//    Func<T, List<T>, List<T>> g = (x, vs) =>
		//    {
		//        vs.Insert(0, f(x));
		//        return vs;
		//    };

		//    List<T> res = Fold(g, new List<T>(xs.Count), xs);

		//    //Func<T,List<T>,List<T>> g = (x, vs) => { 
		//    //    vs.Add(f(x));
		//    //    return vs;
		//    //};

		//    //List<T> res = FoldL0(g, new List<T>(), xs, 0, xs.Count);

		//    return res;
		//}

		//------------------------------------------------------------------------ secondo tentativo
		static IEnumerable<T> MyMap<T>(Func<T, T> f, IEnumerable<T> xs)
		{
			Func<T, IEnumerable<T>, IEnumerable<T>> g = (x, vs) =>
			{
				T[] temp = new T[] { f(x) };
				return temp.Concat(vs);
			};

			IEnumerable<T> res = Fold(g, Enumerable.Empty<T>(), xs);
			return res;
		}

		//==================================================================================================== ...Scramble...
		static List<int> MyScramble(List<int> xs)
		{
			Random r = new Random();
			Func<int, List<int>, List<int>> g = (x, vs) =>
			{
				int indexOld = vs.FindIndex(item => item == x);
				int indexNew = r.Next(0, vs.Count - 1);
				if (indexOld != indexNew)
				{
					int old = vs[indexOld];
					vs[indexOld] = vs[indexNew];
					vs[indexNew] = old;
				}
				return vs;
			};

			List<int> res = Fold(g, new List<int>(xs), xs);
			return res;
		}
	}
}
