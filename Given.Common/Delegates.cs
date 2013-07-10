using System;

namespace Given.Common
{
    public delegate void before();
    public delegate void after();
    public delegate void given();
    public delegate TReturn context<out TReturn>();
    public delegate Tuple<TReturn, TReturn2> context<TReturn, TReturn2>();
    public delegate Tuple<TReturn, TReturn2, TReturn3> context<TReturn, TReturn2, TReturn3>();
    public delegate Tuple<TReturn, TReturn2, TReturn3, TReturn4> context<TReturn, TReturn2, TReturn3, TReturn4>();
    public delegate Tuple<TReturn, TReturn2, TReturn3, TReturn4, TReturn5> context<TReturn, TReturn2, TReturn3, TReturn4, TReturn5>();

    public delegate void MethodThatThrows();

    public delegate void when();
}