// Learn more about F# at http://fsharp.org

open System
open Printer
open Assembler
open wave_packer
open composer_parsing
open System.IO

let write fileName (ms:MemoryStream) =
    use fs = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName), FileMode.Create)
    ms.WriteTo(fs)

let extractChoice2 v =
    match v with 
    | Choice1Of2 s -> sprintf "unexpected choice value %A" s |> failwith
    | Choice2Of2 s -> s

[<EntryPoint>]
let main argv =
    parse "4g2 8.#a2 16g2 16- 16g2 8c3 8g2 8f2 4g2 8.d3 16g2 16- 16g2 8#d3 8d3 8#a2 8g2 8d3 8g3 16g2 16f2 16- 16f2 8d2 8a2 2g2" 
    |> extractChoice2 
    |> assemble 
    |> pack
    |> write "test.wav"
    0 // return an integer exit code
