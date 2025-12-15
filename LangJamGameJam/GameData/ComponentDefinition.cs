using LangJam.Loader.AST;

namespace LangJam;

public class ComponentDefinition
{
	private List<Expr> _roots;
	public Component(List<Expr> rootExpressions)
	{
		_roots = rootExpressions;
	}
}