// Decompiled with JetBrains decompiler
// Type: GooseDesktop.MainGame
// Assembly: GooseDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 163C7FB0-432D-4C0F-A75F-86497CB58900
// Assembly location: C:\Users\9613elliotb\Desktop\Desktop Goose v0.21\GooseDesktop.exe

using SamEngine;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GooseDesktop
{
  public static class MainGame
  {
    private static float curQuitAlpha = 0.0f;
    private static Font showCurQuitFont = new Font("Arial", 12f, FontStyle.Bold);

    public static void Init()
    {
      string toFileInAssembly = Program.GetPathToFileInAssembly("Assets/Images/Memes/");
      try
      {
        Directory.GetFiles(toFileInAssembly);
      }
      catch
      {
        int num = (int) MessageBox.Show("Warning: Some assets expected at the path: \n\n'" + toFileInAssembly + "' \n\ncannot be found. \n\nYour .exe should ideally be next to an Assets folder and config, all bundled together!\n\nPlease make sure you extracted the zip file, with the whole folder together, to a known location like Documents or Desktop- and we didn't end up somewhere random like AppData.\n\nGoose will still work, but he won't be able to use custom memes or any of that fanciness.\nHold ESC for several seconds to quit.");
      }
      GooseConfig.LoadConfig();
      Sound.Init();
      TheGoose.Init();
    }

    public static void Update(Graphics g)
    {
      Time.TickTime();
      if (Program.GetAsyncKeyState(Keys.Escape) != (short) 0)
        MainGame.curQuitAlpha += 0.002166667f;
      else
        MainGame.curQuitAlpha -= 0.01666667f;
      MainGame.curQuitAlpha = SamMath.Clamp(MainGame.curQuitAlpha, 0.0f, 1f);
      if ((double) MainGame.curQuitAlpha > 0.200000002980232)
      {
        float p = (float) (((double) MainGame.curQuitAlpha - 0.200000002980232) / 0.800000011920929);
        int num = (int) SamMath.Lerp(-15f, 10f, Easings.ExponentialEaseOut(p * 2f));
        SizeF sizeF = g.MeasureString("Continue Holding ESC to evict goose", MainGame.showCurQuitFont, int.MaxValue);
        g.FillRectangle(Brushes.LightBlue, new Rectangle(5, num - 5, (int) sizeF.Width + 10, (int) sizeF.Height + 10));
        g.FillRectangle(Brushes.LightPink, new Rectangle(5, num - 5, (int) SamMath.Lerp(0.0f, sizeF.Width + 10f, p), (int) sizeF.Height + 10));
        SolidBrush solidBrush = new SolidBrush(Color.FromArgb((int) byte.MaxValue, (int) (256.0 * (double) MainGame.curQuitAlpha), (int) (256.0 * (double) MainGame.curQuitAlpha), (int) (256.0 * (double) MainGame.curQuitAlpha)));
        g.DrawString("Continue holding ESC to evict goose", MainGame.showCurQuitFont, (Brush) solidBrush, 10f, (float) num);
        solidBrush.Dispose();
      }
      if ((double) MainGame.curQuitAlpha > 0.990000009536743)
        Application.Exit();
      TheGoose.Tick();
      TheGoose.Render(g);
    }
  }
}
