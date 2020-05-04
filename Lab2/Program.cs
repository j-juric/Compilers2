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
    int x;
    x = 5;
    x = factorial(x);
    print(x);
    x = 6;
    x = fibonacci(x);
    print(x);
    print(func());
    foo();
}
int factorial(int x)
{
    if(x==1)
        return 1;
    return x * factorial(x-1);
}
int fibonacci(int x)
{
    if( x==1 || x==0)
        return x;
    return fibonacci(x-1) + fibonacci(x-2);
}
bool xor(bool a, bool b)
{
    return !a&&b || a&&!b;
}
int func(){
    return 777;
    return 666;
}
void foo()
{   
    print(1,3,3,7);
}
";
                
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
