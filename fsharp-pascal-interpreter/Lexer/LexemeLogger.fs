module LexemeLogger

open TokenTypes

open System.IO

let printTokens (tokens : TokenInfo list) =

    printfn "\nTokens found:"
    tokens 
    |> List.iter (fun t -> 
        printfn "Line %d, Position %d: %A (lexeme: %s)" t.Line t.Column t.Token t.Lexeme)

    let tokenGroups =
        tokens
        |> List.filter (fun t -> match t.Token with Whitespace -> false | Keyword _ -> false | Operator _ -> false | Separator _ -> false | Comment _ -> false | Unknown _ -> false | _ -> true)
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
        printfn "%-20s | %-20s | %-10d | %s" tokenTypeStr lexeme count positions
