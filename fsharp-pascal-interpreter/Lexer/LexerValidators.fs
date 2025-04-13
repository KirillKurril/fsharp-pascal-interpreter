module LexerValidators

open TokenTypes

let validateTokens (tokens: TokenInfo list) =
    let errors = tokens |> List.filter (fun t -> match t.Token with Unknown _ -> true | _ -> false)

    if errors.Length > 0 then
        printfn "\nLexical analysis errors detected:"
        errors |> List.iter (fun e -> 
            printfn "Line %d, Position %d: %A (Lexeme: %s)" e.Line e.Column e.Token e.Lexeme)
    else
        printfn "\nNo lexical analysis errors found."