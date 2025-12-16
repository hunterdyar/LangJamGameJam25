using LangJam;

public static class Builtins
{
	public static Dictionary<string, Func<RuntimeBase, RuntimeObject[], RuntimeObject?>> BuiltinFunctions =
		new Dictionary<string, Func<
			RuntimeBase, RuntimeObject[], RuntimeObject?>>{
			//rendering
			{ "draw-grid-color", RenderFunctions.DrawGridColor },
			{ "draw-grid-sprite", RenderFunctions.DrawGridSprite},
			{ "draw-grid-circle", RenderFunctions.DrawGridCircle },
			//core
			{ "spawn", Spawn },
			{ "get", Get},
			{ "get-from", GetFrom},
			{ "set", Set},
			{ "set-in", SetIn },
			{ "invoke", Invoke},
			{ "call", Call },
			{ "range", Range},
			//input
			{"register-input-event", RegisterInputEvent},
			{"unregister-input-event", UnregisterInputEvent },
			
			//component
			{ "find-comp-in-parent", FindComponentInParent },

			//math
			{ "inc", MathFunctions.Increment},
			{ "dec", MathFunctions.Decrement},
			
			//compare
			{ "gt", MathFunctions.BinComp(((a , b) => a>b))},
			{ "greater-than", MathFunctions.BinComp(((a , b) => a>b))},
			{ "gte", MathFunctions.BinComp(((a , b) => a>=b))},
			{ "greater-than-equal", MathFunctions.BinComp(((a , b) => a>=b))},
			{ "lt", MathFunctions.BinComp(((a , b) => a<b))},
			{ "less-than", MathFunctions.BinComp(((a , b) => a<b))},
			{ "lte", MathFunctions.BinComp(((a , b) => a<=b))},
			{ "less-than-equal", MathFunctions.BinComp(((a , b) => a<=b))},
			{ "eq", IsEqualTo},
			{ "equal", IsEqualTo}
		};

	private static RuntimeObject? Range(RuntimeBase context, RuntimeObject[] args)
	{
		double fromInc = 0;
		double toExc = 0;
		double inc = 1;
		if (args.Length == 3)
		{
			fromInc = args[0].AsNumber();
			toExc = args[1].AsNumber();
			inc = args[2].AsNumber();
		}else if (args.Length == 2)
		{
			fromInc = args[0].AsNumber();
			toExc = args[1].AsNumber();
		}else if (args.Length == 1)
		{
			toExc = args[1].AsNumber();
		}

		var list = new LJList();
		for (double i = fromInc; i < toExc; i+=inc)
		{
			list.Value.Add(new LJNumber(i));
		}

		return list;
	}

	private static RuntimeObject? Invoke(RuntimeBase context, RuntimeObject[] args)
	{
		var a = args[0].AsString();
		context.Scene.CallMethodRecursive(a);
		return null;
	}

	private static RuntimeObject? Call(RuntimeBase context, RuntimeObject[] args)
	{
		var cont = args[0] as LJRuntimeBaseReference;
		if (cont == null)
		{
			throw new Exception($"Unable to call on {args[0]}. expected scene or component reference");
		}
		var func = args[1].AsString();
		if(cont.Value.Scene.Methods.TryGetValue(func, out var e))
		{
			cont.Value.Scene.WalkDeclaredExpr(e);	
		}
		return null;
	}

	private static RuntimeObject? IsEqualTo(RuntimeBase context, RuntimeObject[] args)
	{
		var a = args[0];
		var b = args[1];
		return new LJBool(a == b);//i think we overloaded the equality things for this to work?
	}

	public static RuntimeObject? Set(RuntimeBase context, RuntimeObject[] args)
	{
		var key = args[0].AsString();
		var val = args[1];
		context.SetProperty(key, val);
		return val;
	}

	public static RuntimeObject? SetIn(RuntimeBase context, RuntimeObject[] args)
	{
		RuntimeBase realTarget;

		var target = args[0];
		if (target is LJKey)
		{
			realTarget = context.Scene.GetComponent(target.AsString());
		}else if (target is LJRuntimeBaseReference ljref)
		{
			realTarget = ljref.Value;
		}
		else
		{
			throw new Exception($"Cannot set-in {context}. needs component key (name) or entity/component reference");
		}
		
		
		var key = args[1].AsString();
		var val = args[2];

		//wait.... i need... hmm. fuck. i need to either have everything be an expression
		//or i need to be smarter about both the event system and the property system.
		//which involves thinking. taking a break...
		realTarget.SetProperty(key,val);
		return val;
	}

	public static RuntimeObject? Get(RuntimeBase context, RuntimeObject[] args)
	{
		var key = args[0].AsString();

		//wait.... i need... hmm. fuck. i need to either have everything be an expression
		//or i need to be smarter about both the event system and the property system.
		//which involves thinking. taking a break...
		if (context.Properties.TryGetValue(key, out var val))
		{
			return val;
		}

		throw new Exception($"cannot get value {key} from {context}");
	}

	public static RuntimeObject? GetFrom(RuntimeBase context, RuntimeObject[] args)
	{
		RuntimeBase realTarget;
		var target = args[0];
		if (target is LJKey)
		{
			realTarget = context.Scene.GetComponent(target.AsString());
		}
		else if (target is LJRuntimeBaseReference ljref)
		{
			realTarget = ljref.Value;
		}
		else
		{
			throw new Exception($"Cannot get-from {context}. needs component key (name) or entity/component reference");
		}
		var key = args[1].AsString();
		if (realTarget.Properties.TryGetValue(key, out var val))
		{
			return val;
		}

		throw new Exception($"cannot get-from value {key} from {target}, by {context}");
	}

	public static RuntimeObject? FindComponentInParent(RuntimeBase context, RuntimeObject[] args)
	{
		var key = args[0].AsString();
		var p = context.Scene;
		while (p != null)
		{
			if (p.TryGetComponent(key, out var c))
			{
				return new LJComponentReference(c);
			}

			p = p.Parent;
		}
		if (context.Properties.TryGetValue(key, out var val))
		{
			return val;
		}

		throw new Exception($"cannot get value {key} from {context}");
	}
	public static RuntimeObject? Spawn(RuntimeBase context, RuntimeObject[] args)
	{
		if(context.Game.SceneDefinitions.TryGetValue(args[0].AsString(), out var val))
		{
			var e = context.Game.SpawnScene(context.Scene, val);
			return new LJSceneReference(e);
		}
		else
		{
			throw new Exception($"" +
			                    $"Unable to spawn entity {args[0].AsString()}");
		}
		return null;
	}

	public static RuntimeObject? SetEnabled(RuntimeBase context, RuntimeObject[] args)
	{
		var shouldBeEnabled = args[1].AsBool();
		context.SetEnabled(shouldBeEnabled);
		
		return null;
	}

	public static RuntimeObject? RegisterInputEvent(RuntimeBase context, RuntimeObject[] args)
	{
		var funcName = args[0].AsString();
		var keyName = args[1].AsString();
		var keyState = args[2].AsString();
		
		//funcName to decExpr
		if (context.Methods.TryGetValue(funcName, out var declareExpr))
		{
			context.Game.InputSystem.RegisterInputEvent(context, declareExpr, keyName, keyState);
		}
		else
		{
			throw new Exception($"Unable to register input event, unknown method {funcName} on {context}");
		}

		return null;
	}

	public static RuntimeObject? UnregisterInputEvent(RuntimeBase context, RuntimeObject[] args)
	{
		var funcName = args[0].AsString();
		var keyName = args[1].AsString();
		var keyState = args[2].AsString();

		//funcName to decExpr
		if (context.Methods.TryGetValue(funcName, out var declareExpr))
		{
			context.Game.InputSystem.UnregisterInputEvent(context, declareExpr, keyName, keyState);
		}
		else
		{
			throw new Exception($"Unable to register input event, unknown method {funcName} on {context}");
		}

		return null;
	}
}