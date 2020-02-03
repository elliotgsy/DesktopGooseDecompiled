// Decompiled with JetBrains decompiler
// Type: SamEngine.Vector2
// Assembly: GooseDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 163C7FB0-432D-4C0F-A75F-86497CB58900
// Assembly location: C:\Users\9613elliotb\Desktop\Desktop Goose v0.21\GooseDesktop.exe

using System;

namespace SamEngine
{
  public struct Vector2
  {
    public static readonly Vector2 zero = new Vector2(0.0f, 0.0f);
    public float x;
    public float y;

    public Vector2(float _x, float _y)
    {
      this.x = _x;
      this.y = _y;
    }

    public static Vector2 operator +(Vector2 a, Vector2 b)
    {
      return new Vector2(a.x + b.x, a.y + b.y);
    }

    public static Vector2 operator -(Vector2 a, Vector2 b)
    {
      return new Vector2(a.x - b.x, a.y - b.y);
    }

    public static Vector2 operator -(Vector2 a)
    {
      return a * -1f;
    }

    public static Vector2 operator *(Vector2 a, Vector2 b)
    {
      return new Vector2(a.x * b.x, a.y * b.y);
    }

    public static Vector2 operator *(Vector2 a, float b)
    {
      return new Vector2(a.x * b, a.y * b);
    }

    public static Vector2 operator /(Vector2 a, float b)
    {
      return new Vector2(a.x / b, a.y / b);
    }

    public static Vector2 GetFromAngleDegrees(float angle)
    {
      return new Vector2((float) Math.Cos((double) angle * (Math.PI / 180.0)), (float) Math.Sin((double) angle * (Math.PI / 180.0)));
    }

    public static float Distance(Vector2 a, Vector2 b)
    {
      Vector2 vector2 = new Vector2(a.x - b.x, a.y - b.y);
      return (float) Math.Sqrt((double) vector2.x * (double) vector2.x + (double) vector2.y * (double) vector2.y);
    }

    public static Vector2 Lerp(Vector2 a, Vector2 b, float p)
    {
      return new Vector2(SamMath.Lerp(a.x, b.x, p), SamMath.Lerp(a.y, b.y, p));
    }

    public static float Dot(Vector2 a, Vector2 b)
    {
      return (float) ((double) a.x * (double) b.x + (double) a.y * (double) b.y);
    }

    public static Vector2 Normalize(Vector2 a)
    {
      if ((double) a.x == 0.0 && (double) a.y == 0.0)
        return Vector2.zero;
      float num = (float) Math.Sqrt((double) a.x * (double) a.x + (double) a.y * (double) a.y);
      return new Vector2(a.x / num, a.y / num);
    }

    public static float Magnitude(Vector2 a)
    {
      return (float) Math.Sqrt((double) a.x * (double) a.x + (double) a.y * (double) a.y);
    }
  }
}
