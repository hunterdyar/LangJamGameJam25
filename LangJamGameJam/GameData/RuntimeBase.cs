using LangJam.Loader.AST;

namespace LangJam;

public abstract class RuntimeBase : IStackContext
{
	public Dictionary<string, Expr> Properties = new Dictionary<string, Expr>();

	public virtual bool TryGetProperty(string id, out Expr expr)
	{
		return Properties.TryGetValue(id, out expr);
	}
}