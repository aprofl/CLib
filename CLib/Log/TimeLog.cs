using System.Diagnostics;

public static class TimeLog
{
    static Stopwatch sw = new Stopwatch();
    public static void StartNew()
    {
        sw.Restart();
    }

    public static void Add(string str)
    {
        Debug.WriteLine($"[{sw.ElapsedMilliseconds}] {str}");
    }
}