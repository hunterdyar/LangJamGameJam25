
public class YieldForRoutine : YieldInstruction
{
	private Routine _routine;
	public YieldForRoutine(Routine routine)
	{
		_routine = routine;
	}

	public override void Tick()
	{
		_routine.Tick();
	}

	public override bool KeepWaiting()
	{
		return !_routine.Complete;
	}
}