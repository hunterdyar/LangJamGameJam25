using LangJam.Loader.AST;

namespace LangJam.Loader;

public static class GameLoader
{
	public static Game LoadGame(DirectoryInfo gameDir)
	{
		if (!gameDir.Exists)
		{
			throw new Exception($"Can't load game, directory {gameDir.Name} does nto exist");
		}

		List<FileInfo> _entityFiles = new List<FileInfo>();
		List<FileInfo> _componentsFiles = new List<FileInfo>();
		List<FileInfo> _sceneFiles = new List<FileInfo>();
		List<FileInfo> _spriteFiles = new List<FileInfo>();
		//other resources?
		
		foreach (var dirInfo in gameDir.EnumerateDirectories("*", SearchOption.AllDirectories))
		{
			if (dirInfo.Name == "entities")
			{
				foreach (var info in dirInfo.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly))
				{
					_entityFiles.Add(info);
				}
			}else if (dirInfo.Name == "components")
			{
				foreach (var info in dirInfo.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly))
				{
					_componentsFiles.Add(info);
				}
			}else if (dirInfo.Name == "scenes")
			{
				foreach (var info in dirInfo.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly))
				{
					_sceneFiles.Add(info);
				}
			}else if (dirInfo.Name == "sprites")
			{
				foreach (var info in dirInfo.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly))
				{
					_spriteFiles.Add(info);
				}
			}
		}

		Game game = new Game();
		game.SetComponentDefinitions(LoadComponents(_componentsFiles));
		game.SetSceneDefinitions(LoadScenes(_sceneFiles));
		game.SetEntityDefinitions(LoadEntities(_entityFiles));
		if (game.SceneDefinitions.TryGetValue("main", out var mainScene))
		{
			game.LoadScene(mainScene);
		}
		else
		{
			Console.WriteLine("No main scene found. Loading empty scene.");
			game.LoadScene(new SceneDefinition(new List<Expr>()));
		}
		//find the main scene.
		//top-level folders are organizing.
			//entities
				//entityName.txt
			//components
				//code.lispish
			//scenes
			//sprites
				//subfolders turned lowercase and into sprite name with hyphen
		//top-level files are data
		//game.lj set width/heights/etc.
		
		return game;
	}

	private static Dictionary<string, EntityDefinition> LoadEntities(List<FileInfo> entityFiles)
	{
		Dictionary<string, EntityDefinition> entities = new Dictionary<string, EntityDefinition>();
		foreach (var fileInfo in entityFiles)
		{
			var contents = fileInfo.OpenText().ReadToEnd();
			var p = new Parser.Parser();
			p.Parse(fileInfo.Name, contents);
			var eName = Path.GetFileNameWithoutExtension(fileInfo.Name).ToLower();
			//todo: comp list is special?
			entities.Add(eName, new EntityDefinition(eName, p.RootExpressions, new List<string>()));
		}

		return entities;
	}

	private static Dictionary<string, SceneDefinition> LoadScenes(List<FileInfo> sceneFiles)
	{
		Dictionary<string, SceneDefinition> comps = new Dictionary<string, SceneDefinition>();
		foreach (var fileInfo in sceneFiles)
		{
			var contents = fileInfo.OpenText().ReadToEnd();
			var p = new Parser.Parser();
			p.Parse(fileInfo.Name, contents);
			var compName = Path.GetFileNameWithoutExtension(fileInfo.Name).ToLower();
			comps.Add(compName, new SceneDefinition(p.RootExpressions));
		}

		return comps;
	}

	private static Dictionary<string, ComponentDefinition> LoadComponents(List<FileInfo> componentsFiles)
	{
		Dictionary<string, ComponentDefinition> comps = new Dictionary<string, ComponentDefinition>();
		foreach (var fileInfo in componentsFiles)
		{
			var contents = fileInfo.OpenText().ReadToEnd();
			var p = new Parser.Parser();
			p.Parse(fileInfo.Name, contents);
			var compName = Path.GetFileNameWithoutExtension(fileInfo.Name).ToLower();
			comps.Add(compName, new ComponentDefinition(p.RootExpressions));
		}

		return comps;
	}
}