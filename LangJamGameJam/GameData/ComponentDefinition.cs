using LangJam.Loader.AST;

namespace LangJam;

public class ComponentDefinition : DefinitionBase<Component>
{
	public ComponentDefinition(List<Expr> rootExpressions) : base(rootExpressions)
	{
	}

	public override Component CreateInstance(Game game, Scene scene)
	{
		var exprs = new List<Expr>();
		foreach (var expr in RootExprs)
		{
			exprs.Add(expr); //todo: do we need to clone this? is the code self-modifying?
		}

		return new Component(exprs, game, scene);
		//todo: run init function of the component
	}
}