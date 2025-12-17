namespace LangJam;

public class FrameContext : RuntimeBase
{
	private IStackContext Parent;

	public FrameContext(RuntimeBase parent) : base(parent.Game, parent.Scene)
	{
		Parent = parent;
	}

	public override bool TryGetProperty(string id, out RuntimeObject value)
	{
		if (!Properties.TryGetValue(id, out value))
		{
			return Parent.TryGetProperty(id, out value);
		}

		return true;
	}
}