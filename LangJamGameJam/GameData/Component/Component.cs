using LangJam.Loader.AST;

namespace LangJam;

public class Component : ComponentBase
{
	private List<Expr> _exprs;

	public Component(List<Expr> exprs, string name, Game game, Scene scene) : base(name, game, scene)
	{
		_exprs = exprs;
		RegisterEventFunctions(_exprs);
	}

}