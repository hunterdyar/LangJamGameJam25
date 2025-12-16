using LangJam;

namespace NativeComps;
public class Grid : NativeComponent
{
	public Grid(Game game, Scene scene) : base("grid",game, scene)
	{
		
	}
	
	public GridInfo GridInfo
	{
		get => _gridInfo;
		set => _gridInfo = value;
	}

	private GridInfo _gridInfo = new GridInfo();
	public override bool TryGetProperty(string id, out RuntimeObject expr)
	{
		switch (id)
		{
			case "scale":
				expr = new LJNumber(GridInfo.Scale);
				return true;
			case "x-offset":
				expr = new LJNumber(GridInfo.XOffset);
				return true;
			case "y-offset":
				expr = new LJNumber(GridInfo.YOffset);
				return true;
			case "rows":
				expr = new LJNumber(GridInfo.Rows);
				return true;
			case "cols":
				expr = new LJNumber(GridInfo.Cols);
				return true;
			default:
				return base.TryGetProperty(id, out expr);
		}
	}

	public override void SetProperty(string key, RuntimeObject val)
	{
		switch (key)
		{
			case "scale":
				GridInfo.Scale = (int)val.AsNumber();
				break;
			case "y-offset":
				GridInfo.SetYOffset(val.AsNumber());
				break;
			case "x-offset":
				GridInfo.SetXOffset(val.AsNumber());
				break;
			case "rows":
				GridInfo.Rows = (int)val.AsNumber();
				break;
			case "cols":
				GridInfo.Cols = (int)val.AsNumber();
				break;
			case "horizontal-align":
			case "h-align":
				GridInfo.SetHorizontalAlignment(val.AsString());
				break;
			case "vertical-align":
			case "v-align":
				GridInfo.SetVerticalAlignment(val.AsString());
				break;
			default:
				throw new Exception($"Unable to get grid data {key}.");
		}

		base.SetProperty(key, val);
	}
}