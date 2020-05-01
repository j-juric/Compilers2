// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, John Gough, QUT 2005-2014
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.2
// Machine:  DESKTOP-BURIB4I
// DateTime: 4/30/2020 8:19:22 PM
// UserName: jovan
// Input file <Parser.y - 4/30/2020 8:19:15 PM>

// options: lines gplex

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Text;
using QUT.Gppg;

namespace Syntax
{
public enum Tokens {error=2,EOF=3,ID=4,NUM=5,ERR=6,
    BOOLVAL=7,LET=8,IN=9,LAMBDA=10,DOT=11,SEMI=12,
    PLUS=13,SUB=14,MUL=15,DIV=16,NEG=17,ASN=18,
    LPAR=19,RPAR=20,LSQR=21,RSQR=22,LCUR=23,RCUR=24,
    COMMA=25,OR=26,AND=27,EQ=28,NEQ=29,LESSER=30,
    GREATER=31,LE=32,GE=33,IF=34,ELSE=35,WHILE=36,
    RETURN=37,INT=38,BOOL=39,VOID=40,TRUE=41,FALSE=42};

public struct ValueType
#line 3 "Parser.y"
       {
  public string value;
  public List<Statement> L;
  public Declaration D;
  public List<Argument> FL;
  public Statement S;
  public Argument A;
  public Type T;
}
#line default
// Abstract base class for GPLEX scanners
[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
public abstract class ScanBase : AbstractScanner<ValueType,LexLocation> {
  private LexLocation __yylloc = new LexLocation();
  public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
  protected virtual bool yywrap() { return true; }
}

// Utility class for encapsulating token information
[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
public class ScanObj {
  public int token;
  public ValueType yylval;
  public LexLocation yylloc;
  public ScanObj( int t, ValueType val, LexLocation loc ) {
    this.token = t; this.yylval = val; this.yylloc = loc;
  }
}

[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
public class Parser: ShiftReduceParser<ValueType, LexLocation>
{
#pragma warning disable 649
  private static Dictionary<int, string> aliases;
#pragma warning restore 649
  private static Rule[] rules = new Rule[63];
  private static State[] states = new State[115];
  private static string[] nonTerms = new string[] {
      "FormalList1", "FormalList2", "Stmt0", "Stmt1", "Stmt2", "Stmt3", "Stmt4", 
      "Stmt5", "Expr1", "Expr2", "Expr3", "Expr4", "Expr5", "Expr6", "Expr7", 
      "Expr8", "Expr9", "Expr10", "Expr11", "Decl0", "Decl", "Arg", "ExprList1", 
      "ExprList2", "Type", "P", "$accept", };

  static Parser() {
    states[0] = new State(-4,new int[]{-26,1,-20,3});
    states[1] = new State(new int[]{3,2});
    states[2] = new State(-1);
    states[3] = new State(new int[]{3,4,38,105,39,93,40,107},new int[]{-21,5,-25,6});
    states[4] = new State(-2);
    states[5] = new State(-3);
    states[6] = new State(new int[]{4,7});
    states[7] = new State(new int[]{19,8});
    states[8] = new State(new int[]{38,105,39,93,20,-10},new int[]{-1,9,-2,100,-22,106,-25,103});
    states[9] = new State(new int[]{20,10});
    states[10] = new State(new int[]{23,11});
    states[11] = new State(-15,new int[]{-3,12});
    states[12] = new State(new int[]{24,13,34,16,36,27,23,23,4,34,17,50,14,52,38,89,7,56,5,57,19,58,39,93,37,94},new int[]{-4,14,-5,15,-6,26,-7,99,-9,32,-10,37,-11,61,-12,62,-13,80,-14,87,-15,83,-16,84,-17,54,-18,71,-25,90});
    states[13] = new State(-5);
    states[14] = new State(-14);
    states[15] = new State(-16);
    states[16] = new State(new int[]{19,17});
    states[17] = new State(new int[]{4,34,17,50,14,52,38,55,7,56,5,57,19,58},new int[]{-9,18,-10,37,-11,61,-12,62,-13,80,-14,87,-15,83,-16,84,-17,54,-18,71});
    states[18] = new State(new int[]{20,19});
    states[19] = new State(new int[]{23,23,4,34,17,50,14,52,38,89,7,56,5,57,19,58,39,93,37,94},new int[]{-7,20,-9,32,-10,37,-11,61,-12,62,-13,80,-14,87,-15,83,-16,84,-17,54,-18,71,-25,90});
    states[20] = new State(new int[]{35,21,24,-19,34,-19,36,-19,23,-19,4,-19,17,-19,14,-19,38,-19,7,-19,5,-19,19,-19,39,-19,37,-19});
    states[21] = new State(new int[]{23,23,4,34,17,50,14,52,38,89,7,56,5,57,19,58,39,93,37,94},new int[]{-7,22,-9,32,-10,37,-11,61,-12,62,-13,80,-14,87,-15,83,-16,84,-17,54,-18,71,-25,90});
    states[22] = new State(-20);
    states[23] = new State(-15,new int[]{-3,24});
    states[24] = new State(new int[]{24,25,34,16,36,27,23,23,4,34,17,50,14,52,38,89,7,56,5,57,19,58,39,93,37,94},new int[]{-4,14,-5,15,-6,26,-7,99,-9,32,-10,37,-11,61,-12,62,-13,80,-14,87,-15,83,-16,84,-17,54,-18,71,-25,90});
    states[25] = new State(-22);
    states[26] = new State(-17);
    states[27] = new State(new int[]{19,28});
    states[28] = new State(new int[]{4,34,17,50,14,52,38,55,7,56,5,57,19,58},new int[]{-9,29,-10,37,-11,61,-12,62,-13,80,-14,87,-15,83,-16,84,-17,54,-18,71});
    states[29] = new State(new int[]{20,30});
    states[30] = new State(new int[]{23,23,4,34,17,50,14,52,38,89,7,56,5,57,19,58,39,93,37,94},new int[]{-7,31,-9,32,-10,37,-11,61,-12,62,-13,80,-14,87,-15,83,-16,84,-17,54,-18,71,-25,90});
    states[31] = new State(-21);
    states[32] = new State(new int[]{12,33});
    states[33] = new State(-23);
    states[34] = new State(new int[]{18,35,19,74,15,-58,16,-58,13,-58,14,-58,30,-58,31,-58,32,-58,33,-58,28,-58,29,-58,27,-58,26,-58,12,-58,20,-58,25,-58},new int[]{-19,73});
    states[35] = new State(new int[]{4,34,17,50,14,52,38,55,7,56,5,57,19,58},new int[]{-9,36,-10,37,-11,61,-12,62,-13,80,-14,87,-15,83,-16,84,-17,54,-18,71});
    states[36] = new State(-28);
    states[37] = new State(new int[]{26,38,12,-29,20,-29,25,-29});
    states[38] = new State(new int[]{17,50,14,52,38,55,7,56,5,57,19,58,4,72},new int[]{-11,39,-12,62,-13,80,-14,87,-15,83,-16,84,-17,54,-18,71});
    states[39] = new State(new int[]{27,40,26,-30,12,-30,20,-30,25,-30});
    states[40] = new State(new int[]{17,50,14,52,38,55,7,56,5,57,19,58,4,72},new int[]{-12,41,-13,80,-14,87,-15,83,-16,84,-17,54,-18,71});
    states[41] = new State(new int[]{28,42,29,63,27,-32,26,-32,12,-32,20,-32,25,-32});
    states[42] = new State(new int[]{17,50,14,52,38,55,7,56,5,57,19,58,4,72},new int[]{-13,43,-14,87,-15,83,-16,84,-17,54,-18,71});
    states[43] = new State(new int[]{30,44,31,65,32,81,33,85,28,-34,29,-34,27,-34,26,-34,12,-34,20,-34,25,-34});
    states[44] = new State(new int[]{17,50,14,52,38,55,7,56,5,57,19,58,4,72},new int[]{-14,45,-15,83,-16,84,-17,54,-18,71});
    states[45] = new State(new int[]{13,46,14,67,30,-37,31,-37,32,-37,33,-37,28,-37,29,-37,27,-37,26,-37,12,-37,20,-37,25,-37});
    states[46] = new State(new int[]{17,50,14,52,38,55,7,56,5,57,19,58,4,72},new int[]{-15,47,-16,84,-17,54,-18,71});
    states[47] = new State(new int[]{15,48,16,69,13,-42,14,-42,30,-42,31,-42,32,-42,33,-42,28,-42,29,-42,27,-42,26,-42,12,-42,20,-42,25,-42});
    states[48] = new State(new int[]{17,50,14,52,38,55,7,56,5,57,19,58,4,72},new int[]{-16,49,-17,54,-18,71});
    states[49] = new State(-45);
    states[50] = new State(new int[]{17,50,14,52,38,55,7,56,5,57,19,58,4,72},new int[]{-16,51,-17,54,-18,71});
    states[51] = new State(-48);
    states[52] = new State(new int[]{17,50,14,52,38,55,7,56,5,57,19,58,4,72},new int[]{-16,53,-17,54,-18,71});
    states[53] = new State(-49);
    states[54] = new State(-50);
    states[55] = new State(-51);
    states[56] = new State(-52);
    states[57] = new State(-53);
    states[58] = new State(new int[]{4,34,17,50,14,52,38,55,7,56,5,57,19,58},new int[]{-9,59,-10,37,-11,61,-12,62,-13,80,-14,87,-15,83,-16,84,-17,54,-18,71});
    states[59] = new State(new int[]{20,60});
    states[60] = new State(-54);
    states[61] = new State(new int[]{27,40,26,-31,12,-31,20,-31,25,-31});
    states[62] = new State(new int[]{28,42,29,63,27,-33,26,-33,12,-33,20,-33,25,-33});
    states[63] = new State(new int[]{17,50,14,52,38,55,7,56,5,57,19,58,4,72},new int[]{-13,64,-14,87,-15,83,-16,84,-17,54,-18,71});
    states[64] = new State(new int[]{30,44,31,65,32,81,33,85,28,-35,29,-35,27,-35,26,-35,12,-35,20,-35,25,-35});
    states[65] = new State(new int[]{17,50,14,52,38,55,7,56,5,57,19,58,4,72},new int[]{-14,66,-15,83,-16,84,-17,54,-18,71});
    states[66] = new State(new int[]{13,46,14,67,30,-38,31,-38,32,-38,33,-38,28,-38,29,-38,27,-38,26,-38,12,-38,20,-38,25,-38});
    states[67] = new State(new int[]{17,50,14,52,38,55,7,56,5,57,19,58,4,72},new int[]{-15,68,-16,84,-17,54,-18,71});
    states[68] = new State(new int[]{15,48,16,69,13,-43,14,-43,30,-43,31,-43,32,-43,33,-43,28,-43,29,-43,27,-43,26,-43,12,-43,20,-43,25,-43});
    states[69] = new State(new int[]{17,50,14,52,38,55,7,56,5,57,19,58,4,72},new int[]{-16,70,-17,54,-18,71});
    states[70] = new State(-46);
    states[71] = new State(-55);
    states[72] = new State(new int[]{19,74,15,-58,16,-58,13,-58,14,-58,30,-58,31,-58,32,-58,33,-58,28,-58,29,-58,27,-58,26,-58,12,-58,20,-58,25,-58},new int[]{-19,73});
    states[73] = new State(-56);
    states[74] = new State(new int[]{4,34,17,50,14,52,38,55,7,56,5,57,19,58,20,-60},new int[]{-23,75,-24,77,-9,88,-10,37,-11,61,-12,62,-13,80,-14,87,-15,83,-16,84,-17,54,-18,71});
    states[75] = new State(new int[]{20,76});
    states[76] = new State(-57);
    states[77] = new State(new int[]{25,78,20,-59});
    states[78] = new State(new int[]{4,34,17,50,14,52,38,55,7,56,5,57,19,58},new int[]{-9,79,-10,37,-11,61,-12,62,-13,80,-14,87,-15,83,-16,84,-17,54,-18,71});
    states[79] = new State(-62);
    states[80] = new State(new int[]{30,44,31,65,32,81,33,85,28,-36,29,-36,27,-36,26,-36,12,-36,20,-36,25,-36});
    states[81] = new State(new int[]{17,50,14,52,38,55,7,56,5,57,19,58,4,72},new int[]{-14,82,-15,83,-16,84,-17,54,-18,71});
    states[82] = new State(new int[]{13,46,14,67,30,-39,31,-39,32,-39,33,-39,28,-39,29,-39,27,-39,26,-39,12,-39,20,-39,25,-39});
    states[83] = new State(new int[]{15,48,16,69,13,-44,14,-44,30,-44,31,-44,32,-44,33,-44,28,-44,29,-44,27,-44,26,-44,12,-44,20,-44,25,-44});
    states[84] = new State(-47);
    states[85] = new State(new int[]{17,50,14,52,38,55,7,56,5,57,19,58,4,72},new int[]{-14,86,-15,83,-16,84,-17,54,-18,71});
    states[86] = new State(new int[]{13,46,14,67,30,-40,31,-40,32,-40,33,-40,28,-40,29,-40,27,-40,26,-40,12,-40,20,-40,25,-40});
    states[87] = new State(new int[]{13,46,14,67,30,-41,31,-41,32,-41,33,-41,28,-41,29,-41,27,-41,26,-41,12,-41,20,-41,25,-41});
    states[88] = new State(-61);
    states[89] = new State(new int[]{15,-51,16,-51,13,-51,14,-51,30,-51,31,-51,32,-51,33,-51,28,-51,29,-51,27,-51,26,-51,12,-51,4,-7});
    states[90] = new State(new int[]{4,91});
    states[91] = new State(new int[]{12,92});
    states[92] = new State(-24);
    states[93] = new State(-8);
    states[94] = new State(new int[]{4,34,17,50,14,52,38,55,7,56,5,57,19,58,12,98},new int[]{-8,95,-9,96,-10,37,-11,61,-12,62,-13,80,-14,87,-15,83,-16,84,-17,54,-18,71});
    states[95] = new State(-25);
    states[96] = new State(new int[]{12,97});
    states[97] = new State(-26);
    states[98] = new State(-27);
    states[99] = new State(-18);
    states[100] = new State(new int[]{25,101,20,-9});
    states[101] = new State(new int[]{38,105,39,93},new int[]{-22,102,-25,103});
    states[102] = new State(-12);
    states[103] = new State(new int[]{4,104});
    states[104] = new State(-13);
    states[105] = new State(-7);
    states[106] = new State(-11);
    states[107] = new State(new int[]{4,108});
    states[108] = new State(new int[]{19,109});
    states[109] = new State(new int[]{38,105,39,93,20,-10},new int[]{-1,110,-2,100,-22,106,-25,103});
    states[110] = new State(new int[]{20,111});
    states[111] = new State(new int[]{23,112});
    states[112] = new State(-15,new int[]{-3,113});
    states[113] = new State(new int[]{24,114,34,16,36,27,23,23,4,34,17,50,14,52,38,89,7,56,5,57,19,58,39,93,37,94},new int[]{-4,14,-5,15,-6,26,-7,99,-9,32,-10,37,-11,61,-12,62,-13,80,-14,87,-15,83,-16,84,-17,54,-18,71,-25,90});
    states[114] = new State(-6);

    for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

    rules[1] = new Rule(-27, new int[]{-26,3});
    rules[2] = new Rule(-26, new int[]{-20,3});
    rules[3] = new Rule(-20, new int[]{-20,-21});
    rules[4] = new Rule(-20, new int[]{});
    rules[5] = new Rule(-21, new int[]{-25,4,19,-1,20,23,-3,24});
    rules[6] = new Rule(-21, new int[]{40,4,19,-1,20,23,-3,24});
    rules[7] = new Rule(-25, new int[]{38});
    rules[8] = new Rule(-25, new int[]{39});
    rules[9] = new Rule(-1, new int[]{-2});
    rules[10] = new Rule(-1, new int[]{});
    rules[11] = new Rule(-2, new int[]{-22});
    rules[12] = new Rule(-2, new int[]{-2,25,-22});
    rules[13] = new Rule(-22, new int[]{-25,4});
    rules[14] = new Rule(-3, new int[]{-3,-4});
    rules[15] = new Rule(-3, new int[]{});
    rules[16] = new Rule(-4, new int[]{-5});
    rules[17] = new Rule(-4, new int[]{-6});
    rules[18] = new Rule(-4, new int[]{-7});
    rules[19] = new Rule(-5, new int[]{34,19,-9,20,-7});
    rules[20] = new Rule(-5, new int[]{34,19,-9,20,-7,35,-7});
    rules[21] = new Rule(-6, new int[]{36,19,-9,20,-7});
    rules[22] = new Rule(-7, new int[]{23,-3,24});
    rules[23] = new Rule(-7, new int[]{-9,12});
    rules[24] = new Rule(-7, new int[]{-25,4,12});
    rules[25] = new Rule(-7, new int[]{37,-8});
    rules[26] = new Rule(-8, new int[]{-9,12});
    rules[27] = new Rule(-8, new int[]{12});
    rules[28] = new Rule(-9, new int[]{4,18,-9});
    rules[29] = new Rule(-9, new int[]{-10});
    rules[30] = new Rule(-10, new int[]{-10,26,-11});
    rules[31] = new Rule(-10, new int[]{-11});
    rules[32] = new Rule(-11, new int[]{-11,27,-12});
    rules[33] = new Rule(-11, new int[]{-12});
    rules[34] = new Rule(-12, new int[]{-12,28,-13});
    rules[35] = new Rule(-12, new int[]{-12,29,-13});
    rules[36] = new Rule(-12, new int[]{-13});
    rules[37] = new Rule(-13, new int[]{-13,30,-14});
    rules[38] = new Rule(-13, new int[]{-13,31,-14});
    rules[39] = new Rule(-13, new int[]{-13,32,-14});
    rules[40] = new Rule(-13, new int[]{-13,33,-14});
    rules[41] = new Rule(-13, new int[]{-14});
    rules[42] = new Rule(-14, new int[]{-14,13,-15});
    rules[43] = new Rule(-14, new int[]{-14,14,-15});
    rules[44] = new Rule(-14, new int[]{-15});
    rules[45] = new Rule(-15, new int[]{-15,15,-16});
    rules[46] = new Rule(-15, new int[]{-15,16,-16});
    rules[47] = new Rule(-15, new int[]{-16});
    rules[48] = new Rule(-16, new int[]{17,-16});
    rules[49] = new Rule(-16, new int[]{14,-16});
    rules[50] = new Rule(-16, new int[]{-17});
    rules[51] = new Rule(-17, new int[]{38});
    rules[52] = new Rule(-17, new int[]{7});
    rules[53] = new Rule(-17, new int[]{5});
    rules[54] = new Rule(-17, new int[]{19,-9,20});
    rules[55] = new Rule(-17, new int[]{-18});
    rules[56] = new Rule(-18, new int[]{4,-19});
    rules[57] = new Rule(-19, new int[]{19,-23,20});
    rules[58] = new Rule(-19, new int[]{});
    rules[59] = new Rule(-23, new int[]{-24});
    rules[60] = new Rule(-23, new int[]{});
    rules[61] = new Rule(-24, new int[]{-9});
    rules[62] = new Rule(-24, new int[]{-24,25,-9});

    aliases = new Dictionary<int, string>();
    aliases.Add(8, "let");
    aliases.Add(9, "in");
    aliases.Add(10, "lambda");
    aliases.Add(11, ".");
    aliases.Add(12, ";");
    aliases.Add(13, "+");
    aliases.Add(14, "-");
    aliases.Add(15, "*");
    aliases.Add(16, "/");
    aliases.Add(17, "!");
    aliases.Add(18, "=");
    aliases.Add(19, "(");
    aliases.Add(20, ")");
    aliases.Add(21, "[");
    aliases.Add(22, "]");
    aliases.Add(23, "{");
    aliases.Add(24, "}");
    aliases.Add(25, ",");
    aliases.Add(26, "||");
    aliases.Add(27, "&&");
    aliases.Add(28, "==");
    aliases.Add(29, "!=");
    aliases.Add(30, "<");
    aliases.Add(31, ">");
    aliases.Add(32, "<=");
    aliases.Add(33, ">=");
    aliases.Add(34, "if");
    aliases.Add(35, "else");
    aliases.Add(36, "while");
    aliases.Add(37, "return");
    aliases.Add(38, "int");
    aliases.Add(39, "bool");
    aliases.Add(40, "void");
    aliases.Add(41, "true");
    aliases.Add(42, "false");
  }

  protected override void Initialize() {
    this.InitSpecialTokens((int)Tokens.error, (int)Tokens.EOF);
    this.InitStates(states);
    this.InitRules(rules);
    this.InitNonTerminals(nonTerms);
  }

  protected override void DoAction(int action)
  {
#pragma warning disable 162, 1522
    switch (action)
    {
      case 2: // P -> Decl0, EOF
#line 63 "Parser.y"
                             {  Program = ValueStack[ValueStack.Depth-2].D; }
#line default
        break;
      case 3: // Decl0 -> Decl0, Decl
#line 66 "Parser.y"
                           { CurrentSemanticValue.D = new DeclarationSequence(ValueStack[ValueStack.Depth-2].D, ValueStack[ValueStack.Depth-1].D); CurrentSemanticValue.D.SetLocation(LocationStack[LocationStack.Depth-2]); }
#line default
        break;
      case 4: // Decl0 -> /* empty */
#line 67 "Parser.y"
            { CurrentSemanticValue.D =  null; }
#line default
        break;
      case 5: // Decl -> Type, ID, "(", FormalList1, ")", "{", Stmt0, "}"
#line 70 "Parser.y"
                                                    {  CurrentSemanticValue.D = new TypeDeclaration(ValueStack[ValueStack.Depth-8].T,ValueStack[ValueStack.Depth-7].value,new FormalList(ValueStack[ValueStack.Depth-5].FL),ValueStack[ValueStack.Depth-2].S); CurrentSemanticValue.D.SetLocation(LocationStack[LocationStack.Depth-8]); }
#line default
        break;
      case 6: // Decl -> "void", ID, "(", FormalList1, ")", "{", Stmt0, "}"
#line 71 "Parser.y"
                                                 {  CurrentSemanticValue.D = new VoidDeclaration(ValueStack[ValueStack.Depth-7].value,new FormalList(ValueStack[ValueStack.Depth-5].FL),ValueStack[ValueStack.Depth-2].S); CurrentSemanticValue.D.SetLocation(LocationStack[LocationStack.Depth-8]); }
#line default
        break;
      case 7: // Type -> "int"
#line 74 "Parser.y"
                { CurrentSemanticValue.T = new IntType(); CurrentSemanticValue.T.SetLocation(LocationStack[LocationStack.Depth-1]); }
#line default
        break;
      case 8: // Type -> "bool"
#line 75 "Parser.y"
               { CurrentSemanticValue.T = new BoolType(); CurrentSemanticValue.T.SetLocation(LocationStack[LocationStack.Depth-1]); }
#line default
        break;
      case 9: // FormalList1 -> FormalList2
#line 78 "Parser.y"
                            { CurrentSemanticValue.FL = ValueStack[ValueStack.Depth-1].FL; }
#line default
        break;
      case 10: // FormalList1 -> /* empty */
#line 79 "Parser.y"
          { CurrentSemanticValue.FL =new List<Argument>(); }
#line default
        break;
      case 11: // FormalList2 -> Arg
#line 82 "Parser.y"
                            { CurrentSemanticValue.FL = new List<Argument>(); CurrentSemanticValue.FL.Add(ValueStack[ValueStack.Depth-1].A); }
#line default
        break;
      case 12: // FormalList2 -> FormalList2, ",", Arg
#line 83 "Parser.y"
                         { CurrentSemanticValue.FL = ValueStack[ValueStack.Depth-3].FL; CurrentSemanticValue.FL.Add(ValueStack[ValueStack.Depth-1].A); }
#line default
        break;
      case 13: // Arg -> Type, ID
#line 86 "Parser.y"
                 { CurrentSemanticValue.A = new Argument(ValueStack[ValueStack.Depth-2].T, ValueStack[ValueStack.Depth-1].value); CurrentSemanticValue.A.SetLocation(LocationStack[LocationStack.Depth-2]); }
#line default
        break;
      case 14: // Stmt0 -> Stmt0, Stmt1
#line 89 "Parser.y"
                      { CurrentSemanticValue.S = new SequenceStatement(ValueStack[ValueStack.Depth-2].S, ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-2]); }
#line default
        break;
      case 15: // Stmt0 -> /* empty */
#line 90 "Parser.y"
          {CurrentSemanticValue.S = null;}
#line default
        break;
      case 16: // Stmt1 -> Stmt2
#line 93 "Parser.y"
                  { CurrentSemanticValue.S = ValueStack[ValueStack.Depth-1].S; }
#line default
        break;
      case 17: // Stmt1 -> Stmt3
#line 94 "Parser.y"
             { CurrentSemanticValue.S = ValueStack[ValueStack.Depth-1].S; }
#line default
        break;
      case 18: // Stmt1 -> Stmt4
#line 95 "Parser.y"
             { CurrentSemanticValue.S = ValueStack[ValueStack.Depth-1].S; }
#line default
        break;
      case 19: // Stmt2 -> "if", "(", Expr1, ")", Stmt4
#line 98 "Parser.y"
                                   { CurrentSemanticValue.S = new IfStatement(ValueStack[ValueStack.Depth-3].S, ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-5]); }
#line default
        break;
      case 20: // Stmt2 -> "if", "(", Expr1, ")", Stmt4, "else", Stmt4
#line 99 "Parser.y"
                                          { CurrentSemanticValue.S = new IfElseStatement(ValueStack[ValueStack.Depth-5].S, ValueStack[ValueStack.Depth-3].S, ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-7]); }
#line default
        break;
      case 21: // Stmt3 -> "while", "(", Expr1, ")", Stmt4
#line 102 "Parser.y"
                                      { CurrentSemanticValue.S = new WhileStatement(ValueStack[ValueStack.Depth-3].S, ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-5]); }
#line default
        break;
      case 22: // Stmt4 -> "{", Stmt0, "}"
#line 105 "Parser.y"
                       { CurrentSemanticValue.S = new BlockStatement(ValueStack[ValueStack.Depth-2].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]); }
#line default
        break;
      case 23: // Stmt4 -> Expr1, ";"
#line 106 "Parser.y"
                 { CurrentSemanticValue.S = new RegularStatement(ValueStack[ValueStack.Depth-2].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-2]);}
#line default
        break;
      case 24: // Stmt4 -> Type, ID, ";"
#line 107 "Parser.y"
                   { CurrentSemanticValue.S = new Argument(ValueStack[ValueStack.Depth-3].T, ValueStack[ValueStack.Depth-2].value);  CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]); }
#line default
        break;
      case 25: // Stmt4 -> "return", Stmt5
#line 108 "Parser.y"
                     { CurrentSemanticValue.S = ValueStack[ValueStack.Depth-1].S; }
#line default
        break;
      case 26: // Stmt5 -> Expr1, ";"
#line 111 "Parser.y"
                    { CurrentSemanticValue.S = new Return(ValueStack[ValueStack.Depth-2].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-2]); }
#line default
        break;
      case 27: // Stmt5 -> ";"
#line 112 "Parser.y"
             { CurrentSemanticValue.S = new VoidReturn(); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-1]); }
#line default
        break;
      case 28: // Expr1 -> ID, "=", Expr1
#line 115 "Parser.y"
                      { CurrentSemanticValue.S = new AssignStatement(ValueStack[ValueStack.Depth-3].value, ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]); }
#line default
        break;
      case 29: // Expr1 -> Expr2
#line 116 "Parser.y"
              { CurrentSemanticValue.S = ValueStack[ValueStack.Depth-1].S; }
#line default
        break;
      case 30: // Expr2 -> Expr2, "||", Expr3
#line 119 "Parser.y"
                            { CurrentSemanticValue.S = new BinOperatorStatement(BinOperatorStatement.Type.OR, ValueStack[ValueStack.Depth-3].S, ValueStack[ValueStack.Depth-1].S);  CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]);}
#line default
        break;
      case 31: // Expr2 -> Expr3
#line 120 "Parser.y"
              { CurrentSemanticValue.S = ValueStack[ValueStack.Depth-1].S; }
#line default
        break;
      case 32: // Expr3 -> Expr3, "&&", Expr4
#line 123 "Parser.y"
                         { CurrentSemanticValue.S = new BinOperatorStatement(BinOperatorStatement.Type.AND, ValueStack[ValueStack.Depth-3].S, ValueStack[ValueStack.Depth-1].S);  CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]);}
#line default
        break;
      case 33: // Expr3 -> Expr4
#line 124 "Parser.y"
              { CurrentSemanticValue.S = ValueStack[ValueStack.Depth-1].S; }
#line default
        break;
      case 34: // Expr4 -> Expr4, "==", Expr5
#line 127 "Parser.y"
                         { CurrentSemanticValue.S = new BinOperatorStatement(BinOperatorStatement.Type.EQ, ValueStack[ValueStack.Depth-3].S, ValueStack[ValueStack.Depth-1].S);  CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]);}
#line default
        break;
      case 35: // Expr4 -> Expr4, "!=", Expr5
#line 128 "Parser.y"
                      { CurrentSemanticValue.S = new BinOperatorStatement(BinOperatorStatement.Type.NEQ, ValueStack[ValueStack.Depth-3].S, ValueStack[ValueStack.Depth-1].S);  CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]);}
#line default
        break;
      case 36: // Expr4 -> Expr5
#line 129 "Parser.y"
              { CurrentSemanticValue.S = ValueStack[ValueStack.Depth-1].S; }
#line default
        break;
      case 37: // Expr5 -> Expr5, "<", Expr6
#line 132 "Parser.y"
                         { CurrentSemanticValue.S = new BinOperatorStatement(BinOperatorStatement.Type.LE, ValueStack[ValueStack.Depth-3].S, ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]); }
#line default
        break;
      case 38: // Expr5 -> Expr5, ">", Expr6
#line 133 "Parser.y"
                      { CurrentSemanticValue.S = new BinOperatorStatement(BinOperatorStatement.Type.GR, ValueStack[ValueStack.Depth-3].S, ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]); }
#line default
        break;
      case 39: // Expr5 -> Expr5, "<=", Expr6
#line 134 "Parser.y"
                      { CurrentSemanticValue.S = new BinOperatorStatement(BinOperatorStatement.Type.LEQ, ValueStack[ValueStack.Depth-3].S, ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]); }
#line default
        break;
      case 40: // Expr5 -> Expr5, ">=", Expr6
#line 135 "Parser.y"
                      { CurrentSemanticValue.S = new BinOperatorStatement(BinOperatorStatement.Type.GEQ, ValueStack[ValueStack.Depth-3].S, ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]); }
#line default
        break;
      case 41: // Expr5 -> Expr6
#line 136 "Parser.y"
              { CurrentSemanticValue.S = ValueStack[ValueStack.Depth-1].S; }
#line default
        break;
      case 42: // Expr6 -> Expr6, "+", Expr7
#line 139 "Parser.y"
                         { CurrentSemanticValue.S = new BinOperatorStatement(BinOperatorStatement.Type.ADD, ValueStack[ValueStack.Depth-3].S, ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]);}
#line default
        break;
      case 43: // Expr6 -> Expr6, "-", Expr7
#line 140 "Parser.y"
                      { CurrentSemanticValue.S = new BinOperatorStatement(BinOperatorStatement.Type.SUB, ValueStack[ValueStack.Depth-3].S, ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]);}
#line default
        break;
      case 44: // Expr6 -> Expr7
#line 141 "Parser.y"
              { CurrentSemanticValue.S = ValueStack[ValueStack.Depth-1].S; }
#line default
        break;
      case 45: // Expr7 -> Expr7, "*", Expr8
#line 144 "Parser.y"
                         { CurrentSemanticValue.S = new BinOperatorStatement(BinOperatorStatement.Type.MUL, ValueStack[ValueStack.Depth-3].S, ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]);}
#line default
        break;
      case 46: // Expr7 -> Expr7, "/", Expr8
#line 145 "Parser.y"
                      { CurrentSemanticValue.S = new BinOperatorStatement(BinOperatorStatement.Type.DIV, ValueStack[ValueStack.Depth-3].S, ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]);}
#line default
        break;
      case 47: // Expr7 -> Expr8
#line 146 "Parser.y"
              { CurrentSemanticValue.S = ValueStack[ValueStack.Depth-1].S; }
#line default
        break;
      case 48: // Expr8 -> "!", Expr8
#line 149 "Parser.y"
                    { CurrentSemanticValue.S = new NotStatement(ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-2]); }
#line default
        break;
      case 49: // Expr8 -> "-", Expr8
#line 150 "Parser.y"
                 { CurrentSemanticValue.S = new NegativeStatement(ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-2]); }
#line default
        break;
      case 50: // Expr8 -> Expr9
#line 151 "Parser.y"
              { CurrentSemanticValue.S = ValueStack[ValueStack.Depth-1].S; }
#line default
        break;
      case 51: // Expr9 -> "int"
#line 154 "Parser.y"
                 { CurrentSemanticValue.S = new IntStatement(); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-1]);}
#line default
        break;
      case 52: // Expr9 -> BOOLVAL
#line 155 "Parser.y"
                { CurrentSemanticValue.S = new BoolStatement(ValueStack[ValueStack.Depth-1].value); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-1]);}
#line default
        break;
      case 53: // Expr9 -> NUM
#line 156 "Parser.y"
             { CurrentSemanticValue.S = new NumStatement(ValueStack[ValueStack.Depth-1].value); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-1]);}
#line default
        break;
      case 54: // Expr9 -> "(", Expr1, ")"
#line 157 "Parser.y"
                    { CurrentSemanticValue.S = ValueStack[ValueStack.Depth-2].S; }
#line default
        break;
      case 55: // Expr9 -> Expr10
#line 158 "Parser.y"
               { CurrentSemanticValue.S = ValueStack[ValueStack.Depth-1].S; }
#line default
        break;
      case 56: // Expr10 -> ID, Expr11
#line 161 "Parser.y"
                     { CurrentSemanticValue.S = new IdentifierStatement(ValueStack[ValueStack.Depth-2].value,ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-2]); }
#line default
        break;
      case 57: // Expr11 -> "(", ExprList1, ")"
#line 164 "Parser.y"
                            { CurrentSemanticValue.S = new ListStatement(ValueStack[ValueStack.Depth-2].L); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]); }
#line default
        break;
      case 58: // Expr11 -> /* empty */
#line 165 "Parser.y"
          { CurrentSemanticValue.S = null; }
#line default
        break;
      case 59: // ExprList1 -> ExprList2
#line 168 "Parser.y"
                       { CurrentSemanticValue.L = ValueStack[ValueStack.Depth-1].L; }
#line default
        break;
      case 60: // ExprList1 -> /* empty */
#line 169 "Parser.y"
          { CurrentSemanticValue.L = new List<Statement>(); }
#line default
        break;
      case 61: // ExprList2 -> Expr1
#line 171 "Parser.y"
                    { CurrentSemanticValue.L = new List<Statement>(); CurrentSemanticValue.L.Add(ValueStack[ValueStack.Depth-1].S); }
#line default
        break;
      case 62: // ExprList2 -> ExprList2, ",", Expr1
#line 172 "Parser.y"
                         { CurrentSemanticValue.L = ValueStack[ValueStack.Depth-3].L; CurrentSemanticValue.L.Add(ValueStack[ValueStack.Depth-1].S); }
#line default
        break;
    }
#pragma warning restore 162, 1522
  }

  protected override string TerminalToString(int terminal)
  {
    if (aliases != null && aliases.ContainsKey(terminal))
        return aliases[terminal];
    else if (((Tokens)terminal).ToString() != terminal.ToString(CultureInfo.InvariantCulture))
        return ((Tokens)terminal).ToString();
    else
        return CharToString((char)terminal);
  }

#line 177 "Parser.y"
public Parser(Scanner s) : base(s) { }
public Declaration Program; 
#line default
}
}
