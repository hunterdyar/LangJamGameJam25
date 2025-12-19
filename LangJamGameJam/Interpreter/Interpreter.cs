using LangJam;
using LangJam.Loader.AST;

public class Interpreter
{
	private Stack<RuntimeObject> _returnValues = new Stack<RuntimeObject>();
	//control-flow if
	private IEnumerator<YieldInstruction?> CFIf(SExpr sexpr, RuntimeBase context)
	{
		//id ("if") is 0
		var compare = WalkExpression(sexpr.Elements[1], context);
		if (compare.AsBool())
		{
			var n = WalkStatement(sexpr.Elements[2], context);
			while (n != null && n.MoveNext())
			{
				yield return n.Current;
			}
		}
		else
		{
			if (sexpr.Elements.Length == 4)//if,comp,cons,alt
			{
				var n = WalkStatement(sexpr.Elements[3], context);
				while (n != null && n.MoveNext())
				{
					yield return n.Current;
				}
			}
		}
	}

	private IEnumerator<YieldInstruction?> CFFor(SExpr sexpr, RuntimeBase context)
	{
		//id ("for") is 0
		//for index list do
		var frame = new FrameContext(context);
		if (sexpr.Elements.Length == 4)
		{
			var range = WalkExpression(sexpr.Elements[2], frame).AsList().Value;
			var iterName = WalkExpression(sexpr.Elements[1], frame).AsSymbol();
			foreach (var ro in range)
			{
				//todo: stacks! for loops operate on entity variables.
				frame.SetProperty(iterName, ro, true);
				var n = WalkStatement(sexpr.Elements[3], frame);
				while (n != null && n.MoveNext())
				{
					yield return n.Current;
				}
			}
		}else if (sexpr.Elements.Length == 5)
		{
			//for index value list do
			var indexName = WalkExpression(sexpr.Elements[1], frame).AsSymbol();
			var iterName = WalkExpression(sexpr.Elements[2], frame).AsSymbol();
			var range = WalkExpression(sexpr.Elements[3], frame).AsList().Value;
			for (var i = 0; i < range.Count; i++)
			{
				var ro = range[i];
				//todo: stacks! for loops operate on entity variables.
				frame.SetProperty(indexName, new LJNumber(i), true);
				frame.SetProperty(iterName, ro, true);
				var n = WalkStatement(sexpr.Elements[4], frame);
				while (n != null && n.MoveNext())
				{
					yield return n.Current;
				}
			}
		}
	}

	public IEnumerator<YieldInstruction?> WalkStatement(Expr expr, RuntimeBase context)
	{
		IEnumerator<YieldInstruction?> n;
		switch (expr)
		{
			case NumberConstant numberConstant:
				_returnValues.Push(numberConstant.RuntimeValue);
				break;
			case StringConstant stringConstant:
				_returnValues.Push(stringConstant.RuntimeValue);
				break;
			case SymbolExpr symbolConstant:
				_returnValues.Push(new LJSymbol(symbolConstant.Value));
				break;
			case IdentifierConstant identifier:
				if (context.TryGetProperty(identifier.Value, out var value))
				{
					_returnValues.Push(value);
				}
				else
				{
					throw new Exception($"Unable to get variable {identifier} on {context}");
				}
				break;
			case BooleanExpr booleanExpr:
				var o = WalkExpression(booleanExpr.Expr, context);
				//if it's a list, false for empty true for contains
				//number is non-zero
				//identifiers are true/false/error
				//string is type-error.
				_returnValues.Push(new LJBool(o.AsBool()));
				break;
			case KeyExpr keyExpr:
				_returnValues.Push(new LJKey(keyExpr.Value));
				break;
			case DeclareExpr declareExpr:
				//these have already been declared. so, ignore!
				break;
			case GroupExpr groupExpr:
				foreach (var item in groupExpr.Elements)
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
					case "return":
						//todo
						n = WalkStatement(sexpr.Elements[1], context);
						while (n != null && n.MoveNext())
						{
							yield return n.Current;
						}
						
						//and then we push the value? that's? already been pushed?
						//and then uh. we. abort the call. this won't workkkk?
						break;
					case "start-routine":
						n = CFRoutine(sexpr, context);
						while (n != null && n.MoveNext())
						{
							yield return n.Current;
						}
						break;
					case "yield":
						//pardon me for my sins. yields must only have one instruction... and they cannot be references? shit!
						var ryiSexpr = sexpr.Elements[1] as SExpr;
						if (RoutineFunctions.Yields.TryGetValue(ryiSexpr.Key.Value, out var y))
						{
							//a call to 'yield' that returns the yieldinstruction
							var args = new RuntimeObject[ryiSexpr.Elements.Length - 1];
							for (int i = 1; i < ryiSexpr.Elements.Length; i++)
							{
								var ro = WalkExpression(ryiSexpr.Elements[i], context);
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
						var args = new RuntimeObject[sexpr.Elements.Length - 1];
						for (int i = 1; i < sexpr.Elements.Length; i++)
						{
							var ro = WalkExpression(sexpr.Elements[i], context);
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
			yield return n.Current;
		}
	}
	
	

	public void WalkDeclaredExpr(DeclareExpr expr, RuntimeBase context, RuntimeObject[] args)
	{
		if (args.Length != expr.Arguments.Length)
		{
			throw new Exception($"wrong number of arguments for function {expr.Identifier} in {context}");
		}

		if (args.Length > 0)
		{
			throw new NotImplementedException();
		}
		for (int i = 0; i < expr.Elements.Length; i++)
		{
			var n = WalkStatement(expr.Elements[i], context);
			while (n != null && n.MoveNext())
			{
				//todo:yield?
				continue;
			}
		}
	}

	//we anticipate a value to be spit out here... but all expressions are values...
	private RuntimeObject WalkExpression(Expr expr, RuntimeBase context)
	{
		switch (expr)
		{
			case SExpr sexpr:
				if (Builtins.BuiltinFunctions.TryGetValue(sexpr.Key.ToString(), out var call))
				{
					var args = new RuntimeObject[sexpr.Elements.Length - 1];
					for (int i = 1; i < sexpr.Elements.Length; i++)
					{
						var ro = WalkExpression(sexpr.Elements[i], context);
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

		//if not a sexpr...
		var n = WalkStatement(expr, context);
		while (n != null && n.MoveNext())
		{
			continue;
		}

		return _returnValues.Pop();
	}


}