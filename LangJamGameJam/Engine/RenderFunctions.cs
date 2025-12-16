using LangJam;

public static class RenderFunctions
{
	public static RuntimeObject DrawGridColor(RuntimeBase context, RuntimeObject[] args)
	{
		var x = (int)args[0].AsNumber();
		var y = (int)args[1].AsNumber();
		var cs = args[2].AsString();
		var c = Utilities.StringToColor(cs);
		var grid = context.Scene.GetComponent<NativeComps.Grid>("grid");

		var s = grid.GridInfo.Scale;
		Raylib_cs.Raylib.DrawRectangle(
			grid.GridInfo.XOffset+x*s,
			grid.GridInfo.YOffset+y*s,
			s,
			s,
			c);
		return null;
	}
	
	//draws an unscaled sprite at a grid position.
	public static RuntimeObject DrawGridSprite(RuntimeBase context, RuntimeObject[] args)
	{
		var g = ((args[0] as LJComponentReference)?.Value) as NativeComponent;
		var grid = (NativeComps.Grid)g!;
		if (grid == null)
		{
			throw new Exception("first value of draw-grid-sprite must be a reference to a grid component");
		}
		var x = (int)args[1].AsNumber();
		var y = (int)args[2].AsNumber();
		var snamne = args[3].AsString();
		

		if(!context.Game.Sprites.TryGetValue(snamne, out var sprite))
		{
			throw new Exception($"Unable to render sprite {snamne}. could not find sprite. (hint: names are filenames, all lowercase)");
		}

		var s = grid.GridInfo.Scale;
		var dx = grid.GridInfo.XOffset + x * s;
		var dy = grid.GridInfo.YOffset + y * s;
		sprite.Draw(dx,dy);
		return null;
	}
}