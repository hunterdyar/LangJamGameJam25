using HelloWorld;
using HelloWorld.Interpreter;
using LangJam;
using LangJam.Loader.AST;

public class Interpreter
{
	//control-flow if
	private IEnumerator<YieldInstruction> CFIf(SExpr sexpr, RuntimeBase context)
	{
		//id ("if") is 0
		var compare = WalkExpression(sexpr.elements[1], context);
		if (compare.AsBool())
		{
			var n = WalkStatement(sexpr.elements[2], context);
			while (n != null && n.MoveNext())
			{
				continue;
			}
		}
		else
		{
			if (sexpr.elements.Count == 4)//if,comp,cons,alt
			{
				var n = WalkStatement(sexpr.elements[3], context);
				while (n!=null && n.MoveNext())
				{
					continue;
				}
			}
		}

		return null;
	}

	private IEnumerator<YieldInstruction?> CFFor(SExpr sexpr, RuntimeBase context)
	{
		//id ("for") is 0
		//for index list do
		var frame = new FrameContext(context);
		if (sexpr.elements.Count == 4)
		{
			var range = WalkExpression(sexpr.elements[2], frame).AsList().Value;
			var iterName = WalkExpression(sexpr.elements[1], frame).AsSymbol();
			foreach (var ro in range)
			{
				//todo: stacks! for loops operate on entity variables.
				frame.SetProperty(iterName, ro, true);
				var n = WalkStatement(sexpr.elements[3], frame);
				while (n != null && n.MoveNext())
				{
					yield return n.Current;
				}
			}
		}else if (sexpr.elements.Count == 5)
		{
			//for index value list do
			var indexName = WalkExpression(sexpr.elements[1], frame).AsSymbol();
			var iterName = WalkExpression(sexpr.elements[2], frame).AsSymbol();
			var range = WalkExpression(sexpr.elements[3], frame).AsList().Value;
			for (var i = 0; i < range.Count; i++)
			{
				var ro = range[i];
				//todo: stacks! for loops operate on entity variables.
				frame.SetProperty(indexName, new LJNumber(i), true);
				frame.SetProperty(iterName, ro, true);
				var n = WalkStatement(sexpr.elements[4], frame);
				while (n != null && n.MoveNext())
				{
					continue;
				}
			}
		}
	}

	public IEnumerator<YieldInstruction?> WalkStatement(Expr expr, RuntimeBase context)
	{
		IEnumerator<YieldInstruction?> n;
		switch (expr)
		{
			case DeclareExpr declareExpr:
				//still calling 'root functions' on init, but we ... shouldnt?
				break;
			case GroupExpr groupExpr:
				foreach (var item in groupExpr.elements)
				{
					n = WalkStatement(item, context);
					while (n!= null && n.MoveNext())
					{
						yield return n.Current;
					}
				}
				break;
			case SExpr sexpr:
				var id = sexpr.Key.Value.ToString();
				bool handledBySwitch = true;
				switch (id)
				{
					case "if":
						n = CFIf(sexpr, context);
						while (n != null && n.MoveNext())
						{
							yield return n.Current;
						}
						break;
					case "for":
						n = CFFor(sexpr, context);
						while (n != null && n.MoveNext())
						{
							yield return n.Current;
						}
						break;
					case "while":
						// return CF(sexpr, context);
						throw new NotImplementedException();
					case "start-routine":
						n = CFRoutine(sexpr, context);
						while (n != null && n.MoveNext())
						{
							yield return n.Current;
						}
						break;
					case "yield":
						//pardon me for my sins. yields must only have one instruction... and they cannot be references? shit!
						var ryiSexpr = sexpr.elements[1] as SExpr;
						if (RoutineFunctions.Yields.TryGetValue(ryiSexpr.Key.Value, out var y))
						{
							//a call to 'yield' that returns the yieldinstruction
							var args = new RuntimeObject[ryiSexpr.elements.Count - 1];
							for (int i = 1; i < ryiSexpr.elements.Count; i++)
							{
								var ro = WalkExpression(ryiSexpr.elements[i], context);
								args[i - 1] = ro;
							}
							//create the actual YieldInstruction
							var yi = y?.Invoke(context, args);
							yield return yi;
						}
						else
						{
							throw new Exception($"Unknown yield to '{id} for {context}");
						}
						break;
					default:
						handledBySwitch = false;
						break;
				}

				if (!handledBySwitch)
				{
					if (Builtins.BuiltinFunctions.TryGetValue(id, out var call))
					{
						var args = new RuntimeObject[sexpr.elements.Count - 1];
						for (int i = 1; i < sexpr.elements.Count; i++)
						{
							var ro = WalkExpression(sexpr.elements[i], context);
							args[i - 1] = ro;
						}

						call?.Invoke(context, args);
					}
					else
					{
						throw new Exception($"Unknown call to '{id} for {context}");
					}
					//check if there are constants...
				}

				break;
			break;
		}
	}

	private IEnumerator<YieldInstruction> CFRoutine(SExpr sexpr, RuntimeBase context, Routine? routineContext = null)
	{
		var routine = new Routine(sexpr, context);
		if (routineContext != null)
		{
			yield return new YieldForRoutine(routine);
			Console.WriteLine("i hope this works");
		}
		var n = context.Game.RoutineSystem.StartRoutine(routine);
		while (n != null && n.MoveNext())
		{
			continue;
		}
	}

	//we anticipate a value to be spit out here... but all expressions are values...
	private RuntimeObject WalkExpression(Expr expr, RuntimeBase context, Routine? routineContext = null)
	{
		switch (expr)
		{
			case NumberConstant numberConstant:
				return numberConstant.RuntimeValue;
			case StringConstant stringConstant:
				return stringConstant.RuntimeValue;
			case SymbolExpr symbolConstant:
				return new LJSymbol(symbolConstant.Value);
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
				return new LJBool(o.AsBool());
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