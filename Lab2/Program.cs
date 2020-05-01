using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            if (true)
            {
                if (args.Length < 1)
                {
                    Console.WriteLine("Usage; {0} [-t | <filename>]", Process.GetCurrentProcess().ProcessName);
                    return;
                }

                try
                {
                    StreamReader input;

                    if (args[0] == "-t")
                    {
                        input = new StreamReader(Console.OpenStandardInput());
                    }
                    else
                    {
                        input = new StreamReader(args[0]);
                    }

                    string source = input.ReadToEnd();
                    

                    var program = Syntax.ParserUtility.Parse(source);
                    var pretty = program.Pretty();
                    var strip_origi = source.Replace(" ", "");
                    strip_origi = strip_origi.Replace("\n", "");
                    strip_origi = strip_origi.Replace("\r", "");
                    strip_origi = strip_origi.Replace("\r\n", "");
                    strip_origi = strip_origi.Replace(Environment.NewLine, "");

                    var strip_pretty = pretty.Replace(" ", "");
                    strip_pretty = strip_pretty.Replace("\n", "");

                    var new_pretty = Syntax.ParserUtility.Parse(source).Pretty();

                    if (strip_origi.Equals(strip_pretty) && pretty.Equals(new_pretty))
                        Console.WriteLine("True");
                    else
                    {
                        Console.WriteLine("False");
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                String source = @"int j43()
{
  {
    return MZG(false, RG);
    bool f3a;
    bool h;
  }
}
int uG()
{
}
bool Z(int QIg, bool H, bool ek)
{
  {
    {
    }
    if (true)
    {
    }
    else
    {
      if (!(-yfJ||!-true))
      {
        return j;
        bool D;
      }
      else
      {
      }
    }
    bool sx;
  }
  while (Bl = fE(4, Fda = false, kwl())/T3Q(J*25, p8A, -Ga(((vCx = 6)!=true)-(!237-okW))))
  {
    bool qWA;
    {
      {
        if (o = Fn(Rj))
        {
          false;
          {
            while (c)
            {
              int s8;
            }
          }
          return !y(oI(), 5)==true;
        }
        else
        {
        }
      }
      int d;
    }
  }
  if (m(85==175, false, f))
  {
    {
      if (aI()+727||false)
      {
        while (76)
        {
          {
            return V00*((2<=!Hnr(rn, true, IW = FD))+(y = tc = Aks)>Iu);
            while (J(K, jSV, false))
            {
              {
                {
                  false;
                  false;
                  return i = -81;
                }
                while (n())
                {
                  return I = !(j = false);
                  while (h(WJJ = false))
                  {
                    {
                      while (FdP(XlM(t), v(true, Q||Mq(171, !131, ZY = V), Hg = -J(-((h = j(true))-WV), false, rVx = yL(false)))))
                      {
                        6;
                        int czb;
                      }
                      bool Z;
                      return I;
                    }
                  }
                  {
                    return false;
                    tWD(((e4T(xv(true, Q(-false), true), 64, false)>=K)-D(true==true))*(u = Hh<=(JWp = Uq)));
                  }
                }
              }
            }
          }
        }
        while (-da(mHV = true, -(T!=ZUZ(NH = Kv = cUF, k(false<(h = 60)), !false))))
        {
        }
      }
      else
      {
      }
    }
    int fV;
    4;
  }
  else
  {
  }
}";
                
                    var program = Syntax.ParserUtility.Parse(source);
                if (program != null)
                {
                    var pretty = program.Pretty();
                    var strip_origi = source.Replace(" ", "");
                    strip_origi = strip_origi.Replace(Environment.NewLine, "");

                    var strip_pretty = pretty.Replace(" ", "");
                    strip_pretty = strip_pretty.Replace("\n", "");

                    var new_pretty = Syntax.ParserUtility.Parse(source).Pretty();

                    if (strip_origi.Equals(strip_pretty) && pretty.Equals(new_pretty))
                        Console.WriteLine("True");
                    else
                    {
                        Console.WriteLine("False");
                    }


                }
                else
                {
                    Console.WriteLine("False");
                }

            }
            
}
    }
}
