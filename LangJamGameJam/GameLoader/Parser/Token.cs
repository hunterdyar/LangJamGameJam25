namespace LangJam.Loader.Parser;

public struct Token
{
	public string Source => _source;
	private string _source;//should be a Span<string>
	public TokenType TokenType;
	public readonly int Position;
	public Token(TokenType tokenType, int position, char c)
	{
		Position = position;
		TokenType = tokenType;
		_source = c.ToString();
	}

	public Token(TokenType tokenType, string c)
	{
		TokenType = tokenType;
		_source = c;
	}

	public override string ToString()
	{
		return _source.ToString();
	}
}