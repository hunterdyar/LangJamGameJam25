using System.Collections;


public abstract class YieldInstruction : IEnumerator
{
	public abstract bool KeepWaiting();
	public virtual bool ContinueAfter => true;

	public bool MoveNext()
	{
		return KeepWaiting();
	}

	public virtual void Tick()
	{
		
	}
	public virtual void Reset()
	{
		
	}

	public object Current => null;
}