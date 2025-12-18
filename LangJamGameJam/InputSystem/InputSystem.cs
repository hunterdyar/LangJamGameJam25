
using LangJam;
using LangJam.Loader.AST;
using Raylib_cs;

public enum InputButtonState
{
	OnFirstPress,
	OnDown,
	OnRelease
}
public struct LJInputEvent : IEquatable<LJInputEvent>
{
	public int key;
	public InputButtonState State;

	public LJInputEvent(int key, InputButtonState state)
	{
		this.key = key;
		State = state;
	}

	public bool Equals(LJInputEvent other)
	{
		return key == other.key && State == other.State;
	}

	public override bool Equals(object? obj)
	{
		return obj is LJInputEvent other && Equals(other);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(key, (int)State);
	}

	public override string ToString()
	{
		return $"ie:{key}_{State}";
	}

	public static bool operator ==(LJInputEvent left, LJInputEvent right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(LJInputEvent left, LJInputEvent right)
	{
		return !left.Equals(right);
	}
}

public class InputSystem
{
	public Dictionary<LJInputEvent, List<(RuntimeBase, DeclareExpr, RuntimeObject[])>> Registered = new Dictionary<LJInputEvent, List<(RuntimeBase, DeclareExpr, RuntimeObject[])>>();
	
	public void OnSystemEvent()
	{
		//find the appropriate event and uh, you know. send it.
	}

	//todo: pass along arguments
	public void RegisterInputEvent(RuntimeBase element, DeclareExpr tocall, string keyname, string keystate, RuntimeObject[] args)
	{
		var key = StringToKeycode(keyname);
		var state = StringToState(keystate);
		var ljievent = new LJInputEvent(key, state);
		if (Registered.ContainsKey(ljievent))
		{
			Registered[ljievent].Add((element, tocall,args));
		}
		else
		{
			Registered.Add(ljievent, [(element,tocall,args)]);
		}
	}

	public void UnregisterInputEvent(RuntimeBase element,DeclareExpr tocall, string keyname, string keystate)
	{
		var key = StringToKeycode(keyname);
		var state = StringToState(keystate);
		var ljievent = new LJInputEvent(key, state);
		if (Registered.ContainsKey(ljievent))
		{
			Registered[ljievent].Remove((element,tocall, []));
		}
		else
		{
			throw new Exception($"cann't unregister input event ({keyname}:{keystate}, event not in system.");
		}
	}


	private InputButtonState StringToState(string keystate)
	{
		switch (keystate)
		{
			case "press":
				return InputButtonState.OnFirstPress;
			case "release":
				return InputButtonState.OnRelease;
			case "down":
				return InputButtonState.OnDown;
			default:
				throw new Exception(
					$"Unable to register input event. Unknown state {keystate}. Expected 'press', 'release', or 'down'");
		}
	}

	private static int StringToKeycode(string keyname)
	{
		keyname = keyname.ToLower();
		switch (keyname)
		{
			case "up": return (int)KeyboardKey.Up;
			case "down": return (int)KeyboardKey.Down;
			case "left": return (int)KeyboardKey.Left;
			case "right": return (int)KeyboardKey.Right;
			case "space": return (int)KeyboardKey.Space;
			case "w": return (int)KeyboardKey.W;
			case "a": return (int)KeyboardKey.A;
			case "s": return (int)KeyboardKey.S;
			case "d": return (int)KeyboardKey.D;
			//todo: there has to be a better way to do this.
			default:
				throw new Exception(
					$"Unable to register input event. Unknown key {keyname}. This is probably my fault.");
		}
	}

	private LJInputEvent _tickEvent = new LJInputEvent();
	public void Tick()
	{
		var c = Raylib.GetKeyPressed();
		while (c != 0)
		{
			//get the state for c? pressed or not? huh?
			var state = InputButtonState.OnFirstPress;
			var key = c;
			_tickEvent.key = key;
			_tickEvent.State = state;
			if (Registered.TryGetValue(_tickEvent, out var value))
			{
				foreach (var context in value)
				{
					 context.Item1.Game.Interpreter.WalkDeclaredExpr(context.Item2, context.Item1, context.Item3);
				}
			}
			c = Raylib.GetKeyPressed();
		}
	}
}

