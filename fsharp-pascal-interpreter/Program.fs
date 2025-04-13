module Program

open TokenTypes
open Lexer
open LexerValidators
open LexemeLogger

open System.IO

////
[<EntryPoint>]
let main args =
    let fileName = @"D:\uni\MTRAN\fsharp-pascal-interpreter\fsharp-pascal-interpreter\test.txt";

    let mutable tokens : TokenInfo list = []

    let saveTokens (fileName: string) : unit =
        if not (File.Exists(fileName)) then
            printfn "File '%s' does not found." fileName
        else
            let code = File.ReadAllText(fileName)
            tokens <- tokenize code  
            validateTokens tokens
    
    saveTokens fileName
    
    printTokens tokens
    0
////

//[<EntryPoint>]
//let main args =
//    if args.Length = 0 then
//        printf "Specify the file name: dotnet run -- <filename>"
//        -1
//    else
//        for arg in args do
//            let mutable tokens : TokenInfo list = []

//            let saveTokens (fileName: string) : unit =
//                if not (File.Exists(fileName)) then
//                    printfn "File '%s' does not found." fileName
//                else
//                    let code = File.ReadAllText(fileName)
//                    tokens <- tokenize code  
//                    validateTokens tokens
            
//            saveTokens arg
            
//            printTokens tokens
//        0