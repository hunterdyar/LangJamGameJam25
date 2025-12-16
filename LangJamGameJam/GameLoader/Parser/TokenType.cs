namespace LangJam.Loader.Parser;

public enum TokenType
{
	Bool,
	Numeric,
	Point,
	OpenList,
	CloseList,
	OpenParen,
	CloseParen,
	OpenDeclare,
	CloseDeclare,
	String,
	Identifier,
	Key,
	EOF,
}