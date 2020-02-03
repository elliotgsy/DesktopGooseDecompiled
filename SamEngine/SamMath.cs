// Decompiled with JetBrains decompiler
// Type: SamEngine.SamMath
// Assembly: GooseDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 163C7FB0-432D-4C0F-A75F-86497CB58900
// Assembly location: C:\Users\9613elliotb\Desktop\Desktop Goose v0.21\GooseDesktop.exe

using System;

namespace SamEngine
{
  public static class SamMath
  {
    public static Random Rand = new Random();
    public const float Deg2Rad = 0.01745329f;
    public const float Rad2Deg = 57.29578f;

    public static float RandomRange(float min, float max)
    {
      return min + (float) SamMath.Rand.NextDouble() * (max - min);
    }

    public static float Lerp(float a, float b, float p)
    {
      return (float) ((double) a * (1.0 - (double) p) + (double) b * (double) p);
    }

    public static float Clamp(float a, float min, float max)
    {
      return Math.Min(Math.Max(a, min), max);
    }
  }
}
