namespace SignalGeneratorTests

open Xunit
open FsUnit.Xunit
open signal_generator

module ``When generating 2 seconds at 440Hz``  =

    [<Fact>]
    let ``there should be 88200 samples`` () =
        let samples = generateSamples 2000. 440.
        Seq.length samples |> should equal 88200

    [<Fact>]
    let ``all samples should be in range`` () =
        let sixteenBitSampleLimit = 32767s
        let samples = generateSamples 2000. 440.

        samples |> Seq.iter (fun s -> 
            (s > (-1s * sixteenBitSampleLimit) && s < (sixteenBitSampleLimit)) 
            |> should be True
        )


module ``When generating 2 seconds at 0Hz`` =

    [<Fact>]
    let ``the samples should be all 0`` () =
        let samples = generateSamples 2000. 0.
        let expected = Array.init 88200 (fun i -> int16 0)
        Seq.toArray samples |> should equal expected
