module Firebrand.IrcBot

open System
open Firebrand.IrcCommand.Outbound
open Firebrand.IrcCommand.Inbound
open Firebrand.IrcConnection

type BotName = { nick : String; username : String; realname : String }

type IrcBot(server, port, name) = 

    let conn = IrcConnection(server, port)
    do    
        conn.Send(Nick(name.nick))
        conn.Send(User(name.username, name.realname))

    member this.Join(channel) = 
        conn.Send(Join(channel))

    member this.Listen() = 
        for cmd in conn.Stream() do
            match this.respond cmd with
                | Some(response) -> conn.Send response
                | None -> ()

    member this.respond cmd =
        match cmd with
            | Ping server -> Some(Pong server)
            | ChannelMessage (channel, msg) -> Some(OutMessage(channel, "You said " + msg))
            | UserMessage (user, msg) -> Some(OutMessage(user, "You said " + msg))
            | _ -> None
