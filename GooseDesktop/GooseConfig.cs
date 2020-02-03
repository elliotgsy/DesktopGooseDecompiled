// Decompiled with JetBrains decompiler
// Type: GooseDesktop.GooseConfig
// Assembly: GooseDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 163C7FB0-432D-4C0F-A75F-86497CB58900
// Assembly location: C:\Users\9613elliotb\Desktop\Desktop Goose v0.21\GooseDesktop.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace GooseDesktop
{
  public static class GooseConfig
  {
    private static string filePath = Program.GetPathToFileInAssembly("config.goos");
    public static GooseConfig.ConfigSettings settings = (GooseConfig.ConfigSettings) null;
    public const int GOOSE_CONFIG_VERSION = 0;

    public static void LoadConfig()
    {
      GooseConfig.settings = GooseConfig.ConfigSettings.ReadFileIntoConfig(GooseConfig.filePath);
    }

    public class ConfigSettings
    {
      public float MinWanderingTimeSeconds = 20f;
      public float MaxWanderingTimeSeconds = 40f;
      public float FirstWanderTimeSeconds = 20f;
      public int Version;
      public bool CanAttackAtRandom;

      public static GooseConfig.ConfigSettings ReadFileIntoConfig(string configGivenPath)
      {
        GooseConfig.ConfigSettings f = new GooseConfig.ConfigSettings();
        if (!File.Exists(configGivenPath))
        {
          int num = (int) MessageBox.Show("Can't find config.goos file! Creating a new one with default values");
          GooseConfig.ConfigSettings.WriteConfigToFile(configGivenPath, f);
          return f;
        }
        try
        {
          using (StreamReader streamReader = new StreamReader(configGivenPath))
          {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string str;
            while ((str = streamReader.ReadLine()) != null)
            {
              string[] strArray = str.Split('=');
              if (strArray.Length == 2)
                dictionary.Add(strArray[0], strArray[1]);
            }
            int result = -1;
            int.TryParse(dictionary["Version"], out result);
            if (result != 0)
            {
              int num = (int) MessageBox.Show("config.goos is for the wrong version! Creating a new one with default values!");
              File.Delete(configGivenPath);
              GooseConfig.ConfigSettings.WriteConfigToFile(configGivenPath, f);
              return f;
            }
            foreach (KeyValuePair<string, string> keyValuePair in dictionary)
            {
              FieldInfo field = typeof (GooseConfig.ConfigSettings).GetField(keyValuePair.Key);
              try
              {
                field.SetValue((object) f, Convert.ChangeType((object) keyValuePair.Value, field.FieldType));
              }
              catch
              {
                int num = (int) MessageBox.Show("Loading config error: field " + field.Name + "'s value is not valid. Setting it to the default value.");
              }
            }
          }
        }
        catch
        {
          int num = (int) MessageBox.Show("config.goos corrupt! Creating a new one!");
          File.Delete(configGivenPath);
          GooseConfig.ConfigSettings.WriteConfigToFile(configGivenPath, f);
          return f;
        }
        return f;
      }

      public static void WriteConfigToFile(string path, GooseConfig.ConfigSettings f)
      {
        using (StreamWriter text = File.CreateText(path))
          text.Write(GooseConfig.ConfigSettings.GenerateTextFromSettings(f));
      }

      public static string GenerateTextFromSettings(GooseConfig.ConfigSettings f)
      {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (FieldInfo field in typeof (GooseConfig.ConfigSettings).GetFields())
          stringBuilder.Append(string.Format("{0}={1}\n", (object) field.Name, (object) field.GetValue((object) f).ToString()));
        return stringBuilder.ToString();
      }
    }
  }
}
