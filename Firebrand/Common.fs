module Firebrand.Common

open System.Text.RegularExpressions

//active pattern for regex matching 
let (|Match|_|) (pattern:Regex) input =
    let m = pattern.Match(input) in
    if m.Success then Some (List.tail [ for g in m.Groups -> g.Value ]) else None