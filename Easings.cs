// Decompiled with JetBrains decompiler
// Type: Easings
// Assembly: GooseDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 163C7FB0-432D-4C0F-A75F-86497CB58900
// Assembly location: C:\Users\9613elliotb\Desktop\Desktop Goose v0.21\GooseDesktop.exe

using System;

public static class Easings
{
  private const float PI = 3.141593f;
  private const float HALFPI = 1.570796f;

  public static float Interpolate(float p, Easings.Functions function)
  {
    switch (function)
    {
      case Easings.Functions.QuadraticEaseIn:
        return Easings.QuadraticEaseIn(p);
      case Easings.Functions.QuadraticEaseOut:
        return Easings.QuadraticEaseOut(p);
      case Easings.Functions.QuadraticEaseInOut:
        return Easings.QuadraticEaseInOut(p);
      case Easings.Functions.CubicEaseIn:
        return Easings.CubicEaseIn(p);
      case Easings.Functions.CubicEaseOut:
        return Easings.CubicEaseOut(p);
      case Easings.Functions.CubicEaseInOut:
        return Easings.CubicEaseInOut(p);
      case Easings.Functions.QuarticEaseIn:
        return Easings.QuarticEaseIn(p);
      case Easings.Functions.QuarticEaseOut:
        return Easings.QuarticEaseOut(p);
      case Easings.Functions.QuarticEaseInOut:
        return Easings.QuarticEaseInOut(p);
      case Easings.Functions.QuinticEaseIn:
        return Easings.QuinticEaseIn(p);
      case Easings.Functions.QuinticEaseOut:
        return Easings.QuinticEaseOut(p);
      case Easings.Functions.QuinticEaseInOut:
        return Easings.QuinticEaseInOut(p);
      case Easings.Functions.SineEaseIn:
        return Easings.SineEaseIn(p);
      case Easings.Functions.SineEaseOut:
        return Easings.SineEaseOut(p);
      case Easings.Functions.SineEaseInOut:
        return Easings.SineEaseInOut(p);
      case Easings.Functions.CircularEaseIn:
        return Easings.CircularEaseIn(p);
      case Easings.Functions.CircularEaseOut:
        return Easings.CircularEaseOut(p);
      case Easings.Functions.CircularEaseInOut:
        return Easings.CircularEaseInOut(p);
      case Easings.Functions.ExponentialEaseIn:
        return Easings.ExponentialEaseIn(p);
      case Easings.Functions.ExponentialEaseOut:
        return Easings.ExponentialEaseOut(p);
      case Easings.Functions.ExponentialEaseInOut:
        return Easings.ExponentialEaseInOut(p);
      case Easings.Functions.ElasticEaseIn:
        return Easings.ElasticEaseIn(p);
      case Easings.Functions.ElasticEaseOut:
        return Easings.ElasticEaseOut(p);
      case Easings.Functions.ElasticEaseInOut:
        return Easings.ElasticEaseInOut(p);
      case Easings.Functions.BackEaseIn:
        return Easings.BackEaseIn(p);
      case Easings.Functions.BackEaseOut:
        return Easings.BackEaseOut(p);
      case Easings.Functions.BackEaseInOut:
        return Easings.BackEaseInOut(p);
      case Easings.Functions.BounceEaseIn:
        return Easings.BounceEaseIn(p);
      case Easings.Functions.BounceEaseOut:
        return Easings.BounceEaseOut(p);
      case Easings.Functions.BounceEaseInOut:
        return Easings.BounceEaseInOut(p);
      default:
        return Easings.Linear(p);
    }
  }

  public static float Linear(float p)
  {
    return p;
  }

  public static float QuadraticEaseIn(float p)
  {
    return p * p;
  }

  public static float QuadraticEaseOut(float p)
  {
    return (float) -((double) p * ((double) p - 2.0));
  }

  public static float QuadraticEaseInOut(float p)
  {
    return (double) p < 0.5 ? 2f * p * p : (float) (-2.0 * (double) p * (double) p + 4.0 * (double) p - 1.0);
  }

  public static float CubicEaseIn(float p)
  {
    return p * p * p;
  }

  public static float CubicEaseOut(float p)
  {
    float num = p - 1f;
    return (float) ((double) num * (double) num * (double) num + 1.0);
  }

  public static float CubicEaseInOut(float p)
  {
    if ((double) p < 0.5)
      return 4f * p * p * p;
    float num = (float) (2.0 * (double) p - 2.0);
    return (float) (0.5 * (double) num * (double) num * (double) num + 1.0);
  }

  public static float QuarticEaseIn(float p)
  {
    return p * p * p * p;
  }

  public static float QuarticEaseOut(float p)
  {
    float num = p - 1f;
    return (float) ((double) num * (double) num * (double) num * (1.0 - (double) p) + 1.0);
  }

  public static float QuarticEaseInOut(float p)
  {
    if ((double) p < 0.5)
      return 8f * p * p * p * p;
    float num = p - 1f;
    return (float) (-8.0 * (double) num * (double) num * (double) num * (double) num + 1.0);
  }

  public static float QuinticEaseIn(float p)
  {
    return p * p * p * p * p;
  }

  public static float QuinticEaseOut(float p)
  {
    float num = p - 1f;
    return (float) ((double) num * (double) num * (double) num * (double) num * (double) num + 1.0);
  }

  public static float QuinticEaseInOut(float p)
  {
    if ((double) p < 0.5)
      return 16f * p * p * p * p * p;
    float num = (float) (2.0 * (double) p - 2.0);
    return (float) (0.5 * (double) num * (double) num * (double) num * (double) num * (double) num + 1.0);
  }

  public static float SineEaseIn(float p)
  {
    return (float) Math.Sin(((double) p - 1.0) * 1.57079637050629) + 1f;
  }

  public static float SineEaseOut(float p)
  {
    return (float) Math.Sin((double) p * 1.57079637050629);
  }

  public static float SineEaseInOut(float p)
  {
    return (float) (0.5 * (1.0 - Math.Cos((double) p * 3.14159274101257)));
  }

  public static float CircularEaseIn(float p)
  {
    return 1f - (float) Math.Sqrt(1.0 - (double) p * (double) p);
  }

  public static float CircularEaseOut(float p)
  {
    return (float) Math.Sqrt((2.0 - (double) p) * (double) p);
  }

  public static float CircularEaseInOut(float p)
  {
    return (double) p < 0.5 ? (float) (0.5 * (1.0 - Math.Sqrt(1.0 - 4.0 * ((double) p * (double) p)))) : (float) (0.5 * (Math.Sqrt(-(2.0 * (double) p - 3.0) * (2.0 * (double) p - 1.0)) + 1.0));
  }

  public static float ExponentialEaseIn(float p)
  {
    return (double) p != 0.0 ? (float) Math.Pow(2.0, 10.0 * ((double) p - 1.0)) : p;
  }

  public static float ExponentialEaseOut(float p)
  {
    return (double) p != 1.0 ? 1f - (float) Math.Pow(2.0, -10.0 * (double) p) : p;
  }

  public static float ExponentialEaseInOut(float p)
  {
    if ((double) p == 0.0 || (double) p == 1.0)
      return p;
    return (double) p < 0.5 ? 0.5f * (float) Math.Pow(2.0, 20.0 * (double) p - 10.0) : (float) (-0.5 * Math.Pow(2.0, -20.0 * (double) p + 10.0) + 1.0);
  }

  public static float ElasticEaseIn(float p)
  {
    return (float) Math.Sin(20.420352935791 * (double) p) * (float) Math.Pow(2.0, 10.0 * ((double) p - 1.0));
  }

  public static float ElasticEaseOut(float p)
  {
    return (float) (Math.Sin(-20.420352935791 * ((double) p + 1.0)) * Math.Pow(2.0, -10.0 * (double) p) + 1.0);
  }

  public static float ElasticEaseInOut(float p)
  {
    return (double) p < 0.5 ? 0.5f * (float) Math.Sin(20.420352935791 * (2.0 * (double) p)) * (float) Math.Pow(2.0, 10.0 * (2.0 * (double) p - 1.0)) : (float) (0.5 * (Math.Sin(-20.420352935791 * (2.0 * (double) p - 1.0 + 1.0)) * Math.Pow(2.0, -10.0 * (2.0 * (double) p - 1.0)) + 2.0));
  }

  public static float BackEaseIn(float p)
  {
    return (float) ((double) p * (double) p * (double) p - (double) p * Math.Sin((double) p * 3.14159274101257));
  }

  public static float BackEaseOut(float p)
  {
    float num = 1f - p;
    return (float) (1.0 - ((double) num * (double) num * (double) num - (double) num * Math.Sin((double) num * 3.14159274101257)));
  }

  public static float BackEaseInOut(float p)
  {
    if ((double) p < 0.5)
    {
      float num = 2f * p;
      return (float) (0.5 * ((double) num * (double) num * (double) num - (double) num * Math.Sin((double) num * 3.14159274101257)));
    }
    float num1 = (float) (1.0 - (2.0 * (double) p - 1.0));
    return (float) (0.5 * (1.0 - ((double) num1 * (double) num1 * (double) num1 - (double) num1 * Math.Sin((double) num1 * 3.14159274101257))) + 0.5);
  }

  public static float BounceEaseIn(float p)
  {
    return 1f - Easings.BounceEaseOut(1f - p);
  }

  public static float BounceEaseOut(float p)
  {
    if ((double) p < 0.363636374473572)
      return (float) (121.0 * (double) p * (double) p / 16.0);
    if ((double) p < 0.727272748947144)
      return (float) (9.07499980926514 * (double) p * (double) p - 9.89999961853027 * (double) p + 3.40000009536743);
    return (double) p < 0.899999976158142 ? (float) (12.066481590271 * (double) p * (double) p - 19.6354579925537 * (double) p + 8.89806079864502) : (float) (10.8000001907349 * (double) p * (double) p - 20.5200004577637 * (double) p + 10.7200002670288);
  }

  public static float BounceEaseInOut(float p)
  {
    return (double) p < 0.5 ? 0.5f * Easings.BounceEaseIn(p * 2f) : (float) (0.5 * (double) Easings.BounceEaseOut((float) ((double) p * 2.0 - 1.0)) + 0.5);
  }

  public enum Functions
  {
    Linear,
    QuadraticEaseIn,
    QuadraticEaseOut,
    QuadraticEaseInOut,
    CubicEaseIn,
    CubicEaseOut,
    CubicEaseInOut,
    QuarticEaseIn,
    QuarticEaseOut,
    QuarticEaseInOut,
    QuinticEaseIn,
    QuinticEaseOut,
    QuinticEaseInOut,
    SineEaseIn,
    SineEaseOut,
    SineEaseInOut,
    CircularEaseIn,
    CircularEaseOut,
    CircularEaseInOut,
    ExponentialEaseIn,
    ExponentialEaseOut,
    ExponentialEaseInOut,
    ElasticEaseIn,
    ElasticEaseOut,
    ElasticEaseInOut,
    BackEaseIn,
    BackEaseOut,
    BackEaseInOut,
    BounceEaseIn,
    BounceEaseOut,
    BounceEaseInOut,
  }
}
