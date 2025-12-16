using System.Numerics;
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
	
	public static Func<RuntimeBase, RuntimeObject[], RuntimeObject?> BinComp(Func<double,double,bool> op)
	{
		return new Func<RuntimeBase, RuntimeObject[], RuntimeObject?>((context, args) =>
		{
			var left = args[0].AsNumber();
			var right = args[1].AsNumber();
			var result = op.Invoke(left, right);
			return new LJBool(result);
		});
	}
	
}