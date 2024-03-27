namespace BancoCentralCrawler.Services.Helpers;

public static class CircuitBreakerHelper
{
    public static async Task<T?> TryNTimesAsync<T>(Func<Task<T>> func, int times, int millisecondsWaitingTime)
    {
        while (times > 0)
        {
            try
            {
                return await func();
            }
            catch (Exception)
            {
                await Task.Delay(millisecondsWaitingTime);

                if (--times <= 0) break;
            }
        }

        return default;
    }
        
    public static T? TryNTimes<T>(Func<T> func, int times, int millisecondsWaitingTime)
    {
        while (times > 0)
        {
            try
            {
                return func();
            }
            catch (Exception)
            {
                Thread.Sleep(millisecondsWaitingTime);

                if (--times <= 0) break;
            }
        }

        return default;
    }

    public static void TryNTimes(Action func, int times, int millisecondsWaitingTime)
    {
        while (times > 0)
        {
            try
            {
                func();
            }
            catch (Exception)
            {
                Thread.Sleep(millisecondsWaitingTime);

                if (--times <= 0) break;
            }
        }
    }
}