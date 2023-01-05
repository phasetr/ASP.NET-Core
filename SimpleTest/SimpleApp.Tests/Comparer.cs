using System;
using System.Collections.Generic;

namespace SimpleApp.Tests;

public class Comparer
{
    public static Comparer<T?> Get<T>(Func<T?, T?, bool> func)
    {
        return new Comparer<T?>(func);
    }
}

public class Comparer<T> : Comparer, IEqualityComparer<T>
{
    private readonly Func<T?, T?, bool> _comparisonFunction;

    public Comparer(Func<T?, T?, bool> func)
    {
        _comparisonFunction = func;
    }

    public bool Equals(T? x, T? y)
    {
        return _comparisonFunction(x, y);
    }

    public int GetHashCode(T obj)
    {
        return obj?.GetHashCode() ?? 0;
    }
}