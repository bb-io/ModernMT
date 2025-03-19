using Blackbird.Applications.Sdk.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ModernMT.Base;
public static class Throws
{
    public async static Task ApplicationException(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (PluginApplicationException ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }
        Assert.Fail();
    }

    public async static Task MisconfigurationException(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (PluginMisconfigurationException ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }
        Assert.Fail();
    }

    public static void ApplicationException(Action action)
    {
        try
        {
            action();
        }
        catch (PluginApplicationException ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }
        Assert.Fail();
    }

    public static void MisconfigurationException(Action action)
    {
        try
        {
            action();
            Assert.Fail("Expected PluginMisconfigurationException was not thrown");
        }
        catch (PluginMisconfigurationException)
        {
            // Expected exception
        }
        catch (Exception ex)
        {
            Assert.Fail($"Expected PluginMisconfigurationException but got {ex.GetType().Name}");
        }
    }

    public static async Task MisconfigurationExceptionAsync(Func<Task> action)
    {
        try
        {
            await action();
            Assert.Fail("Expected PluginMisconfigurationException was not thrown");
        }
        catch (PluginMisconfigurationException)
        {
            // Expected exception
        }
        catch (Exception ex)
        {
            Assert.Fail($"Expected PluginMisconfigurationException but got {ex.GetType().Name}");
        }
    }
}