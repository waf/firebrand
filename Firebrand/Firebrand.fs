module Firebrand.Main

open Firebrand.IrcBot

let server = "irc.freenode.net"
let port = 6667
let nick = "firebrand"
let user = "firebrand"
let channel = "#firebrand-test"

[<EntryPoint>]
let main args = 
    let nickInfo = {nick = nick; username = user; realname = user}
    let bot = IrcBot(server, port, nickInfo)
    bot.Join(channel)
    bot.Listen()
    0