using Raylib_cs;

public static class Utilities
{
	public static Color StringToColor(string cname)
	{
		switch (cname.ToLower())
		{
			case "black":
				return Color.Black;
			case "white":
				return Color.White;
			case "green":
				return Color.Green;
			case "blue":
				return Color.Blue;
			case "red":
				return Color.Red;
			case "gray":
			case "grey":
				return Color.Gray;
			default:
				return Color.Magenta;
		}
	}
}