using LangJam;

public static class Builtins
{
	public static Dictionary<string, Action<RuntimeBase, RuntimeObject[]>> BuiltinFunctions =
		new Dictionary<string, Action<
			RuntimeBase, RuntimeObject[]>>
		{
			{ "draw-grid-color", RenderFunctions.DrawGridColor },
			{ "spawn", Spawn }
		};

	public static void Set(RuntimeBase context, RuntimeObject[] args)
	{
		var key = args[0].AsString();
		var val = args[1];
		
		//wait.... i need... hmm. fuck. i need to either have everything be an expression
		//or i need to be smarter about both the event system and the property system.
		//which involves thinking. taking a break...
		//todo: marker for break 12/14 evening
		if (context.Properties.TryAdd(key, val))
		{
			
		}
	}
	public static void Spawn(RuntimeBase context, RuntimeObject[] args)
	{
		if(context.Game.Prototypes.TryGetValue(args[0].AsString(), out var val))
		{
			context.Game.SpawnEntity(val);
		}
		else
		{
			throw new Exception($"Unable to spawn entity {args[0].ToString()}");
		}
	}
}