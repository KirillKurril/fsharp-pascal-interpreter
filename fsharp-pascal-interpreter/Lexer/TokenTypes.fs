module TokenTypes

type Token =
    | Comment of string
    | Keyword of string
    | Identifier of string
    | Number of string
    | StringLiteral of string
    | CharLiteral of string
    | Operator of string
    | Separator of string
    | Whitespace
    | Unknown of string

type TokenInfo = {
    Token: Token
    Lexeme: string
    Line: int
    Column: int
}