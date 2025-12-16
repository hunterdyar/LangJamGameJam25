using LangJam;

public static class MathFunctions
{
	
	public static RuntimeObject Increment(RuntimeBase context, RuntimeObject[] args)
	{
		return new LJNumber(args[0].AsNumber() + 1);
	}

	//we can probably normalize some kind of "transform single number property" function
	public static RuntimeObject Decrement(RuntimeBase context, RuntimeObject[] args)
	{
		return new LJNumber(args[0].AsNumber() - 1);
	}


}