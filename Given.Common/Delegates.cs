using System;

namespace Given.Common
{
    public delegate void given();
    public delegate TReturn given<out TReturn>();
    public delegate Tuple<TReturn, TReturn2> given<TReturn, TReturn2>();
    public delegate Tuple<TReturn, TReturn2, TReturn3> given<TReturn, TReturn2, TReturn3>();
    public delegate Tuple<TReturn, TReturn2, TReturn3, TReturn4> given<TReturn, TReturn2, TReturn3, TReturn4>();
    public delegate Tuple<TReturn, TReturn2, TReturn3, TReturn4, TReturn5> given<TReturn, TReturn2, TReturn3, TReturn4, TReturn5>();

    public delegate void MethodThatThrows();

    public delegate void when();
}