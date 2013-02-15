using System;
using System.Collections.Generic;

namespace Given.Common
{
    public class TestResult
    {
        readonly IEnumerable<string> _givens;
        readonly IEnumerable<string> _whens;
        readonly IEnumerable<StatedThen> _thens;
        readonly Type _type;

        public IEnumerable<string> Givens
        {
            get { return _givens; }
        }

        public IEnumerable<string> Whens
        {
            get { return _whens; }
        }

        public IEnumerable<StatedThen> Thens
        {
            get { return _thens; }
        }

        public Type Type
        {
            get { return _type; }
        }

        public TestResult(IEnumerable<string> givens, IEnumerable<string> whens, IEnumerable<StatedThen> thens, Type specType)
        {
            _givens = givens;
            _whens = whens;
            _thens = thens;
            _type = specType;
        }
    }
}