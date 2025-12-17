using HelloWorld.Interpreter;
using LangJam.Loader.AST;
using Microsoft.VisualBasic;

namespace LangJam;

public class Game : IStackContext
{
	//interpreter things
	public InputSystem InputSystem => _inputSystem;
	private InputSystem _inputSystem;
	private Interpreter _interpreter;

	public RoutineSystem RoutineSystem => _routineSystem;
	private RoutineSystem _routineSystem;
	//list of sprite data
	//list of level data
	//list of entities
	public Dictionary<string, ComponentDefinition> Components => _components;
	private Dictionary<string, ComponentDefinition> _components;
	
	private Dictionary<string, RuntimeObject> Globals = new Dictionary<string, RuntimeObject>();
	public Dictionary<string, SceneDefinition> SceneDefinitions => _sceneDefinitions;
	private Dictionary<string, SceneDefinition> _sceneDefinitions;

	public Dictionary<string, Sprite> Sprites => _sprites;
	private Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
	
	public Scene _rootScene;

	public Game()
	{
		_interpreter = new Interpreter();
		_inputSystem = new InputSystem();
		_routineSystem = new RoutineSystem();
	}
	public void SetComponentDefinitions(Dictionary<string, ComponentDefinition> components)
	{
		_components = components;
	}

	public void SetSprites(Dictionary<string, Sprite> sprites)
	{
		_sprites = sprites;
	}

	public Scene SpawnScene(Scene? parent, SceneDefinition? definition)
	{
		var e = definition.CreateInstance(this, _rootScene);
		if (parent == null)
		{
			_rootScene.AddChild(e);
		}
		else
		{
			parent.AddChild(e);
		}

		e.CallOnSpawn();
		return e;
	}

	public void WalkStatement(Expr expr, RuntimeBase context, Routine? routine = null)
	{
		_interpreter.WalkStatement(expr, context, routine);
	}

	public void LoadScene(SceneDefinition sceneDef)
	{
		_rootScene = sceneDef.CreateInstance(this, null!);
		_rootScene.RunRootLogic();//todo: eventually will remove this?
		_rootScene.CallOnSpawn();
	}
	
	public bool TryGetProperty(string id, out RuntimeObject expr)
	{
		return Globals.TryGetValue(id, out expr);
	}

	//called by render method.
	public void Tick()
	{
		_inputSystem.Tick();
		_routineSystem.TickRoutines();
		_rootScene.Tick();
	}

	public void SetSceneDefinitions(Dictionary<string, SceneDefinition> sceneDefs)
	{
		_sceneDefinitions = sceneDefs;
	}
}

public interface IStackContext
{
	public bool TryGetProperty(string id, out RuntimeObject runtimeObject);
}