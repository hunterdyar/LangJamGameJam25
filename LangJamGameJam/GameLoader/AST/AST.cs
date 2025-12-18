using System.Data.SqlTypes;
using System.Text;

namespace LangJam.Loader.AST;

public class Expr
{
}

//()
public class SExpr : Expr
{
	public KeyExpr? Key;
	public Expr[] Elements;

	public SExpr(List<Expr> expressions)
	{
		Elements = expressions.ToArray();
		Key = Elements.Length > 0 ? Elements[0] as KeyExpr : null;
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append('(');
		for (var i = 0; i < Elements.Length; i++)
		{
			var e = Elements[i];
			sb.Append(e.ToString());
			if (i < Elements.Length - 1)
			{
				sb.Append(' ');
			}
		}

		sb.Append(')');
		return sb.ToString();
	}
}

public class GroupExpr : SExpr
{
	public GroupExpr(List<Expr> expressions) : base(expressions)
	{
	}
}

//{}
public class DeclareExpr : SExpr
{
	public readonly string Identifier;
	public readonly SymbolExpr[] Arguments;

	public DeclareExpr(string identifier, List<SymbolExpr> symbols, List<Expr> expressions) : base(expressions)
	{
		Identifier = identifier;
		Arguments = symbols.ToArray();
		Elements = expressions.ToArray();
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append('{');
		for (var i = 0; i < Elements.Length; i++)
		{
			var e = Elements[i];
			sb.Append(e.ToString());
			if (i < Elements.Length - 1)
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
	public string Value;

	public IdentifierConstant(string s)
	{
		Value = s;
	}

	public override string ToString()
	{
		return ":"+Value;
	}
}

public class KeyExpr : Expr
{
	private string _value;
	public string Value => _value;

	public KeyExpr(string val)
	{
		_value = val;
	}


	public override string ToString()
	{
		return _value;
	}
}

public class SymbolExpr : Expr
{
	private string _value;
	public string Value => _value;

	public SymbolExpr(string val)
	{
		_value = val;
	}
	
	public override string ToString()
	{
		return _value;
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
