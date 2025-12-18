using LangJam;
using LangJam.Loader.AST;

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

	public override bool TryExecuteMethod(string id, RuntimeObject[] toList)
	{
		switch (id)
		{
			case "get-scene-at":
				throw new Exception("shit, return types are fucked if we do it this way");
		}
		return base.TryExecuteMethod(id, toList);
	}

	public override bool TryGetMethod(string id, out DeclareExpr expr)
	{
		switch (id)
		{
			case "get-scene-at":
				// todo: replace returning the tree with "call method"
				
				break;
		}
			
		return base.TryGetMethod(id, out expr);
	}

	public override void SetProperty(string key, RuntimeObject val, bool forceCreate)
	{
		switch (key)
		{
			case "scale":
				GridInfo.Scale = (int)val.AsNumber();
				return;
			case "y-offset":
				GridInfo.SetYOffset(val.AsNumber());
				return;
			case "x-offset":
				GridInfo.SetXOffset(val.AsNumber());
				return;
			case "rows":
				GridInfo.Rows = (int)val.AsNumber();
				return;
			case "cols":
				GridInfo.Cols = (int)val.AsNumber();
				return;
			case "horizontal-align":
			case "h-align":
				GridInfo.SetHorizontalAlignment(val.AsString());
				return;
			case "vertical-align":
			case "v-align":
				GridInfo.SetVerticalAlignment(val.AsString());
				return;
		}

		base.SetProperty(key, val, forceCreate);
	}

	public LJBool HasSceneAt(LJPoint point)
	{
		return HasSceneAt(point.X, point.Y);
	}

	private LJBool HasSceneAt(LJNumber x, LJNumber y)
	{
		var any = Scene.Children.Any(s => s.Position.X.Value == x.Value && s.Position.Y.Value == y.Value);
		return new LJBool(any);
	}

	private LJSceneReference GetFirstSceneAt(LJNumber x, LJNumber y)
	{
		var f = Scene.Children.Find(s => s.Position.X.Value == x.Value && s.Position.Y.Value == y.Value);
		if (f == null)
		{
			//null scene references? do we have nulls? no? well uh
			throw new Exception($"cannot get child scene at grid position {x}/{y}");
		}

		return new LJSceneReference(f);
	}
}