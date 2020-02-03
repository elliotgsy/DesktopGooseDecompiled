// Decompiled with JetBrains decompiler
// Type: SamEngine.Time
// Assembly: GooseDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 163C7FB0-432D-4C0F-A75F-86497CB58900
// Assembly location: C:\Users\9613elliotb\Desktop\Desktop Goose v0.21\GooseDesktop.exe

using System.Diagnostics;

namespace SamEngine
{
  public static class Time
  {
    public static Stopwatch timeStopwatch = new Stopwatch();
    public const int framerate = 120;
    public const float deltaTime = 0.008333334f;
    public static float time;

    static Time()
    {
      Time.timeStopwatch.Start();
      Time.TickTime();
    }

    public static void TickTime()
    {
      Time.time = (float) Time.timeStopwatch.Elapsed.TotalSeconds;
    }
  }
}
