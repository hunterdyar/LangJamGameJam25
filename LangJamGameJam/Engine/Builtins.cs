using LangJam;

public static class Builtins
{
	public static Dictionary<string, Func<RuntimeBase, RuntimeObject[], RuntimeObject?>> BuiltinFunctions =
		new Dictionary<string, Func<
			RuntimeBase, RuntimeObject[], RuntimeObject>>{
			{ "draw-grid-color", RenderFunctions.DrawGridColor },
			{ "spawn", Spawn },
			{ "get", Get},
			{ "set", Set},
		};

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

		return null;
	}

	public static RuntimeObject Get(RuntimeBase context, RuntimeObject[] args)
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
	public static RuntimeObject Spawn(RuntimeBase context, RuntimeObject[] args)
	{
		if(context.Game.Prototypes.TryGetValue(args[0].AsString(), out var val))
		{
			context.Game.SpawnEntity(val);
		}
		else
		{
			throw new Exception($"Unable to spawn entity {args[0].ToString()}");
		}
		return null;
	}
}