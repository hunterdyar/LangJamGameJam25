using LangJam.Loader.AST;

namespace LangJam;

public class SceneDefinition
{
	public List<Expr> SceneLogic;

	public SceneDefinition(List<Expr> exprs)
	{
		SceneLogic = exprs;
	}

	public Scene CreateInstance(Game game)
	{
		return new Scene(game)
		{
			SceneLogic = SceneLogic,
		};
	}
}