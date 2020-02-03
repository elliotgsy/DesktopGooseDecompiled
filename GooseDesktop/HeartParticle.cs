// Decompiled with JetBrains decompiler
// Type: GooseDesktop.HeartParticle
// Assembly: GooseDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 163C7FB0-432D-4C0F-A75F-86497CB58900
// Assembly location: C:\Users\9613elliotb\Desktop\Desktop Goose v0.21\GooseDesktop.exe

using SamEngine;

namespace GooseDesktop
{
  internal struct HeartParticle
  {
    public Vector2 position;
    public float deathTime;
    private const float lifetime = 3f;
    private const float velY = 150f;
  }
}
