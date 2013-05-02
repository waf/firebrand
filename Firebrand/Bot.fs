module Firebrand.Bot

open System
open IrcCommand
open IrcConnection

let server = "irc.freenode.net"
let port = 6667
let nick = "firebrand"
let user = "firebrand"
let channel = "#firebrand-test"
    
[<EntryPoint>]
let main args = 

    let conn = IrcConnection(server, port)

    conn.Send(Nick(nick))
    conn.Send(User(user,user))
    conn.Send(Join(channel))
        
    for cmd in conn.Stream() do
           
        let response = 
            match cmd with
                | Ping server -> Some(Pong server)
                | ChannelMessage (channel, msg) -> Some(OutMessage(channel, "You said " + msg))
                | UserMessage (user, msg) -> Some(OutMessage(user, "You said " + msg))
                | _ -> None

        match response with
            | Some rsp -> conn.Send(rsp)
            | None -> ()

    0