using LangJam.Loader;
using Raylib_cs;

namespace HelloWorld;

internal static class Program
{
	[System.STAThread]
	public static void Main(string[] args)
	{
		string gameDir = Environment.CurrentDirectory + "\\game";
		if (args.Length > 0)
		{
			gameDir = args[0];
		}

		DirectoryInfo source = new DirectoryInfo(gameDir);
		Raylib.InitWindow(800, 480, gameDir);

		var game = GameLoader.LoadGame(source);
		
		while (!Raylib.WindowShouldClose())
		{
			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.White);//game.loadedscene.backgroundColor

			// Raylib.DrawText("Hello, world!", 12, 12, 20, Color.Black);
			game.Tick();

			//Raylib.DrawFPS(10, 10);
			Raylib.EndDrawing();
		}

		Raylib.CloseWindow();
	}
}