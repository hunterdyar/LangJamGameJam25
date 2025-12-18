using LangJam.Loader.AST;

namespace LangJam;

public class ComponentBase : RuntimeBase
{
	protected Scene _myScene;//todo:remove. use scene in runtimeBase
	protected string ComponentName { get; set; }

	public ComponentBase(string name, Game game, Scene scene) : base(game, scene)
	{
		ComponentName = name;
		_myScene = scene;
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
			return true;
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

	public override bool TryGetMethod(string id, out DeclareExpr expr)
	{
		if (!Methods.TryGetValue(id, out expr))
		{
			return _myScene.TryGetMethod(id, out expr);
		}
		else
		{
			return true;
		}
	}
}