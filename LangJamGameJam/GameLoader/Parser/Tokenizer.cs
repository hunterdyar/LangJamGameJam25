using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LangJam.Loader.Parser;

public class Tokenizer
{
	public int pos = 0;
	public string Source;
	public int Length;
	private string _context;

	public string GetPrettyPos(int position)
	{
		string err = "";
		int line = 0;
		int col = 0;
		for (int i = 0; i < position; i++)
		{
			col++;
			if (Source[i] == '\n')
			{
				line++;
				col = 0;
			}
		}

		return $"line {line}, col {col}";
	}
	public List<Token> Tokenize(string contextName, string source)
	{
		_context = contextName;
		List <Token> tokens = new List<Token>();
		Source = source;
		pos = 0;
		Length = Source.Length;
		while (pos < source.Length)
		{
			if (TokenizeNext(out Token? token))
			{
				tokens.Add(token.Value);
			}
			else
			{
				if (pos >= Length)
				{
					tokens.Add(new Token(TokenType.EOF, ""));
					break;
				}
				else
				{
					throw new Exception($"unexpected tokenization error at {token} in {contextName}. {GetPrettyPos(pos)}");
				}
			}
		}

		return tokens;
	}

	private StringBuilder _valueSB = new StringBuilder();
	private bool TokenizeNext([NotNullWhen(true)]out Token? token)
	{
		ConsumeWhitespace();

		if (Consume('#'))
		{
			ConsumeUntilLinebreak();
			return TokenizeNext(out token);//quick hack to handle multiline comments.
		}

		ConsumeWhitespace();

		//pos may have advanced
		if (pos >= Length)
		{
			token = null;
			return false;
		}
		
		var c = Source[pos];
		switch (c)
		{
			case '~':
				pos++;
				token = new Token(TokenType.Bool, c);
				return true;
			case '|':
				pos++;
				token = new Token(TokenType.Numeric, c);
				return true;
			case '!':
				pos++;
				token = new Token(TokenType.Point,c);
				return true;
			case '[':
				pos++;
				token = new Token(TokenType.OpenList, c);
				return true;
			case ']':
				pos++;
				token = new Token(TokenType.CloseList,c);
				return true;
			case '(':
				pos++;
				token = new Token(TokenType.OpenParen, c);
				return true;
			case ')':
				pos++;
				token = new Token(TokenType.CloseParen, c);
				return true;
			case '{':
				pos++;
				token = new Token(TokenType.OpenDeclare, c);
				return true;
			case '}':
				pos++;
				token = new Token(TokenType.CloseDeclare, c);
				return true;
			case '"':
				pos++;//consume opening "
				var data = ConsumeUntil('"');
				Consume('"');
				token = new Token(TokenType.String, data);
				return true;
			case ':':
				pos++;
				var ident = ConsumeValue();
				token = new Token(TokenType.Identifier, ident);
				return true;
		}

		if (char.IsWhiteSpace(Source[pos]))
		{
			Console.WriteLine("huh?");
		}
		if (pos >= Length)
		{
			token = null;
			return false;
		}

		var value = ConsumeValue();
		token = new Token(TokenType.Key, value);
		if (value.Length == 0)
		{
			return false;
		}
		return true;
	}

	private string ConsumeValue()
	{
		_valueSB.Clear();
		while (IsValidValueCharacter())
		{
			_valueSB.Append(Source[pos]);
			pos++;
		}

		return _valueSB.ToString();
	}

	private StringBuilder consumeUntilSB = new StringBuilder();
	private string ConsumeUntil(char c)
	{
		consumeUntilSB.Clear();
		while (Source[pos] != c)
		{
			consumeUntilSB.Append(c);
			pos++;
			if (pos >= Length)
			{
				throw new Exception($"unexpected end of file. in {_context}. missing {c}. {GetPrettyPos(pos)}");
			}
		}

		return consumeUntilSB.ToString();
	}

	private bool IsValidValueCharacter()
	{
		if (pos >= Length)
		{
			return false;
		}
		var c = Source[pos];
		return char.IsAsciiLetterOrDigit(c) || c == '-' || c == '_' || c == ':';
	}

	private void ConsumeWhitespace()
	{
		while(pos < Length)
		{
			if (char.IsWhiteSpace(Source[pos]))
			{
				pos++;
			}
			else
			{
				break;
			}
		}
	}
	private bool Consume(char c)
	{
		if (pos >= Length)
		{
			return false;
		}
		
		if (Source[pos] == c)
		{
			pos++;
			return true;
		}

		return false;
	}
	private void ConsumeUntilLinebreak()
	{
		while (pos < Length && Source[pos] != '\n')
		{
			pos++;
		}

		pos++;
	}
}