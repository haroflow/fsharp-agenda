module Persistence

module JSON =
    open Domain
    open FSharp.Json

    let saveTaskList path (taskList: Task list) =
        let json = Json.serialize taskList
        System.IO.File.WriteAllText(path, json)

    let loadTaskList path =
        if System.IO.File.Exists path then
            let json = System.IO.File.ReadAllText path
            Json.deserialize<Task list> json
        else
            []