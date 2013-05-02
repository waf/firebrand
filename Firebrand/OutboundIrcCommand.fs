// irc command serializing. See http://tools.ietf.org/html/rfc2812
module Firebrand.IrcCommand.Outbound

open System

type OutboundIrcCommand =
    | Pong of String
    | Nick of String
    | User of String * String
    | Join of String
    | OutMessage of String * String

let serialize cmd = 
    match cmd with
    | Pong server -> "PONG " + server
    | Nick nick -> "NICK " + nick
    | User (username, realname) -> sprintf "USER %s 8 * :%s" username realname
    | Join channel -> "JOIN " + channel
    | OutMessage (target, msg) -> "PRIVMSG " + target + " :" + msg