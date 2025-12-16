using LangJam.Loader.AST;

namespace LangJam.Loader.Parser;

public class Parser
{
	public List<AST.Expr> RootExpressions = new List<AST.Expr>();
	private Queue<Token> _tokens;
	private string _context;
	public void Parse(string contextName, string source)
	{
		_context = contextName;
		var t = new Tokenizer();
		var tokens = t.Tokenize(contextName, source); 
		_tokens = new Queue<Token>(tokens);

		while (_tokens.Count > 0)
		{
			if (_tokens.Peek().TokenType == TokenType.EOF)
			{
				break;
			}
			RootExpressions.Add(ParseExpression());
		}
	}

	private AST.Expr ParseExpression()
	{
		var top = _tokens.Dequeue();
		switch (top.TokenType)
		{
			case TokenType.OpenParen:
				return ParseSExpr();
			case TokenType.OpenDeclare:
				return ParseDecExpr();
			case TokenType.Identifier:
				if (double.TryParse(top.Source, out var val))
				{
					return new NumberConstant(val);
				}
				return new IdentifierConstant(top.Source);
			case TokenType.Key:
				if (double.TryParse(top.Source, out var n))
				{
					return new NumberConstant(n);
				}
				return new KeyExpr(top.Source);
			case TokenType.String:
				return new StringConstant(top.Source);
			case TokenType.Point:
				return ParsePoint();
			case TokenType.Numeric:
				throw new NotImplementedException();
			case TokenType.Bool:
				return ParseBool();
			case TokenType.EOF:
				throw new Exception($"Unexpected end of file in {_context}.");
			
		}

		throw new Exception($"Unexpected token {top} in {_context}");
	}

	private Expr ParseBool()
	{
		var e = ParseExpression();
		return new BooleanExpr(e);
	}

	private Expr ParsePoint()
	{
		var X = ParseExpression();
		var y = ParseExpression();
		return new Point(X, y);
	}

	private AST.Expr ParseSExpr()
	{
		List<AST.Expr> expressions = new List<AST.Expr>();
		while (_tokens.Peek().TokenType != TokenType.CloseParen)
		{
			expressions.Add(ParseExpression());
		}
		Consume(TokenType.CloseParen);
		return new SExpr(expressions);
	}

	private AST.Expr ParseDecExpr()
	{
		List<AST.Expr> expressions = new List<AST.Expr>();
		var id = _tokens.Dequeue();
		while (_tokens.Peek().TokenType != TokenType.CloseDeclare)
		{
			expressions.Add(ParseExpression());
		}

		Consume(TokenType.CloseDeclare);
		return new DeclareExpr(id.Source,expressions);
	}

	private void Consume(TokenType expected)
	{
		if (_tokens.TryPeek(out var got))
		{
			if (got.TokenType == expected)
			{
				_tokens.Dequeue();
				return;
			}
		}

		throw new Exception($"Unexpected value or EOF, expected {expected}");
	}
}