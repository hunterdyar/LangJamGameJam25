namespace LangJam;

public class ComponentBase : RuntimeBase
{
	protected Scene _myScene;
	protected string ComponentName { get; set; }

	public ComponentBase(string name, Game game, Scene scene) : base(game, scene)
	{
		ComponentName = name;
	}

	public virtual string Name()
	{
		return ComponentName;
	}

	public void SetEntity(Scene entity)
	{
		_myScene = entity;
	}
	public override bool TryGetProperty(string id, out RuntimeObject expr)
	{
		if (!Properties.TryGetValue(id, out expr))
		{
			return _myScene.TryGetProperty(id, out expr);
		}
		else
		{
			return false;
		}
	}
	
}