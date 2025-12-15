using System.Data.SqlTypes;

namespace LangJam.Loader.AST;

public class Expr
{
}

//()
public class SExpr : Expr
{
	public Identifier? Identifier => elements.Count > 0 ? elements[0] as Identifier : null;
	public List<Expr> elements = new List<Expr>();

	public SExpr(List<Expr> expressions)
	{
		elements = expressions;
	}
}

//{}
public class DeclareExpr : SExpr
{
	public DeclareExpr(List<Expr> expressions) : base(expressions)
	{
	}
}

//[]

//~ or true or false
public class BooleanExpr : Expr
{
	
}

public class Point : Expr
{
	public Expr X;
	public Expr Y;
}
public class NumberConstant : Expr
{
	public double Value;
}

public class Identifier : Expr
{
	private string Value;
}

public class StringConstant : Expr
{
	public string Value;
}