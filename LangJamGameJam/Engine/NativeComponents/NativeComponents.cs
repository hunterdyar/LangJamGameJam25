using System.Diagnostics.CodeAnalysis;
using LangJam;
using NativeComps;

public static class NativeComponents
{
	//todo:we can generate this dictionary through reflection on initial loading.
	private static Dictionary<string, Func<Game, Scene, ComponentBase>> Constructors =
		new Dictionary<string, Func<Game, Scene, ComponentBase>>()
		{
			{ "grid", (game, scene) => new Grid(game, scene) }
		};
	
	public static bool TryGetNativeComponent(string name, Game game, Scene scene, [NotNullWhen(true)]out ComponentBase component)
	{
		if (Constructors.TryGetValue(name, out var c))
		{
			component = c.Invoke(game, scene);
			return true;
		}

		component = null;
		return false;
	}
}