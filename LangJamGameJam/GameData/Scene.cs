using LangJam.Loader.AST;

namespace LangJam;

public class Scene : RuntimeBase
{
	public List<Expr> SceneLogic;
	private List<Entity> _entities;
	public GridInfo GridInfo => _gridInfo;
	private GridInfo _gridInfo = new GridInfo();
	public Scene(Game game) : base(game, null)
	{
		_scene = this;
		_entities = new List<Entity>();
	}
	public void RunSceneLogic()
	{
		foreach (var core in SceneLogic)
		{
			_game.WalkStatement(core, this);
		}
	}

	public void AddEntity(Entity entity)
	{
		if (!_entities.Contains(entity))
		{
			_entities.Add(entity);
			entity.SetLoaded(true);
		}
		else
		{
			throw new Exception("entity already added, cannot add.");
		}
	}

	public void RemoveEntity(Entity entity)
	{
		if (_entities.Contains(entity))
		{
			_entities.Remove(entity);
		}
		else
		{
			throw new Exception("entity not in scene, cannot remove.");
		}
	}

	public void Tick()
	{
		//render the scene.
		CallRender();
		foreach (var entity in _entities)
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

	#region RuntimeHelpers

	public bool GetEntitiesAt(LJPoint point, out List<Entity> entities)
	{
		entities = _entities.Where(x=> x.Position == point).ToList();
		return entities.Any();
	}

	#endregion
}