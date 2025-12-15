namespace LangJam;

public struct GridInfo
{
	public int Scale = 100;
	public int XOffset = 0;
	public int YOffset = 0;
	public int Rows = 10;
	public int Cols = 10;

	public GridInfo()
	{
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