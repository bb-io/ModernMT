using Blackbird.Applications.Sdk.Common;

namespace Apps.ModernMT;

public class ModernMtApplication : IApplication
{
    public string Name
    {
        get => "Modern MT";
        set { }
    }

    public T GetInstance<T>()
    {
        throw new NotImplementedException();
    }
}