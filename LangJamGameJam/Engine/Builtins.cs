using LangJam;

public static class Builtins
{
	public static Dictionary<string, Func<RuntimeBase, RuntimeObject[], RuntimeObject?>> BuiltinFunctions =
		new Dictionary<string, Func<
			RuntimeBase, RuntimeObject[], RuntimeObject?>>{
			//rendering
			{ "draw-grid-color", RenderFunctions.DrawGridColor },
			{ "draw-grid-sprite", RenderFunctions.DrawGridSprite},
			
			//core
			{ "spawn", Spawn },
			{ "get", Get},
			{ "set", Set},
			{"register-input-event", RegisterInputEvent},
			{"unregister-input-event", UnregisterInputEvent },

			//grid
			{ "get-grid", GridFunctions.GetGrid},
			{ "set-grid", GridFunctions.SetGrid},
			
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
		
		//wait.... i need... hmm. fuck. i need to either have everything be an expression
		//or i need to be smarter about both the event system and the property system.
		//which involves thinking. taking a break...
		if (!context.Properties.TryAdd(key, val))
		{
			context.Properties[key] = val;
		}

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
	public static RuntimeObject? Spawn(RuntimeBase context, RuntimeObject[] args)
	{
		if(context.Game.Prototypes.TryGetValue(args[0].AsString(), out var val))
		{
			var e = context.Game.SpawnEntity(val);
			return new LJEntityReference(e);
		}
		else
		{
			throw new Exception($"Unable to spawn entity {args[0].ToString()}");
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