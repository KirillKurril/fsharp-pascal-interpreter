module TokenDefs

open TokenTypes;

let tokenDefs : (string * string * (string -> Token)) list = [
    ("Comment", @"^\{(.*?)(\*\)|\})|^\(\*(.*?)(\*\)|\})", fun lex ->
        if lex.Contains("(*)") then 
            Unknown "Invalid comment syntax: (*) sequence forbidden"
        else Comment lex);

    ("Keyword", @"\b(?i:begin|end|if|then|else|while|do|for|to|downto|var|const|procedure|function|program|integer|real|char|boolean|div|mod|and|or|not|array|case|goto|in|label|record|repeat|of|packed|set|type|until|with)\b", 
        fun lex -> Keyword (lex.ToLower()));

    ("InvalidIdentifier", "^[0-9][a-zA-Z0-9]*", fun _ -> 
        Unknown "Identifier cannot start with a digit");

    ("Identifier", "^[a-zA-Z][a-zA-Z0-9]*", fun lex -> Identifier lex);

    ("Number", @"^(0|([1-9]\d*))(\.\d+)?([eE][+-]?\d+)?$", fun lex ->
        if lex.StartsWith('0') && lex.Length > 1 && not (lex.Contains('.')) then 
            Unknown "Leading zeros forbidden in integers"
        else 
            Number lex);

     ("StringOrCharLiteral", "^'(''|[^'\n\r])*'", fun lex ->
            if not (lex.EndsWith("'")) then 
                Unknown "Unclosed string literal"
            else
                let content = lex.[1..lex.Length-2].Replace("''", "'")
                match content.Length with
                | 0 -> StringLiteral "" 
                | 1 -> CharLiteral content 
                | _ -> StringLiteral content 
        );

    ("Operator", "^(:=|<=|>=|<>|[+\\-*/=<>])", fun lex -> Operator lex);

    ("Separator", "^[.,;:()\\[\\]]", fun lex -> Separator lex);

    ("Whitespace", "^[ \t\r\n]+", fun _ -> Whitespace)
]