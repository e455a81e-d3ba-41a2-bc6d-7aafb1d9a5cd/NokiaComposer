module Assembler

open signal_generator
open composer_parsing 
open wave_packer

let tokenToSound  token= 
    generateSamples (durationFromToken token) (frequency token.sound)

let assemble tokens =
        List.map tokenToSound tokens |> Seq.concat

let assembleToPackedStream (score:string) = 
    match parse score with 
        | Choice1Of2 errorMsg -> Choice1Of2 errorMsg
        | Choice2Of2 tokens -> 
            assemble tokens 
            |> pack 
            |> Choice2Of2 
