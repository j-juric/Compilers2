%option out:Generated/Lexer.cs
%namespace Syntax
%using QUT.Gppg;

alpha   [a-zA-Z_]
digit   [0-9]
alphanum    {alpha}|{digit}
%%

" "|\r|\n       { }
//.*$           { }

"."             { return (int) Tokens.DOT; }
";"             { return (int) Tokens.SEMI; }
"+"             { return (int) Tokens.PLUS; }
"-"             { return (int) Tokens.SUB; }
"*"             { return (int) Tokens.MUL; }
"/"             { return (int) Tokens.DIV; }
"="             { return (int) Tokens.ASN; }
"=="            { return (int) Tokens.EQ; }
"!="            { return (int) Tokens.NEQ; }
"!"            { return (int) Tokens.NEG; }
">"             { return (int) Tokens.GREATER; }
"<"             { return (int) Tokens.LESSER; }
">="            { return (int) Tokens.GE; }
"<="            { return (int) Tokens.LE; }
"||"            { return (int) Tokens.OR; }
"&&"            { return (int) Tokens.AND; }
"("             { return (int) Tokens.LPAR; }
")"             { return (int) Tokens.RPAR; }
"["             { return (int) Tokens.LSQR; }
"]"             { return (int) Tokens.RSQR; }
"{"             { return (int) Tokens.LCUR; }
"}"             { return (int) Tokens.RCUR; }
","             { return (int) Tokens.COMMA; }
"if"			{ return (int) Tokens.IF; }
"else"			{ return (int) Tokens.ELSE; }
"while"			{ return (int) Tokens.WHILE; }
"return"		{ return (int) Tokens.RETURN; }
"void"			{ return (int) Tokens.VOID; }
"int"			{ return (int) Tokens.INT; }
"bool"			{ return (int) Tokens.BOOL; }


"true"	{ yylval.value = yytext; return (int) Tokens.BOOLVAL;}
"false" { yylval.value = yytext; return (int) Tokens.BOOLVAL;}
{alpha}{alphanum}*      { yylval.value = yytext;
                          return (int) Tokens.ID; 
                        }
                        
{digit}+                { yylval.value = yytext;
                          return (int) Tokens.NUM; 
                        }
.                       { yylval.value = yytext;
                          return (int) Tokens.ERR;
                        }
                        
%{
      yylloc = new LexLocation(tokLin, tokCol, tokELin, tokECol);
%}

%% 

override public void yyerror(string msg, object[] arg) {
  Console.WriteLine("{0} on line {1} column {2}", msg, yylloc.StartLine, yylloc.StartColumn);
}