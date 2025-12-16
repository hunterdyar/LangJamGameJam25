using HelloWorld;
using LangJam.Loader.AST;

namespace LangJam;

public class SceneDefinition 
{
	public string EntityDefName => _defName;
	private string _defName;
	private bool compListDirty = true;
	private List<ComponentDefinition> RealizedCompList;
	public bool HasRenderFunction = false;
	private List<string> CompList;
	public List<Expr> RootExprs => _rootExprs;
	private List<Expr> _rootExprs;

	public SceneDefinition(string entityName, List<Expr> pRootExpressions, List<string> compList)
	{
		_rootExprs = pRootExpressions;
		this._defName = entityName;
		CompList = compList;
	}

	public Scene CreateInstance(Game game, Scene parent)
	{
		//lazy init.
		if (compListDirty)
		{
			RealizedCompList = new List<ComponentDefinition>();
			foreach (string component in CompList)
			{
				if (game.Components.TryGetValue(component, out var c))
				{
					RealizedCompList.Add(c);
				}else if (NativeComponents.TryGetNativeComponent(component, game, parent, out var nc))
				{
					RealizedCompList.Add(new ComponentDefinition(component, new List<Expr>(), true));
				}
				else
				{
					throw new Exception($"error on entity {_defName}, unknown component {component}");
				}
			}

			compListDirty = false;
		}

		var comps = new Dictionary<string, ComponentBase>();
		var e = new Scene(this, game, parent)
		{
			Components = comps,
		};
		foreach (var definition in RealizedCompList)
		{
			var c = definition.CreateInstance(game, e);
			comps.Add(c.Name(),c);
		}

		e.Components = comps;
		e.RegisterEventFunctions(this.RootExprs);

		return e;
	}
	
}