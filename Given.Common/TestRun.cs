using System;
using System.Collections.Generic;
using System.Linq;

namespace Given.Common
{
    internal class TestRun
    {
        readonly List<Story> _stories;
        
        public TestRun()
        {
            _stories = new List<Story>();
        }

        public void AddTest(TestStateManager testStateManager, Type type)
        {
            var story = new Story((StoryAttribute)type.GetCustomAttributes(typeof(StoryAttribute), true).FirstOrDefault() ?? new StoryAttribute());
            
            if (!_stories.Any(x => x.Equals(story)))
                _stories.Add(story);

            story = _stories.First(x => x.Equals(story));
            story.AddTestStateManager(testStateManager);
        }

        public List<Story> GetStories()
        {
            return _stories;
        }
    }
}