%output=Generated/Parser.cs
%namespace Syntax
%union {
  public string value;
  public List<Statement> L;
  public Declaration D;
  public List<Argument> FL;
  public Statement S;
  public Argument A;
  public Type T;
}

%token <value> ID
%token <value> NUM
%token <value> ERR

%token LET "let"
%token IN "in"
%token LAMBDA "lambda"
%token DOT "."
%token SEMI ";"
%token PLUS "+"
%token SUB "-"
%token MUL "*"
%token DIV "/"
%token NEG "!"
%token ASN "="
%token LPAR "("
%token RPAR ")"
%token LSQR "["
%token RSQR "]"
%token LCUR "{"
%token RCUR "}"
%token COMMA ","
%token OR "||"
%token AND "&&"
%token EQ "=="
%token NEQ "!="
%token LESSER "<"
%token GREATER ">"
%token LE "<="
%token GE ">="
%token IF "if"
%token ELSE "else"
%token WHILE "while"
%token RETURN "return"
%token INT "int"
%token BOOL "bool"
%token VOID "void"
%token TRUE "true"
%token FALSE "false"

%type <FL> FormalList1, FormalList2
%type <S> Stmt0,Stmt1,Stmt2,Stmt3,Stmt4,Stmt5,Expr1,Expr2,Expr3,Expr4,Expr5,Expr6,Expr7,Expr8,Expr9,Expr10,Expr11
%type <D> Decl0, Decl
%type <A> Arg 
%type <L> ExprList1,ExprList2
%type <T> Type

%%

P : Decl0 EOF                {  Program = $1; } 
  ;

Decl0 : Decl0 Decl         { $$ = new DeclarationSequence($1, $2); $$.SetLocation(@1); }
  |						   { $$ =  null; }
  ;

Decl : Type ID "(" FormalList1 ")" "{" Stmt0 "}"    {  $$ = new TypeDeclaration($1,$2,new FormalList($4),$7); $$.SetLocation(@1); }
  | "void" ID "(" FormalList1 ")" "{" Stmt0 "}"		{  $$ = new VoidDeclaration($2,new FormalList($4),$7); $$.SetLocation(@1); }
  ;

Type : "int"				{ $$ = new IntStatement(); $$.SetLocation(@1); }
  | "bool"					{ $$ = new BoolStatement(); $$.SetLocation(@1); }
  ;

FormalList1 : FormalList2   { $$ = $1; }
  |							{ $$ =new List<Argument>(); }
  ;

FormalList2 : Arg           { $$ = new List<Argument>(); $$.Add($1); }
  | FormalList2 "," Arg		{ $$ = $1; $$.Add($3); }
  ;

Arg : Type ID				{ $$ = new Argument($1, $2); $$.SetLocation(@1); }
  ;

Stmt0 : Stmt0 Stmt1			{ $$ = new SequenceStatement($1, $2); $$.SetLocation(@1); }
  |							{$$ = null;}
  ;

Stmt1 :  Stmt2				{ $$ = $1; }
 | Stmt3					{ $$ = $1; }
 | Stmt4					{ $$ = $1; }
 ;

Stmt2 : "if" "(" Expr1 ")" Stmt4			{ $$ = new IfStatement($3, $5); $$.SetLocation(@1); }
  | "if" "(" Expr1 ")" Stmt4 "else" Stmt4	{ $$ = new IfElseStatement($3, $5, $7); $$.SetLocation(@1); }
  ;

Stmt3 : "while" "(" Expr1 ")" Stmt4			{ $$ = new WhileStatement($3, $5); $$.SetLocation(@1); }
  ;

Stmt4 : "{" Stmt0 "}"		{ $$ = new BlockStatement($2); $$.SetLocation(@1); }
  | Expr1 ";"				{ $$ = new RegularStatement($1); $$.SetLocation(@1);}	
  | Type ID ";"				{ $$ = new Argument($1, $2);  $$.SetLocation(@1); }
  | "return" Stmt5			{ $$ = $2; }
  ;

Stmt5 : Expr1 ";"			{ $$ = new Return($1); $$.SetLocation(@1); }
  | ";"						{ $$ = new VoidReturn(); $$.SetLocation(@1); }
  ;

Expr1 : ID "=" Expr1		{ $$ = new AssignStatement($1, $3); $$.SetLocation(@1); }
  | Expr2					{ $$ = $1; }
  ;

Expr2 :  Expr2 "||" Expr3   { $$ = new OrStatement($1, $3);  $$.SetLocation(@1);}
  | Expr3					{ $$ = $1; }
  ;

Expr3 : Expr3 "&&" Expr4	{ $$ = new AndStatement($1, $3);  $$.SetLocation(@1);}
  | Expr4					{ $$ = $1; }
  ;

Expr4 : Expr4 "==" Expr5	{ $$ = new EqStatement($1, $3);  $$.SetLocation(@1);}
  | Expr4 "!=" Expr5		{ $$ = new NotEqStatement($1, $3);  $$.SetLocation(@1);}
  | Expr5					{ $$ = $1; }
  ;

Expr5 : Expr5 "<" Expr6		{ $$ = new LesserStatement($1, $3); $$.SetLocation(@1); }
  | Expr5 ">" Expr6			{ $$ = new GreaterStatement($1, $3); $$.SetLocation(@1); }
  | Expr5 "<=" Expr6		{ $$ = new LEqStatement($1, $3); $$.SetLocation(@1); }
  | Expr5 ">=" Expr6		{ $$ = new GEqStatement($1, $3); $$.SetLocation(@1); }
  | Expr6					{ $$ = $1; }
  ;

Expr6 : Expr6 "+" Expr7		{ $$ = new BinOperatorStatement(BinOperatorStatement.Type.ADD, $1, $3); $$.SetLocation(@1);}
  | Expr6 "-" Expr7			{ $$ = new BinOperatorStatement(BinOperatorStatement.Type.SUB, $1, $3); $$.SetLocation(@1);}
  | Expr7					{ $$ = $1; }
  ;

Expr7 : Expr7 "*" Expr8		{ $$ = new BinOperatorStatement(BinOperatorStatement.Type.MUL, $1, $3); $$.SetLocation(@1);}
  | Expr7 "/" Expr8			{ $$ = new BinOperatorStatement(BinOperatorStatement.Type.DIV, $1, $3); $$.SetLocation(@1);}
  | Expr8					{ $$ = $1; }
  ;

Expr8 : "!" Expr8			{ $$ = new NotStatement($2); $$.SetLocation(@1); }
  | "-" Expr8				{ $$ = new NegativeStatement($2); $$.SetLocation(@1); }
  | Expr9					{ $$ = $1; }
  ;

Expr9 : "int"				{ $$ = new IntStatement(); $$.SetLocation(@1);}
  | "true"					{ $$ = new TrueStatement(); $$.SetLocation(@1);}	
  | "false"					{ $$ = new FalseStatement(); $$.SetLocation(@1);}
  | NUM						{ $$ = new NumStatement($1); $$.SetLocation(@1);}
  | "(" Expr1 ")"			{ $$ = new BracketStatement($2); $$.SetLocation(@1); }
  | Expr10					{ $$ = $1; }
  ;

Expr10 : ID Expr11			{ $$ = new IdentifierStatement($1,$2); $$.SetLocation(@1); }
  ;

Expr11 : "(" ExprList1 ")"  { $$ = new ListStatement($2); $$.SetLocation(@1); }
  |							{ $$ = null; }
  ;

ExprList1 : ExprList2		{ $$ = $1; }
  |							{ $$ = new List<Statement>(); }
  ;
ExprList2 : Expr1			{ $$ = new List<Statement>(); $$.Add($1); }
  | ExprList2 "," Expr1		{ $$ = $1; $$.Add($3); }
  ;

%%

public Parser(Scanner s) : base(s) { }
public Declaration Program; 