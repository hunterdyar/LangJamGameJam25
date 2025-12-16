namespace LangJam;

public enum HAlignment { Left, Center, Right }
public enum VAlignment { Top, Middle, Bottom }

//todo: move all this to inside of nativecomp Grid class.
public class GridInfo
{
	public int Scale = 100;
	public int XOffset => GetAlignedXOffset();
	private int _xOff = 0;
	public int YOffset => GetAlignedYOffset();
	private int _yOff = 0;
	public int Rows = 10;
	public int Cols = 10;
	public HAlignment HAlignment = HAlignment.Left;
	public VAlignment VAlignment = VAlignment.Top;
	public GridInfo()
	{
	}

	public void SetXOffset(double xoff)
	{
		_xOff = (int)xoff;
	}

	public void SetYOffset(double yoff)
	{
		_yOff = (int)yoff;
	}

	public int GetAlignedXOffset()
	{
		switch (HAlignment)
		{
			case HAlignment.Left:
				return _xOff;
			case HAlignment.Right:
				return Raylib_cs.Raylib.GetScreenWidth() - Cols * Scale;
			case HAlignment.Center:
				return Raylib_cs.Raylib.GetScreenWidth()/2 - (Cols * Scale)/2;
		}

		return _xOff;
	}

	public int GetAlignedYOffset()
	{
		switch (VAlignment)
		{
			case VAlignment.Top:
				return _xOff;
			case VAlignment.Middle:
				return Raylib_cs.Raylib.GetScreenHeight()/2 - (Rows * Scale)/2;
			case VAlignment.Bottom:
				return Raylib_cs.Raylib.GetScreenHeight() - (Rows * Scale);
		}

		return _xOff;
	}
	public void SetHorizontalAlignment(string alignment)
	{
		switch (alignment)
		{
			case "left":
				HAlignment = HAlignment.Left;
				break;
			case "center":
				HAlignment = HAlignment.Center;
				break;
			case "right":
				HAlignment = HAlignment.Right;
				break;
			default:
				throw new Exception($"Unable to set horizontal alignment on grid. Unknown property {alignment}. Expected 'left', 'center', or 'right'");
		}
	}
	public void SetVerticalAlignment(string alignment)
	{
		switch (alignment)
		{
			case "top":
				VAlignment = VAlignment.Top;
				break;
			case "middle":
				VAlignment = VAlignment.Middle;
				break;
			case "bottom":
				VAlignment = VAlignment.Bottom;
				break;
			default:
				throw new Exception(
					$"Unable to set horizontal alignment on grid. Unknown property {alignment}. Expected 'left', 'center', or 'right'");
		}
	}
	public IEnumerator<LJPoint> EnumeratePoints()
	{
		for (int x = 0; x < Cols; x++)
		{
			for (int y = 0; y < Rows; y++)
			{
				yield return new LJPoint(x, y);
			}
		}
	}
}