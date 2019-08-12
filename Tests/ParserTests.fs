namespace ParserTests

open Xunit
open FsUnit.Xunit
open composer_parsing

module ``When parsing a score``  =


    [<Fact>]
    let ``it should parse a simple score`` () =
        let score = "32.#d3 16-"
        let result = parse score

        let assertFirstToken token=
            token.length |> should equal {fraction = Thirtysecondth; extended=true}
            token.sound |> should equal (Tone ( DSharp, Three))

        let assertSecondToken {length = length; sound=sound}=
            length |> should equal {fraction = Sixteenth; extended=false}
            sound |> should equal Rest

        match result with 
            | Choice1Of2 errorMsg -> failwith errorMsg
            | Choice2Of2 tokens -> 
                tokens |> should haveLength 2
                List.head tokens |> assertFirstToken 
                List.item 1 tokens|> assertSecondToken
                ()

module ``When calculating the frequency of notes``  =
    
    [<Fact>]
    let ``A2 should be 440Hz`` () = 
        Tone(A, Two) |> frequency |> should (equalWithin 0.1) 440.

    [<Fact>]
    let ``GSharp2 should be 830.61`` () =
        Tone(GSharp, Two) |> frequency |> should (equalWithin 0.1) 830.61


module ``When calculating the semitones between notes`` =
    [<Fact>]
    let ``A1->A2 should be 12`` ()=
        ((A, One), (A, Two)) ||> semitonesBetween |> should equal 12
        
    [<Fact>]
    let ``A1->A3 should be 24`` ()=
        ((A, One), (A, Three)) ||> semitonesBetween |> should equal 24 
    
    [<Fact>]
    let ``A2->CSharp4 should be 16`` ()= 
        ((A, Two), (CSharp, Three)) ||> semitonesBetween |> should equal 16 

module ``When calculating the duration of a note`` =
    [<Fact>]
    let ``a quarter note should last 500ms`` ()=
          durationFromToken { 
           length={fraction = Quarter; extended = false}; 
           sound=Tone (DSharp,Three)} |> should equal 500.

    [<Fact>]
    let ``an extended quarter note should last 750ms`` ()=
          durationFromToken { 
           length={fraction = Quarter; extended = true}; 
           sound=Tone (DSharp,Three)} |> should equal 750. 

    [<Fact>]
    let ``a 32nd note should last 62,5ms`` ()=
          durationFromToken { 
           length={fraction = Thirtysecondth; extended = false}; 
           sound=Tone (DSharp,Three)} |> should equal 62.5

    [<Fact>]
    let ``an extended 32th note should last 62,5ms`` ()=
          durationFromToken { 
           length={fraction = Thirtysecondth; extended = true}; 
           sound=Tone (DSharp,Three)} |> should equal 93.75

