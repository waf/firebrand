// irc command parsing/serializing. See http://tools.ietf.org/html/rfc2812
module Firebrand.IrcCommand

open System
open System.Text.RegularExpressions

type InboundCommand =
    | Ping of String
    | Notice of String
    | ChannelMessage of String * String
    | UserMessage of String * String
    | Unrecognized of String
        
type OutboundCommand =
    | Pong of String
    | Nick of String
    | User of String * String
    | Join of String
    | OutMessage of String * String

let message cmd = 
    match cmd with
    | Pong server -> "PONG " + server
    | Nick nick -> "NICK " + nick
    | User (username, realname) -> sprintf "USER %s 8 * :%s" username realname
    | Join channel -> "JOIN " + channel
    | OutMessage (target, msg) -> "PRIVMSG " + target + " :" + msg

let (|Match|_|) pattern input =
    let m = Regex.Match(input, pattern) in
    if m.Success then Some (List.tail [ for g in m.Groups -> g.Value ]) else None

let parse (msg:String) = 
    match msg with
        | Match "PING (.*)" server -> Ping(server.Head)
        | Match ".* NOTICE (.*)" notice -> Notice(notice.Head)
        | Match ".* PRIVMSG (#.*) :(.*)" pm-> ChannelMessage(pm.Head, pm.Tail.Head)
        | Match ":(.*)!.* PRIVMSG .* :(.*)" pm-> UserMessage(pm.Head, pm.Tail.Head)
        | _ -> Unrecognized(msg)