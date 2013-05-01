// irc command parsing/serializing. See http://tools.ietf.org/html/rfc2812
module Firebrand.IrcCommand

open System

type InboundCommand =
    | Ping of String
    | Notice of String
    | Unrecognized of String
        
type OutboundCommand =
    | Pong of String
    | Nick of String
    | User of String * String
    | Join of String

let message cmd = 
    match cmd with
    | Pong server -> "PONG " + server
    | Nick nick -> "NICK " + nick
    | User (username, realname) -> sprintf "USER %s 8 * :%s" username realname
    | Join channel -> "JOIN " + channel
        
let parse (msg:String) = 
    let parts = Array.toList(msg.Split([|' '|],3))
    match parts with
        | ["PING"; server] -> Ping(server)
        | [_; "NOTICE"; notice] -> Notice(notice)
        | _ -> Unrecognized(msg)