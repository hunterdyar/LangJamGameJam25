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

		List<FileInfo> entityFiles = new List<FileInfo>();
		List<FileInfo> componentsFiles = new List<FileInfo>();
		List<FileInfo> sceneFiles = new List<FileInfo>();
		List<FileInfo> spriteFiles = new List<FileInfo>();
		//other resources?
		
		foreach (var dirInfo in gameDir.EnumerateDirectories("*", SearchOption.AllDirectories))
		{
			if (dirInfo.Name == "entities")
			{
				foreach (var info in dirInfo.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly))
				{
					entityFiles.Add(info);
				}
			}else if (dirInfo.Name == "components")
			{
				foreach (var info in dirInfo.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly))
				{
					componentsFiles.Add(info);
				}
			}else if (dirInfo.Name == "scenes")
			{
				foreach (var info in dirInfo.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly))
				{
					sceneFiles.Add(info);
				}
			}else if (dirInfo.Name == "sprites")
			{
				foreach (var info in dirInfo.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly))
				{
					spriteFiles.Add(info);
				}
			}
		}

		Game game = new Game();
		game.SetComponentDefinitions(LoadComponents(componentsFiles));
		game.SetSceneDefinitions(LoadSceneDefinitions(sceneFiles));
		game.SetSprites(LoadSprites(spriteFiles));
		if (game.SceneDefinitions.TryGetValue("main", out var mainScene))
		{
			game.LoadScene(mainScene);
		}
		else
		{
			throw new Exception("no main scene set. There must be a scene called 'main'");
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

	private static Dictionary<string, Sprite> LoadSprites(List<FileInfo> spriteFiles)
	{
		Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
		foreach (var fileInfo in spriteFiles)
		{
			if (fileInfo.Extension.ToLower() != ".png")
			{
				Console.WriteLine($"unable to load sprite {fileInfo.Name}");
				continue;
			}

			var sName = Path.GetFileNameWithoutExtension(fileInfo.Name).ToLower();
			var tex = Raylib_cs.Raylib.LoadTexture(fileInfo.FullName);
			sprites.Add(sName, new Sprite(sName, tex));
		}

		return sprites;
	}

	private static Dictionary<string, SceneDefinition> LoadSceneDefinitions(List<FileInfo> entityFiles)
	{
		Dictionary<string, SceneDefinition> entityDefs = new Dictionary<string, SceneDefinition>();
		foreach (var fileInfo in entityFiles)
		{
			var contents = fileInfo.OpenText().ReadToEnd();
			var p = new Parser.Parser();
			p.Parse(fileInfo.Name, contents);
			var eName = Path.GetFileNameWithoutExtension(fileInfo.Name).ToLower();
			
			//todo: comp list is special?
			var comps = p.RootExpressions.Cast<SExpr>().Where(x => x.Key != null && x.Key.Value == "components").ToArray();
			List<string> compNames = new List<string>();
			if (comps.Any())
			{
				foreach (var compExpression in comps)
				{
					for (var i = 1; i < compExpression.elements.Count; i++)//start at 1 to skip "components"
					{
						var comp = compExpression.elements[i];
						var c = comp.ToString();
						if (!string.IsNullOrEmpty(c) )
						{
							compNames.Add(c); //should this be comp
						}
					}
				}
			}

			//components are special, so remove this list!
			foreach (var compExpr in comps)
			{
				p.RootExpressions.Remove(compExpr);
			}
			entityDefs.Add(eName, new SceneDefinition(eName, p.RootExpressions, compNames));
		}

		return entityDefs;
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
			comps.Add(compName, new ComponentDefinition(compName, p.RootExpressions));
		}

		return comps;
	}
}