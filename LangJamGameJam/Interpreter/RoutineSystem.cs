using System.Runtime;

namespace HelloWorld.Interpreter;

public class RoutineSystem
{
	private List<Routine> _list;

	public RoutineSystem()
	{
		_list = new List<Routine>();
	}

	private List<Routine> _markForDelete = new List<Routine>();

	public void StartRoutine(Routine routine)
	{
		_list.Add(routine);
		routine.Tick();
	}
	public void TickRoutines()
	{
		//i really don't like this code pattern. 
		//normally i would iterate through the list backwards and remove them, but routine order probably matters.
		_markForDelete.Clear();
		foreach (var routine in _list)
		{
			routine.Tick();
			if (routine.Complete)
			{
				_markForDelete.Add(routine);
			}
		}
		
		//remove completed routines. 
		foreach (var routine in _markForDelete)
		{
			_list.Remove(routine);
		}
	}
}