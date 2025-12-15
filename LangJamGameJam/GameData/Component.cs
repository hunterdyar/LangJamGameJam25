using LangJam.Loader.AST;

namespace LangJam;

public class Component : RuntimeBase
{
	private List<Expr> _exprs;
	private Entity _entity;

	public Component(List<Expr> exprs, Game game) : base(game)
	{
		_exprs = exprs;
		RegisterEventFunctions(_exprs);
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

	public void SetEntity(Entity entity)
	{
		_entity = entity;
	}
}