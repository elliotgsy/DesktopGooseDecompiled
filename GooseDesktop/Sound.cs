// Decompiled with JetBrains decompiler
// Type: GooseDesktop.Sound
// Assembly: GooseDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 163C7FB0-432D-4C0F-A75F-86497CB58900
// Assembly location: C:\Users\9613elliotb\Desktop\Desktop Goose v0.21\GooseDesktop.exe

using GooseDesktop.Properties;
using SamEngine;
using System;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;

namespace GooseDesktop
{
  internal static class Sound
  {
    private static readonly Stream[] patSources = new Stream[3]
    {
      (Stream) Resources.Pat1,
      (Stream) Resources.Pat2,
      (Stream) Resources.Pat3
    };
    private static readonly string[] honkSources = new string[4]
    {
      Program.GetPathToFileInAssembly("Assets/Sound/NotEmbedded/Honk1.mp3"),
      Program.GetPathToFileInAssembly("Assets/Sound/NotEmbedded/Honk2.mp3"),
      Program.GetPathToFileInAssembly("Assets/Sound/NotEmbedded/Honk3.mp3"),
      Program.GetPathToFileInAssembly("Assets/Sound/NotEmbedded/Honk4.mp3")
    };
    private static readonly string biteSource = Program.GetPathToFileInAssembly("Assets/Sound/NotEmbedded/BITE.mp3");
    public static Sound.Mp3Player honkBiteSoundPlayer;
    public static Sound.Mp3Player musicPlayer;
    public static Sound.Mp3Player environmentSoundsPlayer;
    private static SoundPlayer[] patSoundPool;

    public static void Init()
    {
      Sound.honkBiteSoundPlayer = new Sound.Mp3Player(Sound.honkSources[0], "honkPlayer");
      Sound.patSoundPool = new SoundPlayer[Sound.patSources.Length];
      for (int index = 0; index < Sound.patSources.Length; ++index)
      {
        //Sound.patSoundPool[index] = new SoundPlayer(Sound.patSources[index]);
        //Sound.patSoundPool[index].Load();
      }
      Sound.environmentSoundsPlayer = new Sound.Mp3Player(Program.GetPathToFileInAssembly("Assets/Sound/NotEmbedded/MudSquith.mp3"), "assortedEnvironment");
      string toFileInAssembly = Program.GetPathToFileInAssembly("Assets/Sound/Music/Music.mp3");
      if (!File.Exists(toFileInAssembly))
        return;
      Sound.musicPlayer = new Sound.Mp3Player(toFileInAssembly, "musicPlayer");
      Sound.musicPlayer.loop = true;
      Sound.musicPlayer.SetVolume(0.5f);
      Sound.musicPlayer.Play();
    }

    public static void PlayPat()
    {
            /*
      int index = (int) (SamMath.Rand.NextDouble() * (double) Sound.patSoundPool.Length);
      SoundPlayer soundPlayer = Sound.patSoundPool[index];
      if (soundPlayer.Stream.CanSeek)
        soundPlayer.Stream.Seek(0L, SeekOrigin.Begin);
      soundPlayer.Play();
      */
    }

    public static void HONCC()
    {
      int index = (int) (SamMath.Rand.NextDouble() * (double) Sound.honkSources.Length);
      Sound.honkBiteSoundPlayer.Pause();
      Sound.honkBiteSoundPlayer.Dispose();
      Sound.honkBiteSoundPlayer.ChangeFile(Sound.honkSources[index]);
      Sound.honkBiteSoundPlayer.SetVolume(0.8f);
      Sound.honkBiteSoundPlayer.Play();
    }

    public static void CHOMP()
    {
      Sound.honkBiteSoundPlayer.Pause();
      Sound.honkBiteSoundPlayer.Dispose();
      Sound.honkBiteSoundPlayer.ChangeFile(Sound.biteSource);
      Sound.honkBiteSoundPlayer.SetVolume(0.07f);
      Sound.honkBiteSoundPlayer.Play();
    }

    public static void PlayMudSquith()
    {
      Sound.environmentSoundsPlayer.Restart();
      Sound.environmentSoundsPlayer.Play();
    }

    public class Mp3Player
    {
      public bool loop;
      private string alias;

      public Mp3Player(string filename, string playerAlias)
      {
        this.alias = playerAlias;
        Sound.Mp3Player.mciSendString(string.Format("open \"{0}\" type MPEGVideo alias {1}", (object) filename, (object) this.alias), (StringBuilder) null, 0, IntPtr.Zero);
      }

      public void Play()
      {
        string strCommand = string.Format("play {0}", (object) this.alias);
        if (this.loop)
          strCommand += " REPEAT";
        Sound.Mp3Player.mciSendString(strCommand, (StringBuilder) null, 0, IntPtr.Zero);
      }

      public void Pause()
      {
        Sound.Mp3Player.mciSendString(string.Format("stop {0}", (object) this.alias), (StringBuilder) null, 0, IntPtr.Zero);
      }

      public void SetVolume(float volume)
      {
        Sound.Mp3Player.mciSendString(string.Format("setaudio {0} volume to {1}", (object) this.alias, (object) (int) Math.Max(Math.Min(volume * 1000f, 1000f), 0.0f)), (StringBuilder) null, 0, IntPtr.Zero);
      }

      public void Dispose()
      {
        Sound.Mp3Player.mciSendString(string.Format("close {0}", (object) this.alias), (StringBuilder) null, 0, IntPtr.Zero);
      }

      public void ChangeFile(string newFilePath)
      {
        Sound.Mp3Player.mciSendString(string.Format("open \"{0}\" type MPEGVideo alias {1}", (object) newFilePath, (object) this.alias), (StringBuilder) null, 0, IntPtr.Zero);
      }

      public void Restart()
      {
        Sound.Mp3Player.mciSendString(string.Format("seek {0} to start", (object) this.alias), (StringBuilder) null, 0, IntPtr.Zero);
      }

      [DllImport("winmm.dll")]
      private static extern long mciSendString(
        string strCommand,
        StringBuilder strReturn,
        int iReturnLength,
        IntPtr hWndCallback);
    }
  }
}
