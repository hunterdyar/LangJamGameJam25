using LangJam;

public static class GridFunctions
{
	public static RuntimeObject? GetGrid(RuntimeBase context, RuntimeObject[] args)
	{
		var key = args[0].AsString();
		switch (key)
		{
			case "scale":
				return new LJNumber(context.Scene.GridInfo.Scale);
			case "x-offset":
				return new LJNumber(context.Scene.GridInfo.XOffset);
			case "y-offset":
				return new LJNumber(context.Scene.GridInfo.YOffset);
			case "rows":
				return new LJNumber(context.Scene.GridInfo.Rows);
			case "cols":
				return new LJNumber(context.Scene.GridInfo.Cols);
			default:
				throw new Exception($"Unable to get grid data {key}.");
		}
	}

	public static RuntimeObject? SetGrid(RuntimeBase context, RuntimeObject[] args)
	{
		var key = args[0].AsString();
		switch (key)
		{
			case "scale":
				context.Scene.GridInfo.Scale = (int)args[1].AsNumber();
				break;
			case "y-offset":
				context.Scene.GridInfo.SetYOffset(args[1].AsNumber());
				break;
			case "x-offset":
				context.Scene.GridInfo.SetXOffset(args[1].AsNumber());
				break;
			case "rows":
				context.Scene.GridInfo.Rows = (int)args[1].AsNumber();
				break;
			case "cols":
				context.Scene.GridInfo.Cols = (int)args[1].AsNumber();
				break;
			case "horizontal-align":
			case "h-align":
				context.Scene.GridInfo.SetHorizontalAlignment(args[1].AsString());
				break;
			case "vertical-align":
			case "v-align":
				context.Scene.GridInfo.SetVerticalAlignment(args[1].AsString());
				break;
			default:
				throw new Exception($"Unable to get grid data {key}.");
		}

		return null;
	}
}