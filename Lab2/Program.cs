using System;
using System.Diagnostics;
using System.IO;

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
                    //var source = @"";
                    var program = Syntax.ParserUtility.Parse(source);
                    if (program != null)
                    {
                        var pretty = program.Pretty();
                        //Console.WriteLine(pretty);
                        program = Syntax.ParserUtility.Parse(pretty);
                        var newPretty = program.Pretty();
                        //Console.WriteLine(newPretty);
                        if (newPretty.Equals(pretty))
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
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                var source = @"void A8(int O2)
{
  if (!8)
  {
    SOq;
  }
  else
  {
    {
      {
        if (om = k5 = -qy(-(FT>bq(-70, tKa)), -(sc(hzx, t = false)/O(67, m = m = Au2 = (x = m)&&R(KuJ(), -(-true&&807)))&&-FE8(false)), gag = r8l = YY = true))
        {
          T0z;
          if (-F)
          {
            {
              if (386)
              {
                if (8)
                {
                  {
                    bool dW;
                  }
                }
                else
                {
                  return !o(mTb = K = --fW7(33));
                }
                return in(true, WX(!false), WAU = 82)!=Fb();
                while (false)
                {
                }
              }
              else
              {
              }
              return (HgY = 4)+true;
              if (86<406)
              {
                while (false)
                {
                }
                {
                  while (IP)
                  {
                    {
                    }
                    GfL;
                  }
                  if (uH)
                  {
                    {
                      {
                        {
                          int HSI;
                          {
                            bool SKb;
                            while (G = Z)
                            {
                              while (true)
                              {
                              }
                              int mF;
                            }
                            lo;
                          }
                        }
                        while ((-false||621)+!!13)
                        {
                          {
                          }
                        }
                        6;
                      }
                      {
                        return SiX = (x = VR)==FN;
                      }
                    }
                    {
                    }
                  }
                  else
                  {
                    return f;
                  }
                }
              }
              else
              {
                while (H0)
                {
                }
              }
            }
            int EdH;
          }
          else
          {
          }
        }
        else
        {
        }
        {
          !!GJ;
        }
        if (false)
        {
          while (w = false)
          {
            bool Q;
          }
        }
        else
        {
        }
      }
    }
  }
  return th3;
}
int hg(bool FI, int pgP, bool k4p)
{
  {
  }
}";
                var program = Syntax.ParserUtility.Parse(source);
                if (program != null)
                {
                    var pretty = program.Pretty();
                    Console.WriteLine(pretty);
                    program = Syntax.ParserUtility.Parse(pretty);
                    var newPretty = program.Pretty();
                    Console.WriteLine("**************************************************");
                    Console.WriteLine(newPretty);
                    if (newPretty.Equals(pretty))
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
