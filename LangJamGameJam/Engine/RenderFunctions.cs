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
			context.Scene.GridInfo.YOffset+y*s,
			s,
			s,
			c);
		return null;
	}
	
	//draws an unscaled sprite at a grid position.
	public static RuntimeObject DrawGridSprite(RuntimeBase context, RuntimeObject[] args)
	{
		var x = (int)args[0].AsNumber();
		var y = (int)args[1].AsNumber();
		var snamne = args[2].AsString();
		if(!context.Game.Sprites.TryGetValue(snamne, out var sprite))
		{
			throw new Exception($"Unable to render sprite {snamne}. could not find sprite. (hint: names are filenames, all lowercase)");
		}

		var s = context.Scene.GridInfo.Scale;
		var dx = context.Scene.GridInfo.XOffset + x * s;
		var dy = context.Scene.GridInfo.YOffset + y * s;
		sprite.Draw(dx,dy);
		return null;
	}
}