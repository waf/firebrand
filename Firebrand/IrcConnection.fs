// for sending/receiving irc commands
module Firebrand.IrcConnection

open System
open System.IO
open System.Net.Sockets
open IrcCommand

type IrcConnection(server : String, port : int) = 
    let tcp = new TcpClient();
    do tcp.Connect(server, port)
    let reader = new StreamReader(tcp.GetStream())
    let writer = new StreamWriter(tcp.GetStream())
    do writer.AutoFlush <- true
    
    // send an irc command to the server
    member this.Send(cmd) =
        writer.WriteLine(message(cmd))
    
    // receive commands from the server
    member this.Stream() = 
        seq {
            while not(reader.EndOfStream) do
                let line = reader.ReadLine()
                Console.WriteLine line
                yield parse line
        }