// Decompiled with JetBrains decompiler
// Type: GooseDesktop.Properties.Resources
// Assembly: GooseDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 163C7FB0-432D-4C0F-A75F-86497CB58900
// Assembly location: C:\Users\9613elliotb\Desktop\Desktop Goose v0.21\GooseDesktop.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;

namespace GooseDesktop.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (GooseDesktop.Properties.Resources.resourceMan == null)
          GooseDesktop.Properties.Resources.resourceMan = new ResourceManager("GooseDesktop.Properties.Resources", typeof (GooseDesktop.Properties.Resources).Assembly);
        return GooseDesktop.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return GooseDesktop.Properties.Resources.resourceCulture;
      }
      set
      {
        GooseDesktop.Properties.Resources.resourceCulture = value;
      }
    }

    internal static UnmanagedMemoryStream Pat1
    {
      get
      {
        return GooseDesktop.Properties.Resources.ResourceManager.GetStream(nameof (Pat1), GooseDesktop.Properties.Resources.resourceCulture);
      }
    }

    internal static UnmanagedMemoryStream Pat2
    {
      get
      {
        return GooseDesktop.Properties.Resources.ResourceManager.GetStream(nameof (Pat2), GooseDesktop.Properties.Resources.resourceCulture);
      }
    }

    internal static UnmanagedMemoryStream Pat3
    {
      get
      {
        return GooseDesktop.Properties.Resources.ResourceManager.GetStream(nameof (Pat3), GooseDesktop.Properties.Resources.resourceCulture);
      }
    }
  }
}
