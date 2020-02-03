// Decompiled with JetBrains decompiler
// Type: SamEngine.Deck
// Assembly: GooseDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 163C7FB0-432D-4C0F-A75F-86497CB58900
// Assembly location: C:\Users\9613elliotb\Desktop\Desktop Goose v0.21\GooseDesktop.exe

namespace SamEngine
{
  internal class Deck
  {
    public int[] indices;
    private int i;

    public Deck(int Length)
    {
      this.indices = new int[Length];
      this.Reshuffle();
    }

    public void Reshuffle()
    {
      for (int index1 = 0; index1 < this.indices.Length; ++index1)
      {
        this.indices[index1] = index1;
        int index2 = (int) SamMath.RandomRange(0.0f, (float) index1);
        int index3 = this.indices[index1];
        this.indices[index1] = this.indices[index2];
        this.indices[index2] = index3;
      }
    }

    public int Next()
    {
      int index = this.indices[this.i];
      ++this.i;
      if (this.i < this.indices.Length)
        return index;
      this.Reshuffle();
      this.i = 0;
      return index;
    }
  }
}
