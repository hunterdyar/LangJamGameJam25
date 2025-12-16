using Raylib_cs;

namespace LangJam;

public class Sprite
{
	//https://www.raylib.com/examples.html
	//todo: contain sprite animation details
	//https://github.com/raysan5/raylib/blob/master/examples/textures/textures_sprite_animation.c
	public Texture2D Texture;
	public Color Tint;
	private string _name;
	public Sprite(string sName, Texture2D texture)
	{
		_name = sName;
		Texture = texture;
		Tint = Color.White;
	}

	public void Draw(int px, int py)
	{
		Raylib.DrawTexture(Texture, px, py, Tint);
	}

	public override string ToString()
	{
		return $"Sprite({_name})";
	}
}