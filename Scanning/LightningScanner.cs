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

        var (type, lexeme, literal, offset) = head switch
        {
            ' ' => (TokenType.Whitespace, "< >", null, 1),
            '-' => (TokenType.Minus, "-", null, 1),
            ',' => (TokenType.Comma, ",", null, 1),
            ';' => (TokenType.Semicolon, ";", null, 1),
            '!'
                => neck == '='
                    ? (TokenType.BangEqual, "!=", null, 2)
                    : (TokenType.Bang, "!", null, 1),
            '.' => (TokenType.Dot, ".", null, 1),
            '(' => (TokenType.LeftParen, "(", null, 1),
            ')' => (TokenType.RightParen, ")", null, 1),
            '{' => (TokenType.LeftBrace, "{", null, 1),
            '}' => (TokenType.RightBrace, "}", null, 1),
            '*' => (TokenType.Star, "*", null, 1),
            '/' => (TokenType.Slash, "/", null, 1),
            '\"' => StringToken(source),
            '\0' => (TokenType.Eof, "EOF", null, 1),
            '\n' => (TokenType.Newline, "<\\n>", null, 1),
            '\r' => (TokenType.Whitespace, "<\\r>", null, 1),
            '\t' => (TokenType.Whitespace, "<\\t>", null, 1),
            '+' => (TokenType.Plus, "+", null, 1),
            '<'
                => neck == '='
                    ? (TokenType.LessEqual, "<=", null, 2)
                    : (TokenType.Less, "<", null, 1),
            '='
                => neck == '='
                    ? (TokenType.EqualEqual, "==", null, 2)
                    : (TokenType.Bang, "=", null, 1),
            '>'
                => neck == '='
                    ? (TokenType.GreaterEqual, ">=", null, 2)
                    : (TokenType.Greater, ">", null, 1),
            _ when char.IsDigit(head) => NumberToken(source),
            _ => (TokenType.Error, "Unknown LightningScanner error", null, 1),
        };

        tokens.Add(new Token(type, lexeme, literal, line));

        switch (type)
        {
            case TokenType.Eof:
                break;
            case TokenType.Newline:
                ScanToken(tokens, source[offset..], line + 1);
                break;
            default:
                ScanToken(tokens, source[offset..], line);
                break;
        }
    }

    private (TokenType, string, object, int) StringToken(ReadOnlySpan<char> source)
    {
        var end = source[1..].IndexOf('"') + 1;
        var value = source[1..end].ToString();

        return (TokenType.String, value, value, value.Length + 2);
    }

    private (TokenType, string, object, int) NumberToken(ReadOnlySpan<char> source)
    {
        var result = 0;
        var isFloating = false;

        foreach (var c in source)
        {
            if (char.IsDigit(c))
            {
                result++;
            }
            else if (c == '.')
            {
                result++;
                if (isFloating)
                {
                    break;
                }
                isFloating = true;
            }
            else
            {
                break;
            }
        }

        var numberLiteral = new string(source[0..result]);
        var numberType = isFloating ? TokenType.NumberFloat : TokenType.NumberFixed;
        var number = isFloating ? float.Parse(numberLiteral) : int.Parse(numberLiteral);

        return (numberType, numberLiteral, number, numberLiteral.Length);
    }
}
