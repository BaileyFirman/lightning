namespace Lightning.Scanning;

public class LightningScanner
{
    public static IDictionary<string, TokenType> Keywords = new Dictionary<string, TokenType>
    {
        ["and"] = TokenType.And,
        ["class"] = TokenType.Class,
        ["else"] = TokenType.Else,
        ["false"] = TokenType.False,
        ["for"] = TokenType.For,
        ["fun"] = TokenType.Fun,
        ["if"] = TokenType.If,
        ["nil"] = TokenType.Nil,
        ["or"] = TokenType.Or,
        ["print"] = TokenType.Print,
        ["return"] = TokenType.Return,
        ["super"] = TokenType.Super,
        ["this"] = TokenType.This,
        ["true"] = TokenType.True,
        ["var"] = TokenType.Var,
        ["while"] = TokenType.While,
    };

    public LightningScanner() { }

    public IEnumerable<Token> ScanTokens(string file)
    {
        // Our base case is the files eof, this is to prevent indexing errors
        var source = file + "\0\0";
        var tokens = new List<Token>();
        var line = 1;

        ScanToken(tokens, source, line);

        return tokens;
    }

    public void ScanToken(List<Token> tokens, ReadOnlySpan<char> source, int line)
    {
        var head = source[0];
        var neck = source[1];

        var (type, literal, offset) = head switch
        {
            ' ' => (TokenType.Whitespace, "< >", 1),
            '-' => (TokenType.Minus, "-", 1),
            ',' => (TokenType.Comma, ",", 1),
            ';' => (TokenType.Semicolon, ";", 1),
            '!' => neck == '=' ? (TokenType.BangEqual, "!=", 2) : (TokenType.Bang, "!", 1),
            '.' => (TokenType.Dot, ".", 1),
            '(' => (TokenType.LeftParen, "(", 1),
            ')' => (TokenType.RightParen, ")", 1),
            '{' => (TokenType.LeftBrace, "{", 1),
            '}' => (TokenType.RightBrace, "}", 1),
            '*' => (TokenType.Star, "*", 1),
            '/' => (TokenType.Slash, "/", 1),
            '\"' => StringToken(source),
            '\0' => (TokenType.Eof, "EOF", 1),
            '\n' => (TokenType.Newline, "<\\n>", 1),
            '\r' => (TokenType.Whitespace, "<\\r>", 1),
            '\t' => (TokenType.Whitespace, "<\\t>", 1),
            '+' => (TokenType.Plus, "+", 1),
            '<' => neck == '=' ? (TokenType.LessEqual, "<=", 2) : (TokenType.Less, "<", 1),
            '=' => neck == '=' ? (TokenType.EqualEqual, "==", 2) : (TokenType.Bang, "=", 1),
            '>' => neck == '=' ? (TokenType.GreaterEqual, ">=", 2) : (TokenType.Greater, ">", 1),
            _ => (TokenType.Error, "Unknown LightningScanner error", 1),
        };

        var token = new Token(type, literal, literal, line);

        switch (type)
        {
            case TokenType.Eof:
                tokens.Add(token);
                break;
            default:
                tokens.Add(token);
                ScanToken(tokens, source[offset..], line + 1);
                break;
        }
    }

    private (TokenType, string, int) StringToken(ReadOnlySpan<char> source)
    {
        var end = source[1..].IndexOf('"') + 1;
        var value = source[1..end].ToString();

        return (TokenType.String, value, value.Length + 2);
    }
}
