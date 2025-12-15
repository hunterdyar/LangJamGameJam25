using LangJam.Loader.AST;

namespace LangJam;

public class EntityDefinition
{
	public string EntityDefName => _defName;
	private string _defName;
	private bool compListDirty = true;
	private List<ComponentDefinition> RealizedCompList;
	public bool HasRenderFunction = false;
	private List<string> CompList;
	private List<Expr> entityLogic;

	public EntityDefinition(string entityName, List<Expr> pRootExpressions, List<string> compList)
	{
		this._defName = entityName;
		entityLogic = pRootExpressions;
		CompList = compList;
		
	}

	public Entity GetRuntimeEntity(Game game)
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
		var e = new Entity(this, game)
		{
			Components = comps,
		};
		foreach (var definition in RealizedCompList)
		{
			comps.Add(definition.CreateInstance(game, e));
		}
		e.Components = comps;
		e.RegisterEventFunctions(this.entityLogic);
		
		return e;
	}
}