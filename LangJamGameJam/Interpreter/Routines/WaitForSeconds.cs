
using Raylib_cs;

public class WaitForSeconds : YieldInstruction
{
	private readonly double _startTime;
	private readonly double _timeToWait;

	public WaitForSeconds(double time)
	{
		_startTime = Raylib_cs.Raylib.GetTime();
		_timeToWait = time;
	}
	public override bool KeepWaiting()
	{
		return Raylib.GetTime() < _startTime + _timeToWait;
	}
}