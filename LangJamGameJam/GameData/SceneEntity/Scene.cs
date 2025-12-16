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

	public override bool TryGetProperty(string id, out RuntimeObject ro)
	{
		if (!Properties.TryGetValue(id, out ro))
		{
			return _game.TryGetProperty(id, out ro);
		}
		return true;
	}

	public override string ToString()
	{
		return $"Scene({_definition.EntityDefName})";
	}

	//
	// #region NativeRuntimeHelpers
	//
	// public bool GetScenesAt(LJPoint point, out List<Scene> entities)
	// {
	// 	entities = _children.Where(x=> x.Position == point).ToList();
	// 	return entities.Any();
	// }
	//
	// #endregion
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