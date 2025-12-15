using LangJam.Loader.AST;

namespace LangJam;

public abstract class RuntimeBase : IStackContext
{
	public Dictionary<string, Expr> Properties = new Dictionary<string, Expr>();
	
	private Expr? RenderCall;
	public Game Game => _game;
	protected Game _game;

	protected RuntimeBase(Game game)
	{
		_game = game;
	}


	public virtual bool TryGetProperty(string id, out Expr expr)
	{
		return Properties.TryGetValue(id, out expr);
	}

	
	public virtual void CallRender()
	{
		//todo: cache this
		Properties.TryGetValue("render", out RenderCall);
		
		if (RenderCall != null)
		{
			_game.WalkStatement(RenderCall, this);
		}
	}
}