using LangJam;
public static class RoutineFunctions
{
	public static Dictionary<string, Func<RuntimeBase, RuntimeObject[], YieldInstruction?>> Yields =
		new Dictionary<string, Func<RuntimeBase, RuntimeObject[], YieldInstruction?>>()
		{
			{ "wait", WaitForSeconds },
		};

	private static YieldInstruction? WaitForSeconds(RuntimeBase context, RuntimeObject[] args)
	{
		var seconds = args[0].AsNumber();
		return new WaitForSeconds(seconds);
	}
}