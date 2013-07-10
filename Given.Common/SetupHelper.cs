namespace Given.Common
{
    public class SetupHelper
    {
        readonly string _context;

        public SetupHelper(string context)
        {
            _context = context;
        }

        public CleanupHelper As(given given)
        {
            Context.Contexts.Add(_context, given);
            return new CleanupHelper(_context);
        }

        public CleanupHelper<T> As<T>(context<T> given)
        {
            Context.Contexts.Add(_context, given);
            return new CleanupHelper<T>(_context);
        }

        public CleanupHelper<T1, T2> As<T1, T2>(context<T1, T2> given)
        {
            Context.Contexts.Add(_context, given);
            return new CleanupHelper<T1, T2>(_context);
        }

        public CleanupHelper<T1, T2, T3> As<T1, T2, T3>(context<T1, T2, T3> given)
        {
            Context.Contexts.Add(_context, given);
            return new CleanupHelper<T1, T2, T3>(_context);
        }

        public CleanupHelper<T1, T2, T3, T4> As<T1, T2, T3, T4>(context<T1, T2, T3, T4> given)
        {
            Context.Contexts.Add(_context, given);
            return new CleanupHelper<T1, T2, T3, T4>(_context);
        }

        public CleanupHelper<T1, T2, T3, T4, T5> As<T1, T2, T3, T4, T5>(context<T1, T2, T3, T4, T5> given)
        {
            Context.Contexts.Add(_context, given);
            return new CleanupHelper<T1, T2, T3, T4, T5>(_context);
        }
    }
}