module Program

open TokenTypes
open Lexer
open LexerValidators
open LexemeLogger

open System.IO

[<EntryPoint>]
let main args =
    if args.Length = 0 then
        printf "Specify the file name: dotnet run -- <filename>"
        -1
    else
        for arg in args do
            let mutable tokens : TokenInfo list = []

            let saveTokens (fileName: string) : unit =
                if not (File.Exists(fileName)) then
                    printfn "Файл '%s' не найден." fileName
                else
                    let code = File.ReadAllText(fileName)
                    tokens <- tokenize code  
                    validateTokens tokens
                    
            printTokens tokens
        0