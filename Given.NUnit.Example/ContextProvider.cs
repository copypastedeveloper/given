using Given.Common;
using Given.Example;

public class ContextProvider : IContextProvider
{
    public void SetupContext()
    {
        Context.Register("a car factory", () => new CarFactory());
    }
}