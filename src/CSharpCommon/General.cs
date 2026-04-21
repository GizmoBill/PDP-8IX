// ************************************
// *                                  *
// *  General-Purpose Math Utilities  *
// *                                  *
// ************************************

using System;

namespace CSharpCommon
{
  public static class MathUtil
  {
    // ****************
    // *              *
    // *  Math Utils  *
    // *              *
    // ****************

    public static int    square(int    x) { return x * x;}
    public static double square(double x) { return x * x;}

    public static double Radians(double degrees) { return degrees * Math.PI / 180.0;}
    public static double Degrees(double radians) { return radians * 180.0 / Math.PI;}

    public static double EuclidDist(double x, double y) { return Math.Sqrt(x * x + y * y); }

    public static bool ApproxEq(double x, double y, double epsilon = 1.0e-12f)
    {
      if (epsilon == 0)
        return x == y;

      if (x == 0 || y == 0)
        return Math.Abs(x + y) <= epsilon;

      return Math.Abs(x - y) <= epsilon * (Math.Abs(x) + Math.Abs(y));
    }

    // Integer modulo, properly defined (result is 0 or its sign matches sign of y) instead of the
    // stupid way that all compilers do it. For example, mod(-1, 10) is 9, not -1. mod(1, -10) is -9.
    // y must not be 0.
    public static int mod(int x, int y)
    {
      int m = x % y;
      if ((m ^ y) < 0 && m != 0)
        m += y;
      return m;
    }

    // Floating point modulo, result is 0 or its sign matches sign of y. y must not be 0.
    public static double mod(double x, double y)
    {
      return x - Math.Floor(x / y) * y;
    }

    // Floating point "signed" modulo. Some examples:
    //    smod(  45,  180) =   45
    //    smod( 135,  180) =  -45
    //    smod( -45,  180) =  -45
    //    smod(-135,  180) =   45
    //    smod(  90,  180) =  -90
    //    smod(  45, -180) = -135
    //    smod( 135, -180) =  135
    //    smod( -45, -180) =  135
    //    smod(-135, -180) = -135
    //    smod(  90, -180) =   90
    // smod is used, for example, to covert an arbitrary angle in degrees or radians to its equivalent
    // in the range +-180, +-90, +-PI/2, etc.
    public static double smod(double x, double y)
    {
      double m = mod(x, y);
      if (2.0 * m >= y)
        m -= y;
      return m;
    }

    // Parabolic interpolation. Result is in the range +-0.5.
    public static double parabolic(double left, double center, double right)
    {
        double d = 2 * center - (left + right);
        d = d != 0.0 ? (right - left) / (2.0 * d) : 0.0;
        d = Math.Min(d, 0.5);
        d = Math.Max(d, -0.5);
        return d;
    }

    // Stardard deviation
    public static double StDev(double sumSq, double sum, double weight)
    {
      return weight > 0 ? Math.Sqrt(Math.Max(weight * sumSq - sum * sum, 0)) / weight : 0.0;
    }

    // Gaussian. Peak is 1.0 when normalized is false; area under curve is 1.0 when normalized is
    // true.
    public static double Gauss(double x, double mean, double stDev, bool normalized)
    {
      double g = Math.Exp(-0.5 * square((x - mean) / stDev));
      if (normalized)
        g /= stDev * Math.Sqrt(2.0 * Math.PI);
      return g;
    }

    // Inverse of Gaussian
    public static double GaussInv(double g, double mean, double stDev, bool normalized)
    {
      if (normalized)
        g *= stDev * Math.Sqrt(2.0 * Math.PI);
      return stDev * Math.Sqrt(-2 * Math.Log(g)) + mean;
    }

    // Cumulative of Gaussian.
    // http://en.wikipedia.org/wiki/Normal_distribution#Numerical_approximations_for_the_normal_cdf
    public static double GaussCum(double x, double mean, double stDev, bool normalized)
    {
      const double err = 1.0e-9;
      double y = (x - mean) / stDev;
      double y2 = y * y;
      double term = y;
      double sum = y;
      double f = 1.0;

      while (term > sum * err)
      {
        f += 2.0;
        term *= y2 / f;
        sum += term;
      }

      double gc = 0.5 + Gauss(y, 0, 1.0, true) * sum;
      if (!normalized)
        gc *= stDev * Math.Sqrt(2.0 * Math.PI);
      return gc;
    }

    // Greatest common divisor. a and b can be any integers, result is always non-negative.
    // If one argument is 0, gcd is other argument. 
    public static int Gcd(int a, int b)
    {
      a = Math.Abs(a);
      b = Math.Abs(b);
      if (a > b)
      {
        a ^= b;
        b ^= a;
        a ^= b;
      }

      if (a == 0)
        return b;

      while (a > 1 && a != b)
      {
        int d = b - a;
        if (d >= a)
          b = d;
        else
        {
          b = a;
          a = d;
        }
      }
      return a;
    }

    public static string AddCommas(long n)
    {
      string src = n.ToString();
      string dst = string.Empty;

      int p;

      if (n >= 0)
        p = 0;
      else
      {
        p = 1;
        dst += '-';
      }

      int leadingDigits = (src.Length - p) % 3;
      if (leadingDigits == 0)
        leadingDigits = 3;

      dst += src.Substring(p, leadingDigits);

      for (p += leadingDigits; p < src.Length; p += 3)
      {
        dst += ',';
        dst += src.Substring(p, 3);
      }

      return dst;
    }
  }

  // *****************************
  // *                           *
  // *  Gaussian Random Numbers  *
  // *                           *
  // *****************************

  public class GaussRandom : Random
  {
    private double a_, b_;
    private void setK()
    {
      double k = Sigma * Math.Sqrt(3.0 / Order);
      a_ = 2 * k;
      b_ = Mean - k * Order;
    }

    private int order_ = 3;
    public int Order
    {
      get { return order_;}
      set { order_ = Math.Min(Math.Max(value, 1), 12); setK(); }
    }

    private double mean_ = 0;
    public double Mean
    {
      get { return mean_;}
      set { mean_ = value; setK();}
    }

    private double sigma_ = 1.0;
    public double Sigma
    {
      get { return sigma_;}
      set { sigma_ = value; setK();}
    }

    public GaussRandom()
    {
      setK();
    }

    protected override double Sample()
    {
      double g = 0;
      for (int i = 0; i < Order; ++i)
        g += base.Sample();
      return a_ * g + b_;
    }
  }
}