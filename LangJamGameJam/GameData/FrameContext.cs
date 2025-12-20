using LangJam.Loader.AST;

namespace LangJam;

public class FrameContext : RuntimeBase
{
	private IStackContext? PropertyGetFallback;
	private IStackContext? PropertySetFallback;
	private IStackContext? MethodGetFallback;
	private IStackContext? MethodSetFallback = null;
	public FrameContext(RuntimeBase parent) : base(parent.Game, parent.Scene)
	{
		PropertyGetFallback = parent;
		PropertySetFallback = parent;
		MethodGetFallback = parent;
	}

	public override bool TryGetProperty(string id, out RuntimeObject value)
	{
		if (!Properties.TryGetValue(id, out value))
		{
			return PropertyGetFallback?.TryGetProperty(id, out value) ?? false;
		}

		return true;
	}

	public override void SetProperty(string key, RuntimeObject val, bool createIfDoesntExist)
	{
		if (createIfDoesntExist)
		{
			if (!Properties.TryAdd(key, val))
			{
				Properties[key] = val;
			}
		}
		else
		{
			if (Properties.ContainsKey(key))
			{
				Properties[key] = val;
			}
			else
			{
				//must not be ours, fail upwards!
				if (PropertySetFallback != null)
				{
					PropertySetFallback.SetProperty(key, val, createIfDoesntExist);
				}
				else
				{
					throw new Exception($"cannot set {key} property on {this}");
				}
			}
		}
	}

	public override bool TryGetMethod(string id, out DeclareExpr expr)
	{
		if (!base.TryGetMethod(id, out expr))
		{
			if (MethodGetFallback != null)
			{
				return MethodGetFallback.TryGetMethod(id, out expr);
			}
		}

		return true;
	}
}