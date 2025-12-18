namespace LangJam;

public abstract class NativeComponent : ComponentBase
{
	public NativeComponent(string name, Game game, Scene scene) : base(name, game, scene)
	{
		//this is so fucken hacky i hate it...
	}

	public override void CallRender()
	{
		base.CallRender();
	}
	
	//list of declared functions, except we call our override special functions
	public override string ToString()
	{
		return $"NativeComponent({Name()})";
	}
}