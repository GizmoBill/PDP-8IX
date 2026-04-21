// ********************
// *                  *
// *  File Utilities  *
// *                  *
// ********************

using System.IO;

namespace CSharpCommon
{
  public static class FileUtil
  {
    // If the specified file exists, rename it by appending "_BACKUPnn" (keeping the existing
    // extension), where 00 <= nn <= 99 with nn chosen so that the renamed file does not
    // already exist.
    public static void fileBackup(string filename)
    {
      if (File.Exists(filename))
      {
        for (int i = 0; i < 100; ++i)
        {
          string backup = filename.Replace(".", string.Format("_BACKUP{0:d2}.", i));
          if (!File.Exists(backup))
          {
            File.Move(filename, backup);
            break;
          }
        }
      }
    }

    public static string scanFiles(string fileName, int next)
    {
      string[] files = Directory.GetFiles(Path.GetDirectoryName(fileName), "*" + Path.GetExtension(fileName));
      if (files.Length <= 1)
        return null;

      int i;
      for (i = 0; i < files.Length; ++i)
        if (files[i] == fileName)
          break;

      if (i == files.Length)
        i = 0;  // not found, start with first
      else
        i = (i + next + files.Length) % files.Length;

      return files[i];
    }
  }
}
