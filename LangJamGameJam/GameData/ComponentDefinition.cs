using LangJam.Loader.AST;

namespace LangJam;

public class ComponentDefinition
{
	public List<Expr> Roots => _roots;
	private List<Expr> _roots;

	public ComponentDefinition(List<Expr> rootExpressions)
	{
		_roots = rootExpressions;
	}

	public Component CreateInstance(Game game, Entity entity)
	{
		var exprs = new List<Expr>();
		foreach (var expr in Roots)
		{
			exprs.Add(expr); //todo: do we need to clone this? is the code self-modifying?
		}

		return new Component( exprs, entity, game);
		//todo: run init function of the component
	}
}