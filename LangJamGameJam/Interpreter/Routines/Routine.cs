using System.Linq.Expressions;
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

	public Routine(SExpr routineBlock, RuntimeBase context)
	{
		_block = routineBlock;
		_context = context;
		_routineBlock = RoutineBlock();
	}

	private IEnumerator<YieldInstruction?> RoutineBlock()
	{
		//1; skip 'start-routine'
		for (var i = 1; i < _block.elements.Count; i++)
		{
			var n = _context.Game.Interpreter.WalkStatement(_block.elements[i], _context);
			if (n == null)
			{
				continue;
			}
			while (n != null && n.MoveNext())
			{
				while (n.Current.KeepWaiting())
				{
					n.Current.Tick();
					yield return n.Current;
				}
			}
		}
	}
	

	public IEnumerator<YieldInstruction?> Tick()
	{
		_routineBlock.MoveNext();
		while (_routineBlock.Current == null || !_routineBlock.Current.KeepWaiting())
		{
			if (!_routineBlock.MoveNext())
			{
				break;
			}
		}

		return null;
	}
}