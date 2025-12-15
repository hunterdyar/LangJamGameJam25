namespace LangJam.Loader.Parser;

public struct Token
{
	public string Source => _source;
	private string _source;//should be a Span<string>
	public TokenType TokenType;
	public Token(TokenType tokenType, char c)
	{
		TokenType = tokenType;
		_source = c.ToString();
	}

	public Token(TokenType tokenType, string c)
	{
		TokenType = tokenType;
		_source = c;
	}
}