using LangJam.Loader.AST;

namespace LangJam;

public class EntityDefinition : DefinitionBase<Entity>
{
	public string EntityDefName => _defName;
	private string _defName;
	private bool compListDirty = true;
	private List<ComponentDefinition> RealizedCompList;
	public bool HasRenderFunction = false;
	private List<string> CompList;

	public EntityDefinition(string entityName, List<Expr> pRootExpressions, List<string> compList) : base(pRootExpressions)
	{
		this._defName = entityName;
		CompList = compList;
	}

	public override Entity CreateInstance(Game game, Scene scene)
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
				}
				else
				{
					throw new Exception($"error on entity {_defName}, unknown component {component}");
				}
			}

			compListDirty = false;
		}

		var comps = new List<Component>();
		var e = new Entity(this, game, scene)
		{
			Components = comps,
		};
		foreach (var definition in RealizedCompList)
		{
			var c = definition.CreateInstance(game, scene);
			c.SetEntity(e);
			comps.Add(c);
		}
		
		e.Components = comps;
		e.RegisterEventFunctions(this.RootExprs);
		
		return e;
	}
}