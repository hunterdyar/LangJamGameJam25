
public class EarlyReturn : YieldInstruction
{
	public override bool ContinueAfter => false;

	public override bool KeepWaiting()
	{
		return false;
	}
	
}