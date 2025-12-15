using LangJam.Loader.AST;

namespace LangJam;

public class Component : RuntimeBase
{
	private List<Expr> _exprs;
	private Entity _entity;
	private Dictionary<string, Expr> Properties;

	public Component(List<Expr> exprs, Entity entity, Game game) : base(game)
	{
		_exprs = exprs;
		_entity = entity;
		Properties = new Dictionary<string, Expr>();
	}
	
	public override bool TryGetProperty(string id, out Expr expr)
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