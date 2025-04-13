module Lexer

open TokenTypes
open TokenDefs

open System
open System.Text.RegularExpressions

let tokenize (code: string) =
    let rec aux line column pos tokens =
        if pos >= code.Length then List.rev tokens
        else
            let substring = code.Substring(pos)
            let matched =
                tokenDefs
                |> List.tryPick (fun (_name, pattern, constructor) ->
                    let regex = Regex("^" + pattern)
                    let m = regex.Match(substring)
                    if m.Success then Some (m.Value, constructor) else None)
            match matched with
            | Some (lexeme, constructor) ->
                let token = constructor lexeme
                let currentLine = line
                let currentColumn = column
                let tokenInfo = {Token = token; Lexeme = lexeme; Line = currentLine; Column = currentColumn }

                let newLine, newColumn =
                    if lexeme.Contains("\n") then
                        let newLinesCount = lexeme.Split([|'\n'|], StringSplitOptions.None).Length - 1
                        line + newLinesCount, lexeme.Length - lexeme.LastIndexOf('\n') - 1
                    else
                        line, column + lexeme.Length

                let newTokens = match token with Whitespace -> tokens | _ -> tokenInfo :: tokens
                aux newLine newColumn (pos + lexeme.Length) newTokens
            | None ->
                let unknownChar = code.[pos].ToString()
                let tokenInfo = { Token = Unknown unknownChar; Lexeme = unknownChar; Line = line; Column = column }
                aux line (column + 1) (pos + 1) (tokenInfo :: tokens)
    aux 1 1 0 []