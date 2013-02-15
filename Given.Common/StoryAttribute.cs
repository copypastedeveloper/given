using System;

namespace Given.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class StoryAttribute : Attribute
    {
        string _asA;
        string _want;
        string _soThat;

        public string AsA
        {
            get { return "As a " + _asA; }
            set { _asA = value; }
        }

        public string IWant
        {
            get { return "I want " + _want; }
            set { _want = value; }
        }

        public string SoThat
        {
            get { return "so that " + _soThat; }
            set { _soThat = value; }
        }
    }
}