using Evaluator;
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
            if (false)
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
                    strip_pretty = strip_pretty.Replace("\r", "");
                    strip_pretty = strip_pretty.Replace("\r\n", "");

                    var new_pretty = Syntax.ParserUtility.Parse(source).Pretty();


                    // strip_origi.Equals(strip_pretty) Checks for the 1st requirement  strip(p) == strip(pretty(parse(p)))
                    //pretty.Equals(new_pretty) Checks for the 2nd requirement pretty(parse(p)) == pretty(parse(pretty(parse(p))))
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
                var source = @"
void main()
{ 
    bool x; 
    int y; 
    x=true; 
    y=2; 
    while(!(-y==-2048)) 
    { 
        print(y); 
        y = y*2;  
    } 
}
";
                source = source.Replace(Environment.NewLine, "");
                var program = Syntax.ParserUtility.Parse(source);
                try
                {
                 program.Accept(new EvaluatorVisitor());

                }
                catch(EvaluationError e)
                {
                  Console.WriteLine(e.Message);
                }                

            }
            
}
    }
}
