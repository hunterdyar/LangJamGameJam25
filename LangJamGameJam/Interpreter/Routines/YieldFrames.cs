namespace HelloWorld.Interpreter;

public class YieldFrames : YieldInstruction
{
	private int _waitCount = 0;

	public YieldFrames(int frames)
	{
		_waitCount = frames;
	}
	public override bool KeepWaiting()
	{
		return _waitCount > 0;
	}

	public override void Tick()
	{
		_waitCount--;
	}
}