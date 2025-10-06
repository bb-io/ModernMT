using ModernMT;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.ModernMT.Utils;

public static class ErrorHandler
{
    public static T ExecuteWithErrorHandling<T>(Func<T> action)
    {
        try
        {
            return action();
        }
        catch (ModernMTException ex)
        {
            throw new PluginApplicationException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new PluginApplicationException(ex.Message);
        }
    }
}
