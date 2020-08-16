// Learn more about F# at http://fsharp.org

open FSharp.Data
open FSharp.Data.CsvExtensions
open System
open System.Collections.Generic

type Declensions = CsvProvider<"./DeclensionTable.csv">


let makeDictionary (table: CsvFile) =  
      
    let dict = new Dictionary<Tuple<String, String>, String>()
    let headers = table.Headers.Value
    //TODO can this be done more functional? With mapping?
    for row in table.Rows do
        for header in headers do
            if not ("Form".Equals header) then
                dict.Add(
                    (row.GetColumn("Form"), header)
                    , row.GetColumn(header))     
    
    dict



[<EntryPoint>]
let main argv =
    let table = CsvFile.Load("../../../DeclensionTable.csv")
    let dict = makeDictionary(table)
    let rnd = System.Random()
    let keyList = new List<Tuple<String, String>>(dict.Keys)
    let mutable running = true
    while running do
        let nextVal = rnd.Next(dict.Count)
        let question = keyList.[nextVal]
        let printKey = function
            | (a, b) -> printfn "%s - %s" a b
        printKey question

        let input = System.Console.ReadLine().ToLower()
        running <- not(String.IsNullOrWhiteSpace input)
        printfn "%b - %s\n-----" (input.Equals dict.[question]) dict.[question]
    0 // return an integer exit code
