using LangJam.Loader.AST;

namespace LangJam;

public abstract class RuntimeBase : IStackContext
{
	public Dictionary<string, RuntimeObject> Properties = new Dictionary<string, RuntimeObject>();

	public SExpr? RenderCall;
	public SExpr? OnSpawn;
	public bool NeedsOnSpawn = true;

	public Game Game => _game;
	protected Game _game;

	protected RuntimeBase(Game game)
	{
		_game = game;
	}

	public void RegisterEventFunctions(List<Expr> baseExpressions)
	{
		int registeredCount = 0;
		foreach (var expression in baseExpressions)
		{
			if (expression is SExpr sExpr)
			{
				switch (sExpr.Identifier.ToString())
				{
					case "render":
						RenderCall = sExpr;
						break;
					case "on-spawn":
						OnSpawn = sExpr;
						break;
				}
			}
		}
	}


	public virtual bool TryGetProperty(string id, out RuntimeObject expr)
	{
		return Properties.TryGetValue(id, out expr);
	}
	
	public virtual void CallRender()
	{
		if (RenderCall != null)
		{
			//todo: make 'execute-s-expr' a proper AST node so we can not do this 'skip identifier' garbage.
			for (int i = 1; i < RenderCall.elements.Count; i++)
			{
				_game.WalkStatement(RenderCall.elements[i], this);
			}
		}
	}

	public void CallOnSpawn()
	{
		if (!NeedsOnSpawn)
		{
			Console.WriteLine("calling onSpawn for entity twice! oops");
			return;
		}

		if (OnSpawn != null)
		{
			//see todo about onrender, refactoring these AST nodes.
			for (int i = 0; i < OnSpawn.elements.Count; i++)
			{
				_game.WalkStatement(OnSpawn.elements[i], this);
			}
		}

		NeedsOnSpawn = false;
	}

}