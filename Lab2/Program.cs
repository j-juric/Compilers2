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

                    source = source.Replace("\n", "");
                    source = source.Replace("\t", "");
                    source = source.Replace("\r", "");
                    source = source.Replace("\r\n", "");
                    source = source.Replace(Environment.NewLine, "");

                    var program = Syntax.ParserUtility.Parse(source);
                    if (program == null)
                    {
                        return;
                    }

                     program.Accept(new EvaluatorVisitor());


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                var source = @"
// can't work with comments
int main(){
	int x;
	int y;
	int z;
	x = 111;
	y = 222;
	z = 333;
	if(x <= y - 80){
		if(56 > z || z >= y){
			while(x < y){
				print(x);
				x = x + 10;
			}
		}
	}
  return 1;
}";
                
                source= source.Replace("\n", "");
                source= source.Replace("\t", "");
                source= source.Replace("\r", "");
                source= source.Replace("\r\n", "");
                source= source.Replace(Environment.NewLine, "");
                var program = Syntax.ParserUtility.Parse(source);
                if (program == null)
                {
                   return;
                }
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
