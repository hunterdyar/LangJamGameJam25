using LangJam.Loader.AST;

namespace LangJam;

public abstract class DefinitionBase<T> where T : RuntimeBase
{
	public abstract T CreateInstance(Game game, Scene scene);
	public List<Expr> RootExprs => _rootExprs;
	private List<Expr> _rootExprs;

	protected DefinitionBase(List<Expr> rootExprs)
	{
		_rootExprs = rootExprs;
	}
}