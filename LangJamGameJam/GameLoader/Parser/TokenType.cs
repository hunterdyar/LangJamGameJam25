namespace LangJam.Loader.Parser;

public enum TokenType
{
	Bool,
	Numeric,
	Point,
	OpenGroup,
	CloseGroup,
	OpenParen,
	CloseParen,
	OpenDeclare,
	CloseDeclare,
	String,
	Identifier,
	Key,
	Symbol,
	EOF,
}