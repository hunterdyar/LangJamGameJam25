using HelloWorld;
using LangJam;
using LangJam.Loader.AST;

public class Interpreter
{
	public void WalkStatement(Expr expr, RuntimeBase context)
	{
		switch (expr)
		{
			case DeclareExpr declareExpr:
			//add it to the relevant 
			throw new NotImplementedException();
				break;
			case SExpr sexpr:
				var id = sexpr.Identifier.ToString();
				if (Builtins.BuiltinFunctions.TryGetValue(id, out var call))
				{
					var args = new RuntimeObject[sexpr.elements.Count-1];
					for (int i = 1; i < sexpr.elements.Count; i++)
					{
						var ro = WalkExpression(sexpr.elements[i], context);
						args[i-1] = ro;
					}
					call?.Invoke(context, args);
				}
				//check if there are constants...
				break;
			break;
			
		}
	}

	//we anticipate a value to be spit out here... but all expressions are values...
	private RuntimeObject WalkExpression(Expr expr, RuntimeBase context)
	{
		switch (expr)
		{
			case NumberConstant numberConstant:
				return numberConstant.RuntimeValue;
			case StringConstant stringConstant:
				return stringConstant.RuntimeValue;
			case IdentifierConstant identifier:
				return new Identifier(identifier.ToString());
			case BooleanExpr booleanExpr:
				var o = WalkExpression(booleanExpr.Expr, context);
				//if it's a list, false for empty true for contains
				//number is non-zero
				//identifiers are true/false/error
				//string is type-error.
				break;
			case SExpr sexpr:
				if (Builtins.BuiltinFunctions.TryGetValue(sexpr.Identifier.ToString(), out var call))
				{
					var args = new RuntimeObject[sexpr.elements.Count - 1];
					for (int i = 1; i < sexpr.elements.Count; i++)
					{
						var ro = WalkExpression(sexpr.elements[i], context);
						args[i - 1] = ro;
					}

					var x = call?.Invoke(context, args);
					if (x != null)
					{
						return x;
					}
					else
					{
						throw new Exception("wait does this language have nulls?");
					}
				}
				else
				{
					throw new Exception($"unknown call to {sexpr.Identifier.ToString()}");
				}
				break;
		}

		throw new NotImplementedException();
	}
}