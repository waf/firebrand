// irc command parsing. See http://tools.ietf.org/html/rfc2812
module Firebrand.IrcCommand.Inbound

open System
open System.Text.RegularExpressions

type InboundIrcCommand =
    | Ping of String
    | Notice of String
    | ChannelMessage of String * String
    | UserMessage of String * String
    | Unrecognized of String
        
let (|Match|_|) (pattern:Regex) input =
    let m = pattern.Match(input) in
    if m.Success then Some (List.tail [ for g in m.Groups -> g.Value ]) else None

module Pattern =    
    let PING = Regex("PING (.*)")
    let NOTICE = Regex(".* NOTICE (.*)")
    let CHANNELMSG = Regex(".* PRIVMSG (#.*) :(.*)")
    let USERMSG = Regex(":(.*)!.* PRIVMSG .* :(.*)")

let parse (msg:String) = 
    match msg with
        | Match Pattern.PING [server]-> Ping(server)
        | Match Pattern.NOTICE [notice] -> Notice(notice)
        | Match Pattern.CHANNELMSG [channel; msg] -> ChannelMessage(channel, msg)
        | Match Pattern.USERMSG [user; msg] -> UserMessage(user, msg)
        | _ -> Unrecognized(msg)