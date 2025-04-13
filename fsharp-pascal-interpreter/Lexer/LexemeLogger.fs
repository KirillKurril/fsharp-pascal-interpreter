module LexemeLogger

open TokenTypes

open System.IO

let printTokens (tokens : TokenInfo list) =
    let cleanStr (s : string) = s.Replace("\n", " ").Replace("\r", " ")

    printfn "\nTokens found:"
    tokens 
    |> List.iter (fun t -> 
        let tokenStr = 
            match t.Token with
            | Comment s -> $"Comment({cleanStr s})"
            | Keyword s -> $"Keyword({cleanStr s})"
            | Identifier s -> $"Identifier({cleanStr s})"
            | Number s -> $"Number({cleanStr s})"
            | StringLiteral s -> $"StringLiteral({cleanStr s})"
            | CharLiteral s -> $"CharLiteral({cleanStr s})"
            | Operator s -> $"Operator({cleanStr s})"
            | Separator s -> $"Separator({cleanStr s})"
            | Unknown s -> $"Unknown({cleanStr s})"
            | Whitespace -> "Whitespace"
        printfn "Line %d, Position %d: %s (lexeme: %s)" t.Line t.Column tokenStr (cleanStr t.Lexeme))

    let tokenGroups =
        tokens
        |> List.filter (fun t -> 
            match t.Token with 
            | Whitespace -> false 
            | Keyword _ -> true 
            | Operator _ -> true 
            | Separator _ -> true 
            | Comment _ -> true 
            | Unknown _ -> true 
            | _ -> true)
        |> List.groupBy (fun t -> (t.Token, t.Lexeme))
        |> List.sortBy (fun ((token, _lexeme), _) -> token)

    printfn "\nTable of tokens:"
    printfn "%-20s | %-20s | %-10s | %s" "Type" "Lexeme" "Quantity" "Positions"
    printfn "%s" (String.replicate 80 "-")
    for ((token, lexeme), group) in tokenGroups do
        let count = group.Length
        let positions = group |> List.map (fun t -> sprintf "(%d, %d)" t.Line t.Column) |> String.concat ", "
        let tokenTypeStr =
            match token with
            | Keyword _ -> "Keyword"
            | Identifier _ -> "Identifier"
            | Number _ -> "Number"
            | StringLiteral _ -> "StringLiteral"
            | CharLiteral _ -> "CharLiteral"
            | Operator _ -> "Operator"
            | Separator _ -> "Separator"
            | Comment _ -> "Comment"
            | Unknown _ -> "Unknown"
            | Whitespace -> "Whitespace"
        printfn "%-20s | %-20s | %-10d | %s" tokenTypeStr (cleanStr lexeme) count positions