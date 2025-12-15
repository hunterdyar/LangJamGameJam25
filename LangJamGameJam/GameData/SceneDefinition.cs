using LangJam.Loader.AST;

namespace LangJam;

public class SceneDefinition : DefinitionBase<Scene>
{

	public SceneDefinition(List<Expr> exprs) : base(exprs)
	{
	}

	public override Scene CreateInstance(Game game)
	{
		return new Scene(game)
		{
			SceneLogic = RootExprs,//todo: this can be to the def.
		};
	}
}