using HelloWorld;
using LangJam;
using LangJam.Loader.AST;

public class Interpreter
{
	//control-flow if
	private void CFIf(SExpr sexpr, RuntimeBase context)
	{
		//id ("if") is 0
		var compare = WalkExpression(sexpr.elements[1], context);
		if (compare.AsBool())
		{
			WalkStatement(sexpr.elements[2], context);
		}
		else
		{
			if (sexpr.elements.Count == 4)//if,comp,cons,alt
			{
				WalkStatement(sexpr.elements[3], context);
			}
		}
	}

	private void CFFor(SExpr sexpr, RuntimeBase context)
	{
		//id ("for") is 0
		//for index list do
		var frame = new FrameContext(context);
		if (sexpr.elements.Count == 4)
		{
			var range = WalkExpression(sexpr.elements[2], frame).AsList().Value;
			var iterName = WalkExpression(sexpr.elements[1], frame).AsString();
			foreach (var ro in range)
			{
				//todo: stacks! for loops operate on entity variables.
				frame.SetProperty(iterName, ro);
				WalkStatement(sexpr.elements[3], frame);
			}
		}else if (sexpr.elements.Count == 5)
		{
			//for index value list do
			var indexName = WalkExpression(sexpr.elements[1], frame).AsString();
			var iterName = WalkExpression(sexpr.elements[2], frame).AsString();
			var range = WalkExpression(sexpr.elements[3], frame).AsList().Value;
			for (var i = 0; i < range.Count; i++)
			{
				var ro = range[i];
				//todo: stacks! for loops operate on entity variables.
				frame.SetProperty(indexName, new LJNumber(i));
				frame.SetProperty(iterName, ro);
				WalkStatement(sexpr.elements[4], frame);
			}
		}
	}

	public void WalkStatement(Expr expr, RuntimeBase context)
	{
		switch (expr)
		{
			case DeclareExpr declareExpr:
				//still calling 'root functions' on init, but we ... shouldnt?
				break;
			case GroupExpr groupExpr:
				foreach (var item in groupExpr.elements)
				{
					WalkStatement(item, context);
				}
				break;
			case SExpr sexpr:
				var id = sexpr.Key.Value.ToString();
				switch (id)
				{
					case "if":
						CFIf(sexpr, context);
						return;
					case "for":
						CFFor(sexpr, context);
						return;
				}
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
				else
				{
					throw new Exception($"Unknown call to '{id} for {context}");
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
			case SymbolExpr symbolConstant:
				return new LJString(symbolConstant.Value);//todo: make an LJSymbol runtime type.
			case GroupExpr groupExpr:
				throw new NotImplementedException("group values constants not supported");
			case IdentifierConstant identifier:
				if (context.TryGetProperty(identifier.Value.ToString(), out var value))
				{
					return value;
				}
				else
				{
					throw new Exception($"Unable to get variable {identifier} on {context}");
				}
			case BooleanExpr booleanExpr:
				var o = WalkExpression(booleanExpr.Expr, context);
				//if it's a list, false for empty true for contains
				//number is non-zero
				//identifiers are true/false/error
				//string is type-error.
				break;
			case KeyExpr keyExpr:
				return new LJKey(keyExpr.Value);
			case SExpr sexpr:
				if (Builtins.BuiltinFunctions.TryGetValue(sexpr.Key.ToString(), out var call))
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
					throw new Exception($"unknown call to {sexpr.Key.ToString()}");
				}
				break;
		}

		throw new NotImplementedException();
	}
}