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

	public override void SetProperty(string key, RuntimeObject val, bool createIfDoesntExist)
	{
		if (createIfDoesntExist)
		{
			if (!Properties.TryAdd(key, val))
			{
				Properties[key] = val;
			}
		}
		else
		{
			if (Properties.ContainsKey(key))
			{
				Properties[key] = val;
			}
			else
			{
				Scene.SetProperty(key, val, createIfDoesntExist);
			}
		}
	}
}