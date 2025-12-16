using LangJam.Loader.AST;
using Microsoft.VisualBasic;

namespace LangJam;

public abstract class RuntimeBase : IStackContext
{
	public Dictionary<string, RuntimeObject> Properties = new Dictionary<string, RuntimeObject>();
	//todo:ExpressionBody Type
	public Dictionary<string, DeclareExpr> Methods = new Dictionary<string, DeclareExpr>();
	
	public Expr[]? RenderCall;
	public Expr[]? OnSpawn;
	public Expr[]? OnEnable;
	public Expr[]? OnDisable;
	public bool NeedsOnSpawn = true;
	public bool Enabled = true;
	public Game Game => _game;
	protected Game _game;
	public Scene Scene => _scene;
	protected Scene _scene;

	protected RuntimeBase(Game game, Scene scene)
	{
		_game = game;
		_scene = scene;
	}

	public void RegisterEventFunctions(List<Expr> baseExpressions)
	{
		int registeredCount = 0;
		foreach (var expression in baseExpressions)
		{
			if (expression is DeclareExpr decExpr)
			{
				var id = decExpr.Identifier.ToString();
				
				//we just add to the Methods dictionary. the others are cache for the hot-path, or because i felt like it.
				switch (id)
				{
					case "render":
						RenderCall = decExpr.elements;
						Methods.Add(id, decExpr);
						break;
					case "on-spawn":
						OnSpawn = decExpr.elements;
						Methods.Add(id, decExpr);
						break;
					case "on-enable":
						OnEnable = decExpr.elements;
						Methods.Add(id, decExpr);
						break;
					case "on-disable":
						OnDisable = decExpr.elements;
						Methods.Add(id, decExpr);
						break;
					default:
						Methods.Add(id, decExpr);
						break;
				}
			}
		}
	}


	public virtual bool TryGetProperty(string id, out RuntimeObject expr)
	{
		return Properties.TryGetValue(id, out expr);
	}
	
	//todo: after we do {definitions}, these event functions can be different. although probably still want render cached without the lookup.
	public virtual void CallRender()
	{
		if (!Enabled)
		{
			return;
		}
		if (RenderCall != null)
		{
			WalkExpressionArray(RenderCall);
		}
	}

	public void CallOnSpawn()
	{
		if (!Enabled)
		{
			return;
		}
		if (!NeedsOnSpawn)
		{
			Console.WriteLine("calling onSpawn for entity twice! oops");
			return;
		}

		if (OnSpawn != null)
		{
			WalkExpressionArray(OnSpawn);
		}

		NeedsOnSpawn = false;
	}

	public void SetEnabled(bool enabled)
	{
		if (Enabled && !enabled)
		{
			Enabled = enabled;
			WalkExpressionArray(OnEnable);
		}else if (!Enabled && enabled)
		{
			Enabled = enabled;
			WalkExpressionArray(OnDisable);
		}
	}

	public void WalkDeclaredExpr(DeclareExpr expr)
	{
		for (int i = 0; i < expr.elements.Length; i++)
		{
			_game.WalkStatement(expr.elements[i], this);
		}
	}
	private void WalkExpressionArray(Expr[] exprs)
	{
		for (int i = 0; i < exprs.Length; i++)
		{
			_game.WalkStatement(exprs[i],this);
		}
	}
}