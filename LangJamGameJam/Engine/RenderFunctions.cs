using LangJam;

public static class RenderFunctions
{
	public static RuntimeObject DrawGridColor(RuntimeBase context, RuntimeObject[] args)
	{
		var x = (int)args[0].AsNumber();
		var y = (int)args[1].AsNumber();
		var cs = args[2].AsString();
		var c = Utilities.StringToColor(cs);
		var s = context.Scene.GridInfo.Scale;
		Raylib_cs.Raylib.DrawRectangle(
			context.Scene.GridInfo.XOffset+x*s,
			context.Scene.GridInfo.XOffset+y*s,
			s,
			s,
			c);
		return null;
	}
}