using LangJam;

public static class RenderFunctions
{
	public static RuntimeObject DrawGridColor(RuntimeBase context, RuntimeObject[] args)
	{
		var x = (int)args[0].AsNumber();
		var y = (int)args[1].AsNumber();
		var cs = args[2].AsString();
		var c = Utilities.StringToColor(cs);
		
		Raylib_cs.Raylib.DrawRectangle(x*10, y*10, 100, 100, c);
		return null;
	}
}