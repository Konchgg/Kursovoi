using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_work
{
    public enum TokenType
    {
        INT, CHAR, STRING, MULTIPLY, DIVIDE, OR, DEGREE, LCURBRACE,
        MAIN, FOR, EXPR, PLUS, DOUBLE_PLUS, MINUS, DOUBLE_MINUS,
        RCURBRACE, EQUAL, MORE, LESS, COMMA, DOT, COLON, SEMICOLON,
        LPAR, RPAR, UNDERSCORE, LINEBREAK, ID, LIT, NETERM
    }
    public class Token
    {
        public TokenType Type;
        public string Value;
        public string DCR;
        public Token(TokenType type)
        {
            Type = type;
        }
        public override string ToString()
        {
            return string.Format("{0}, {1}", Type, Value);

        }
        static TokenType[] Delimiters = new TokenType[]
        {
            TokenType.PLUS, TokenType.DOUBLE_PLUS, TokenType.MINUS, TokenType.DOUBLE_MINUS, TokenType.MULTIPLY,
            TokenType.DIVIDE, TokenType.OR, TokenType.DEGREE, TokenType.LCURBRACE, TokenType.RCURBRACE,
            TokenType.EQUAL, TokenType.MORE, TokenType.LESS,
            TokenType.COMMA, TokenType.DOT, TokenType.COLON,
            TokenType.SEMICOLON, TokenType.LPAR, TokenType.RPAR,
            TokenType.UNDERSCORE, TokenType.LINEBREAK
        };
        public static bool IsDelimiter(Token token)
        {
            return Delimiters.Contains(token.Type);
        }
        public static Dictionary<string, TokenType> SW = new Dictionary<string, TokenType>() {
            {"int", TokenType.INT},
            {"char", TokenType.CHAR},
            {"string", TokenType.STRING},
            {"main", TokenType.MAIN},
            {"for", TokenType.FOR},
            {"expr", TokenType.EXPR},
            {"++", TokenType.DOUBLE_PLUS}
        };
        public static bool IsSpecialWord(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return false;
            }
            return (SW.ContainsKey(word));
        }
        public static Dictionary<char, TokenType> SS = new Dictionary<char, TokenType>()
        {
            {'-', TokenType.MINUS},
            {'{', TokenType.LCURBRACE},
            {'}', TokenType.RCURBRACE},
            {'+', TokenType.PLUS},
            {'*', TokenType.MULTIPLY},
            {'/', TokenType.DIVIDE},
            {'|', TokenType.OR},
            {'^', TokenType.DEGREE},
            {'=', TokenType.EQUAL},
            {'>', TokenType.MORE},
            {'<', TokenType.LESS},
            {',', TokenType.COMMA},
            {'.', TokenType.DOT},
            {':', TokenType.COLON},
            {';', TokenType.SEMICOLON},
            {'(', TokenType.LPAR},
            {')', TokenType.RPAR},
            {'_', TokenType.UNDERSCORE},
            {'\n',TokenType.LINEBREAK}
        };
        public static bool IsSpecialSymbol(char ch)
        {
            return SS.ContainsKey(ch);
        }
    }
}