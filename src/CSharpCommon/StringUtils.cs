// **********************
// *                    *
// *  String Utilities  *
// *                    *
// **********************

using System;

namespace CSharpCommon
{
  public static class StringUtil
  {
    public static string plural(int n)
    {
      return n == 1 ? "" : "s";
    }

    public static string Engineering(double x)
    {
      const string suffix = "<yzafpnum kMGTPEZY>";
      const int zero = 9;
      string[] formats = {"{0,6:f3}{1}", "{0,6:f2}{1}", "{0,6:f1}{1}"};

      int mag = 0;
      if (x != 0.0)
        mag = (int)Math.Floor(Math.Log10(Math.Abs(x)));
      int phase = MathUtil.mod(mag, 3);
      mag = (mag - phase) / 3;

      x *= Math.Pow(1000.0, -mag);
      int i = Math.Min(Math.Max(mag + zero, 0), suffix.Length - 1);
      return string.Format(formats[phase], x, suffix[i]);
    }
  }
}
