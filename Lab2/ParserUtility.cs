using System.Text;
using System.IO;
using System;

namespace Syntax
{
    public static class ParserUtility
    {
        public static Statement Parse(string prg)
        {
            byte[] data = Encoding.ASCII.GetBytes(prg);
            MemoryStream stream = new MemoryStream(data, 0, data.Length);
            Scanner lexer = new Scanner(stream);
            Parser parser = new Parser(lexer);
            if (parser.Parse())
            {
                //Console.WriteLine("True");
                return parser.Program;      
            }
            else
            {
                //Console.WriteLine("SOMETHING IS VERRRRRY WRONG");
                return null;
            }
        }
    }
}
