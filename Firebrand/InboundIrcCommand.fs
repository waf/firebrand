// irc command parsing. See http://tools.ietf.org/html/rfc2812
module Firebrand.IrcCommand.Inbound

open System
open System.Text.RegularExpressions
open Firebrand.Common

type InboundIrcCommand =
    | Ping of String
    | Notice of String
    | ChannelMessage of String * String
    | UserMessage of String * String
    | Unrecognized of String

// holds precompiled regexs for parsing
module Pattern =    
    let PING = Regex("PING (.*)")
    let NOTICE = Regex(".* NOTICE (.*)")
    let CHANNELMSG = Regex(".* PRIVMSG (#.*) :(.*)")
    let USERMSG = Regex(":(.*)!.* PRIVMSG .* :(.*)")

// convert an incoming line from the irc server into an InboundIrcCommand
let parse (msg:String) = 
    match msg with
        | Match Pattern.PING [server]-> Ping(server)
        | Match Pattern.NOTICE [notice] -> Notice(notice)
        | Match Pattern.CHANNELMSG [channel; msg] -> ChannelMessage(channel, msg)
        | Match Pattern.USERMSG [user; msg] -> UserMessage(user, msg)
        | _ -> Unrecognized(msg)