using LangJam.Loader.AST;

namespace LangJam;

public class SceneDefinition : DefinitionBase<Scene>
{
	public SceneDefinition(List<Expr> exprs) : base(exprs)
	{
	}

	/// <summary>
	/// Scene override gets ignored for now BUT this is how we would do a parent relationship, i think?
	/// </summary>
	public override Scene CreateInstance(Game game, Scene scene)
	{
		return new Scene(game)
		{
			SceneLogic = RootExprs,//todo: this can be to the def.
		};
	}
}