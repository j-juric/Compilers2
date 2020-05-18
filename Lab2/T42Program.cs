using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2
{
    public class T42Program
    {
        public Dictionary<string, int> AddressMap = new Dictionary<string, int>();
        public List<T42Instruction> Instructions = new List<T42Instruction>(100);

        public void Emit(T42Instruction inst)
        {
            if (inst.opcode == T42Instruction.OPCODE.LABEL)
            {
                AddressMap[inst.target] = Instructions.Count;
            }

            Instructions.Add(inst);
        }

     


        public override string ToString()
        {
            StringBuilder b = new StringBuilder();

            var lineno = 0;
            foreach (var i in Instructions)
            {
                b.AppendFormat("{0, -3} {1}\n", lineno++, i.ToString());
            }

            return b.ToString();
        }


        public void Link()
        {
            foreach (var inst in Instructions)
            {
                switch (inst.opcode)
                {
                    case T42Instruction.OPCODE.BRA:
                    case T42Instruction.OPCODE.BRF:
                    case T42Instruction.OPCODE.BSR:
                        inst.target = AddressMap[inst.target].ToString();
                        break;
                }
            }
        }
    }

    public class T42Instruction
    {
        public static T42Instruction PUSHINT(int n) => new T42Instruction(OPCODE.PUSHINT, n);
        public static T42Instruction PUSHBOOL(int n) => new T42Instruction(OPCODE.PUSHBOOL, n);

        public static T42Instruction DECL(int n) => new T42Instruction(OPCODE.DECL, n);
        public static T42Instruction LVAL(int n) => new T42Instruction(OPCODE.LVAL, n);
        public static T42Instruction LABEL(string n) => new T42Instruction(OPCODE.LABEL, n);
        public static T42Instruction BRA(string n) => new T42Instruction(OPCODE.BRA, n);
        public static T42Instruction BRF(string n) => new T42Instruction(OPCODE.BRF, n);
        public static T42Instruction BSR(string n) => new T42Instruction(OPCODE.BSR, n);
        public static T42Instruction RVALINT(int n) => new T42Instruction(OPCODE.RVALINT, n);
        public static T42Instruction RVALBOOL(int n) => new T42Instruction(OPCODE.RVALBOOL, n);


        public static T42Instruction EQINT => new T42Instruction(OPCODE.EQINT);
        public static T42Instruction EQBOOL => new T42Instruction(OPCODE.EQBOOL);
        public static T42Instruction ASSINT => new T42Instruction(OPCODE.ASSINT);
        public static T42Instruction ASSBOOL => new T42Instruction(OPCODE.ASSBOOL);
        public static T42Instruction LTINT => new T42Instruction(OPCODE.LTINT);
        public static T42Instruction GTINT => new T42Instruction(OPCODE.GTINT);

        public static T42Instruction NOT => new T42Instruction(OPCODE.NOT);
        public static T42Instruction NEG => new T42Instruction(OPCODE.NEG);
        public static T42Instruction ADD => new T42Instruction(OPCODE.ADD);
        public static T42Instruction SUB => new T42Instruction(OPCODE.SUB);
        public static T42Instruction MUL => new T42Instruction(OPCODE.MUL);
        public static T42Instruction DIV => new T42Instruction(OPCODE.DIV);
        public static T42Instruction POP(int n) => new T42Instruction(OPCODE.POP, n);

        public static T42Instruction LINK => new T42Instruction(OPCODE.LINK);
        public static T42Instruction UNLINK => new T42Instruction(OPCODE.UNLINK);
        public static T42Instruction RTS => new T42Instruction(OPCODE.RTS);

        public static T42Instruction END => new T42Instruction(OPCODE.END);


        public static T42Instruction WRITEINT() => new T42Instruction(OPCODE.WRITEINT);
        public static T42Instruction WRITEBOOL() => new T42Instruction(OPCODE.WRITEBOOL);





        public enum OPCODE
        {
            PUSHINT, PUSHBOOL, LVAL, RVALINT, RVALBOOL, ASSINT,
            ASSBOOL, ADD, EQBOOL, EQINT, DECL, POP, BRF, BRA, WRITEINT,
            LINK, UNLINK, RTS, BSR, END, NOT, SUB, MUL, DIV,
            LABEL,
            NEG,
            WRITEBOOL,
            LTINT,
            GTINT
        }

        public OPCODE opcode;
        public int argument;
        public string target;

        public T42Instruction(OPCODE opcode)
        {
            this.opcode = opcode;
        }

        public T42Instruction(OPCODE opcode, int argument)
        {
            this.opcode = opcode;
            this.argument = argument;
        }

        public T42Instruction(OPCODE opcode, string target)
        {
            this.opcode = opcode;
            this.target = target;
        }

        override public string ToString()
        {
            switch (opcode)
            {
                case OPCODE.LABEL:
                    return $"[{target}]";

                case OPCODE.BSR:
                case OPCODE.BRF:
                case OPCODE.BRA:
                    return opcode.ToString() + " " + target;

                case OPCODE.PUSHINT:
                case OPCODE.PUSHBOOL:

                case OPCODE.POP:

                case OPCODE.DECL:
                    return opcode.ToString() + " " + argument.ToString();

                case OPCODE.LVAL:
                case OPCODE.RVALINT:
                case OPCODE.RVALBOOL:
                    return opcode.ToString() + " " + argument.ToString() + "(FP)";


                case OPCODE.ASSINT:
                case OPCODE.ASSBOOL:
                case OPCODE.ADD:
                case OPCODE.LTINT:
                case OPCODE.GTINT:
                case OPCODE.EQINT:
                case OPCODE.WRITEINT:
                case OPCODE.WRITEBOOL:
                case OPCODE.LINK:
                case OPCODE.UNLINK:
                case OPCODE.RTS:
                case OPCODE.END:
                case OPCODE.NOT:
                case OPCODE.SUB:
                case OPCODE.MUL:
                case OPCODE.DIV:
                case OPCODE.NEG:

                    return opcode.ToString() + " ";
            }

            throw new NotImplementedException();
        }
    }
}

