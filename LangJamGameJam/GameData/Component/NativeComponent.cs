namespace LangJam;

public class NativeComponent : ComponentBase
{
	public NativeComponent(Game game, Scene scene) : base(game, scene)
	{
		
	}

	public override void CallRender()
	{
		base.CallRender();
	}
	
	//list of declared functions, except we call our override special functions
}