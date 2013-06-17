using System;
using System.Collections.Generic;
using System.Linq;

namespace Given.Common
{
    public class ContextDictionary : Dictionary<string, Delegate>
    {
        new public Delegate this[string index]
        {
            get
            {
                if (ContainsKey(index) || Keys.Count == 0) return base[index];
                
                var wordMatching = new Levenshtein();
                var keys = Keys.Select(key => new {Key = key, Similarity = wordMatching.Distance(key, index)}).ToList();
                var bestMatch = keys.OrderBy(x => x.Similarity).First();

                //return base[bestMatch.Key];

                throw new KeyNotFoundException(string.Format("Context item '{0}' was not found.  Did you mean '{1}'?", index, bestMatch.Key));
            }
        }
    }
}