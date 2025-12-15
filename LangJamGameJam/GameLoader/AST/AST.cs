using System.Data.SqlTypes;
using System.Text;

namespace LangJam.Loader.AST;

public class Expr
{
}

//()
public class SExpr : Expr
{
	public IdentifierConstant? Identifier;
	public List<Expr> elements = new List<Expr>();

	public SExpr(List<Expr> expressions)
	{
		elements = expressions;
		Identifier = elements.Count > 0 ? elements[0] as IdentifierConstant : null;
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append('(');
		for (var i = 0; i < elements.Count; i++)
		{
			var e = elements[i];
			sb.Append(e.ToString());
			if (i < elements.Count - 1)
			{
				sb.Append(' ');
			}
		}

		sb.Append(')');
		return sb.ToString();
	}
}

//{}
public class DeclareExpr : SExpr
{
	public readonly string Identifier;
	public readonly Expr[] elements;

	public DeclareExpr(string identifier, List<Expr> expressions) : base(expressions)
	{
		Identifier = identifier;
		elements = expressions.ToArray();
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append('{');
		for (var i = 0; i < elements.Length; i++)
		{
			var e = elements[i];
			sb.Append(e.ToString());
			if (i < elements.Length - 1)
			{
				sb.Append(' ');
			}
		}

		sb.Append('}');
		return sb.ToString();
	}
}
//[]

//~, an expression explicitly coerced into a boolean.
public class BooleanExpr : Expr
{
	public Expr Expr => _expr;
	private Expr _expr;
	public BooleanExpr(Expr expr)
	{
		_expr = expr;
	}

	public override string ToString()
	{
		return "(bool " + _expr.ToString() + ")";
	}
}

public class Point : Expr
{
	public Expr X;
	public Expr Y;

	public Point(Expr x, Expr y)
	{
		X = x;
		Y = y;
	}

	public override string ToString()
	{
		return "(point " + X + " " + Y+")";
	}
}
public class NumberConstant : Expr
{
	public double Value;
	public RuntimeObject RuntimeValue => new LJNumber(Value);

	public NumberConstant(double val)
	{
		Value = val;
	}
	public override string ToString()
	{
		return Value.ToString();
	}
}

public class IdentifierConstant : Expr
{
	private string Value;

	public IdentifierConstant(string s)
	{
		Value = s;
	}

	public override string ToString()
	{
		return Value;
	}
}

public class StringConstant : Expr
{
	public string Value;
	public RuntimeObject RuntimeValue => new LJString(Value);
	public StringConstant(string topSource)
	{
		Value = topSource;
	}


	public override string ToString()
	{
		return "\"" + Value + "\"";
	}
}
