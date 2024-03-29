﻿module wave_packer

open signal_generator
open System.IO
open System.Text
open System


let pack (d:seq<int16>) = 
    let stream = new MemoryStream();
    let writer = new BinaryWriter(stream, System.Text.Encoding.ASCII);
    let dataLength = Seq.length d * 2;

    // RIFF
    writer.Write(Encoding.ASCII.GetBytes("RIFF"))
    writer.Write(Seq.length d)
    writer.Write(Encoding.ASCII.GetBytes("WAVE"))

    writer.Write(Encoding.ASCII.GetBytes("fmt "))
    writer.Write(16)
    writer.Write(1s) //PCM
    writer.Write(1s) //mono
    writer.Write(44100 ) // sample rate
    writer.Write(44100 * 16 / 8) // byte rate
    writer.Write(2s) // bytes per sample
    writer.Write(16s) // bits per sample
    
    //data
    writer.Write(Encoding.ASCII.GetBytes("data"))
    writer.Write(dataLength)
    let data:byte[] = Array.zeroCreate dataLength
    Buffer.BlockCopy(Seq.toArray d, 0, data, 0, data.Length)
    writer.Write(data)
    stream


