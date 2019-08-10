module signal_generator


let generateSamples milliseconds frequency = 
    let _sampleRate = 44100.
    let _sixteenBitSampleLimit = 32767.
    let _volume = 0.8
    let toAmplitude x = 
        x
        |> (*) (2.*System.Math.PI * frequency / _sampleRate)
        |> sin
        |> (*) _sixteenBitSampleLimit
        |> (*) _volume
        |> int16

    let _numberOfSamples = milliseconds / 1000. * _sampleRate
    let _requiredSamples = seq {1.0 .. _numberOfSamples}
    Seq.map toAmplitude _requiredSamples



