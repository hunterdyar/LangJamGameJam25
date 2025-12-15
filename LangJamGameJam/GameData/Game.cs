using LangJam.Loader.AST;
using Microsoft.VisualBasic;

namespace LangJam;

public class Game : IStackContext
{
	//interpreter things
	private Interpreter _interpreter;
	//list of sprite data
	//list of level data
	//list of entities
	public Dictionary<string, ComponentDefinition> Components => _components;
	private Dictionary<string, ComponentDefinition> _components;
	
	private Dictionary<string, RuntimeObject> Globals = new Dictionary<string, RuntimeObject>();
	public Dictionary<string, EntityDefinition> Prototypes;
	public Dictionary<string, SceneDefinition> SceneDefinitions => _sceneDefinitions;
	private Dictionary<string, SceneDefinition> _sceneDefinitions;

	public Dictionary<string, Sprite> Sprites => _sprites;
	private Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
	
	public Scene _loadedScene;

	public Game()
	{
		_interpreter = new Interpreter();
	}
	public void SetComponentDefinitions(Dictionary<string, ComponentDefinition> components)
	{
		_components = components;
	}

	public void SetEntityDefinitions(Dictionary<string, EntityDefinition> entitiyDefs)
	{
		Prototypes = entitiyDefs;
	}

	public void SetSprites(Dictionary<string, Sprite> sprites)
	{
		_sprites = sprites;
	}

	public void SpawnEntity(EntityDefinition definition)
	{
		var e = definition.CreateInstance(this, _loadedScene);
		_loadedScene.AddEntity(e);
		e.CallOnSpawn();
	}

	public void WalkStatement(Expr expr, RuntimeBase context)
	{
		_interpreter.WalkStatement(expr, context);
	}

	public void LoadScene(SceneDefinition sceneDef)
	{
		_loadedScene = sceneDef.CreateInstance(this, null!);
		_loadedScene.RunSceneLogic();
	}
	
	public bool TryGetProperty(string id, out RuntimeObject expr)
	{
		return Globals.TryGetValue(id, out expr);
	}

	//called by render method.
	public void Tick()
	{
		_loadedScene.Tick();
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