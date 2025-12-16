using LangJam.Loader.AST;

namespace LangJam;

public class Scene : RuntimeBase
{
	private List<Scene> _children;
	private SceneDefinition _definition;
	
	#region Grid
	public LJPoint Position => new LJPoint(Properties["x"].AsNumber(), Properties["y"].AsNumber());
	#endregion

	public bool Loaded { get; private set; }
	public Dictionary<string, ComponentBase> Components;
	public Scene Parent;

	public void SetLoaded(bool loaded)
	{
		Loaded = loaded;
	}
	public Scene(SceneDefinition definition, Game game, Scene parent) : base(game, parent)
	{
		_definition = definition;
		_scene = this;
		Parent = parent;
		_children = new List<Scene>();
	}
	
	public void RunRootLogic()
	{
		foreach (var core in _definition.RootExprs)
		{
			_game.WalkStatement(core, this);
		}
	}

	public void AddChild(Scene entity)
	{
		if (!_children.Contains(entity))
		{
			_children.Add(entity);
			entity.Parent = this;
			entity.SetLoaded(true);
		}
		else
		{
			throw new Exception("entity already added, cannot add.");
		}
	}

	public void RemoveEntity(Scene entity)
	{
		if (_children.Contains(entity))
		{
			_children.Remove(entity);
		}
		else
		{
			throw new Exception("entity not in scene, cannot remove.");
		}
	}

	public void Tick()
	{
		//call self
		CallRender();
		foreach (var comp in Components)
		{
			comp.Value.CallRender();
		}
		
		//call children
		foreach (var entity in _children)
		{
			entity.CallRender();
		}
	}

	public void CallMethodRecursive(string methodName)
	{
		if (Methods.TryGetValue(methodName, out var expr))
		{
			WalkDeclaredExpr(expr);
		}

		foreach (var kvp in Components)
		{
			if(kvp.Value.Methods.TryGetValue(methodName, out var cexpr))
			{
				WalkDeclaredExpr(cexpr);
			}
		}

		foreach (var child in _children)
		{
			child.CallMethodRecursive(methodName);
		}
	}

	public override bool TryGetProperty(string id, out RuntimeObject ro)
	{
		if (!Properties.TryGetValue(id, out ro))
		{
			return _game.TryGetProperty(id, out ro);
		}
		return true;
	}

	//todo: "call-down" for event function things.
	public override void CallRender()
	{
		base.CallRender();
		foreach (var child in _children)
		{
			child.CallRender();
		}
	}

	public override string ToString()
	{
		return $"Scene({_definition.EntityDefName})";
	}
	public T? GetComponent<T>(string compName) where T : ComponentBase
	{
		if(Components.TryGetValue(compName, out var componentBase))
		{
			return componentBase as T;
		}

		throw new Exception($"Unable to get component {compName} on {this}");

		return null;
	}

	public ComponentBase GetComponent(string compName)
	{
		if (Components.TryGetValue(compName, out var componentBase))
		{
			return componentBase;
		}

		return null;
	}

	public bool TryGetComponent(string key, out ComponentBase c)
	{
		return Components.TryGetValue(key, out c);
	}
}