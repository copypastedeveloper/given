using System;

namespace Given.Common
{
    public class CleanupHelper
    {
        readonly string _context;

        public CleanupHelper(string context)
        {
            _context = context;
        }

        public void WithCleanup(Action cleanupAction)
        {
            Context.CleanUps.Add(_context, cleanupAction);
        }
    }

    public class CleanupHelper<T>
    {
        readonly string _context;

        public CleanupHelper(string context)
        {
            _context = context;
        }

        public void WithCleanup(Action<T> cleanupAction)
        {
            Context.CleanUps.Add(_context, cleanupAction);
        }
    }

    public class CleanupHelper<T1, T2>
    {
        readonly string _context;

        public CleanupHelper(string context)
        {
            _context = context;
        }

        public void WithCleanup(Action<T1, T2> cleanupAction)
        {
            Context.CleanUps.Add(_context, cleanupAction);
        }
    }

    public class CleanupHelper<T1, T2, T3>
    {
        readonly string _context;

        public CleanupHelper(string context)
        {
            _context = context;
        }

        public void WithCleanup(Action<T1, T2, T3> cleanupAction)
        {
            Context.CleanUps.Add(_context, cleanupAction);
        }
    }

    public class CleanupHelper<T1, T2, T3, T4>
    {
        readonly string _context;

        public CleanupHelper(string context)
        {
            _context = context;
        }

        public void WithCleanup(Action<T1, T2, T3, T4> cleanupAction)
        {
            Context.CleanUps.Add(_context, cleanupAction);
        }
    }

    public class CleanupHelper<T1, T2, T3, T4, T5>
    {
        readonly string _context;

        public CleanupHelper(string context)
        {
            _context = context;
        }

        public void WithCleanup(Action<T1, T2, T3, T4, T5> cleanupAction)
        {
            Context.CleanUps.Add(_context, cleanupAction);
        }
    }
}