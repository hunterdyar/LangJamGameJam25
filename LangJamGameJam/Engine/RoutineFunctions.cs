using HelloWorld.Interpreter;
using LangJam;
public static class RoutineFunctions
{
	public static Dictionary<string, Func<RuntimeBase, RuntimeObject[], YieldInstruction?>> Yields =
		new Dictionary<string, Func<RuntimeBase, RuntimeObject[], YieldInstruction?>>()
		{
			{ "seconds", WaitForSeconds },
			{ "frames", WaitForFrames},
		};
	
	private static YieldInstruction? WaitForFrames(RuntimeBase context, RuntimeObject[] args)
	{
		var frames = args[0].AsNumber();
		return new YieldFrames((int)frames);
	}

	private static YieldInstruction? WaitForSeconds(RuntimeBase context, RuntimeObject[] args)
	{
		var seconds = args[0].AsNumber();
		return new WaitForSeconds(seconds);
	}
}