namespace LangJam;

public class ComponentBase : RuntimeBase
{
	protected Entity _entity;

	public ComponentBase(Game game, Scene scene) : base(game, scene)
	{
		
	}

	public void SetEntity(Entity entity)
	{
		_entity = entity;
	}
	public override bool TryGetProperty(string id, out RuntimeObject expr)
	{
		if (!Properties.TryGetValue(id, out expr))
		{
			return _entity.TryGetProperty(id, out expr);
		}
		else
		{
			return false;
		}
	}
	
}