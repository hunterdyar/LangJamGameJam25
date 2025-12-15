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
		var value = args[1].AsNumber();
		switch (key)
		{
			case "scale":
				context.Scene.GridInfo = context.Scene.GridInfo with { Scale = (int)value };
				break;
			case "y-offset":
				context.Scene.GridInfo = context.Scene.GridInfo with { YOffset = (int)value };
				break;
			case "x-offset":
				context.Scene.GridInfo = context.Scene.GridInfo with { XOffset = (int)value };
				break;
			case "rows":
				context.Scene.GridInfo = context.Scene.GridInfo with { Rows = (int)value };
				break;
			case "cols":
				context.Scene.GridInfo = context.Scene.GridInfo with { Cols = (int)value };
				break;
			default:
				throw new Exception($"Unable to get grid data {key}.");
		}

		return null;
	}
}