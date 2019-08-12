namespace WavePackerTests

open Xunit
open FsUnit.Xunit
open wave_packer
open signal_generator
open System.IO

module ``When packing an audio file``  =

    let getFile milliseconds =
        generateSamples milliseconds 440.
        |> Array.ofSeq
        |> pack
        |> (fun ms -> 
            ms.Seek(0L, SeekOrigin.Begin) |> ignore
            ms)

    [<Fact>]
    let ``the stream should start with 'RIFF'`` () =
        let file = getFile 2000. 
        let bucket = Array.zeroCreate 4
        file.Read(bucket, 0, 4) |> ignore
        let first4Chars = System.Text.Encoding.ASCII.GetString(bucket);
        first4Chars |> should equal "RIFF"

    [<Fact>]
    let ``file size is correct`` () =
        let formatOverhead = 44.
        let audioLengths = [2000.; 50. ; 1500.; 3000.]
        let files = List.zip audioLengths (List.map getFile audioLengths)
        let assertLength (length, file:MemoryStream) =
            file.Length |> should equal (((length / 1000.) * 44100. * 2. + formatOverhead) |> int64)
        List.iter assertLength files
