namespace Lightning.Scanning;

public class Token
{
    public int Line { get; set; }
    public object? Literal { get; set; }
    public string? Lexeme { get; set; }
    public TokenType Type { get; set; }

    public Token(TokenType type, string? lexeme, object? literal, int line)
    {
        Lexeme = lexeme;
        Line = line;
        Literal = literal;
        Type = type;
    }

    public override string ToString() =>
        $"{Type, -16} | {Lexeme, -32} | {Literal, -32} | {Line, -16}";
}
