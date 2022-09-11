# FoldExperiment
Fold (aka reduce, accumulate, aggregate, compress, or inject) higher-order function experiments.

**Language: Haskell / C#**

**Start: 2015**

## Why
Working in Haskell helped me better understand the concepts of functional programming. My understanding of [LINQ](https://en.wikipedia.org/wiki/Language_Integrated_Query) has improved dramatically thanks to Haskell. In this project, I re-implemented (in several ways) the [Fold / Aggregate](https://en.wikipedia.org/wiki/Fold_(higher-order_function)) function in C#. I also re-implemented Map, Distinct and other functions in Haskell using the Fold function.

## Example
Last C# Fold implementation:

```csharp
static T1 Fold<T1, T2>(Func<T2, T1, T1> f, T1 x, IEnumerable<T2> xs)
{
  return Fold0(f, x, xs, 0, xs.Count());
}

static T1 Fold0<T1, T2>(Func<T2, T1, T1> f, T1 x, IEnumerable<T2> xs, int index, int xsCount)
{
  if (xsCount == 0) return x;
  T1 res = f(xs.ElementAt(index), Fold0(f, x, xs, index + 1, xsCount - 1));
  return res;
}
```

Map with Fold:

```haskell
f_map f xs = foldr g [] xs
  where g x v = (f x):v
```