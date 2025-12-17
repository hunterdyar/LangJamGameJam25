using System.Runtime;
using LangJam;
using LangJam.Loader.AST;

public class Routine
{
	private SExpr _block;
	private RuntimeBase _context;
	private IEnumerator<YieldInstruction> _routineBlock;
	public bool Complete => _complete;
	private bool _complete = false;

	private Queue<YieldInstruction> _instructions = new Queue<YieldInstruction>();
	public Routine(SExpr routineBlock, RuntimeBase context)
	{
		_block = routineBlock;
		_context = context;
		_routineBlock = RoutineBlock();
	}

	public bool ShouldNotTick()
	{
		if(_instructions.TryPeek(out var result))
		{
			return result.KeepWaiting();
		}
		return false;
	}

	private IEnumerator<YieldInstruction> RoutineBlock()
	{
		//1; skip 'routine'
		for (var i = 1; i < _block.elements.Count; i++)
		{
			_context.Game.WalkStatement(_block.elements[i], _context, this);
			//during the walk, _instructions will get enqueued with any yields in this area.
			if (_instructions.Count > 1)
			{
				throw new Exception(
					"we've stacked yields, they're nested in some way. this should be allowed but isn't because the routine system is a fragile mess");
			}
			while (_instructions.Count > 0)
			{
				var yi = _instructions.Peek();
				yi.Tick();
				if (yi.KeepWaiting())
				{
					yield return yi;
				}
				else
				{
					_instructions.Dequeue();
				}
			}
		}
	}

	public void StartSubroutine(Routine routine)
	{
		AddYieldInstructionAtCurrent(new YieldForRoutine(routine));
	}

	public void Tick()
	{
		if (_routineBlock.Current == null)
		{
			_routineBlock.MoveNext();
		}else if (_routineBlock.Current.KeepWaiting())
		{
			return;
		}
		else
		{
			_routineBlock.MoveNext();
		}
	}

	public void AddYieldInstructionAtCurrent(YieldInstruction? yi)
	{
		if (yi != null)
		{
			_instructions.Enqueue(yi);
		}
	}
}