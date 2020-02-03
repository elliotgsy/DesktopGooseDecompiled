// Decompiled with JetBrains decompiler
// Type: GooseDesktop.BufferedPanel
// Assembly: GooseDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 163C7FB0-432D-4C0F-A75F-86497CB58900
// Assembly location: C:\Users\9613elliotb\Desktop\Desktop Goose v0.21\GooseDesktop.exe

using System.Windows.Forms;

namespace GooseDesktop
{
  public class BufferedPanel : Panel
  {
    public BufferedPanel()
    {
      this.DoubleBuffered = true;
    }
  }
}
