using System.Security.Claims;
using HelloWorld;
using LangJam.Loader.AST;
using Microsoft.VisualBasic;

namespace LangJam;

public class ComponentDefinition 
{
	public List<Expr> RootExprs => _rootExprs;
	private List<Expr> _rootExprs;
	private string _name;
	private bool _isNative = false;
	public ComponentDefinition(string name, List<Expr> rootExpressions, bool isNative=false)
	{
		_isNative = isNative;
		_name = name;
		_rootExprs = rootExpressions;
	}

	public ComponentBase CreateInstance(Game game, Scene parent)
	{
		if (_isNative)
		{
			if(NativeComponents.TryGetNativeComponent(_name, game, parent, out var cb))
			{
				return cb;
			}
		}
		else
		{
			var exprs = new List<Expr>();
			foreach (var expr in RootExprs)
			{
				exprs.Add(expr); //todo: do we need to clone this? is the code self-modifying?
			}

			return new Component(exprs, _name, game, parent);
		}
		//todo: run init function of the component
		throw new Exception($"unable to create instance of component {_name}. uhoh!");
	}
}