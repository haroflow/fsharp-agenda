module Domain

open System

// General Types

type String3to50 = String3to50 of string

module String3to50 =
    let create s =
        let len = String.length s
        if 3 <= len && len <= 50
        then Some (String3to50 s)
        else None
    
    let value (String3to50 s) = s

// Domain

type Priority =
    | Low
    | Medium
    | High

type Task = {
    Id: Guid
    Title: String3to50
    LongDescription: string
    CreationDate: DateTime
    DueDate: DateTime
    Priority: Priority
    Done: bool
}

let addTask task taskList =
    task :: taskList

let editTask editedTask taskList =
    taskList
    |> List.map (function
                | x when x.Id = editedTask.Id -> editedTask
                | x -> x )

let removeTask taskId taskList =
    taskList
    |> List.filter (fun x -> x.Id <> taskId)

let getTask taskId taskList =
    taskList
    |> List.tryFind (fun x -> x.Id = taskId)

let markAsDone done' task =
    { task with Done = done' }

let sortByPriority taskList =
    taskList
    |> List.sortByDescending (fun x -> x.Priority)

let sortByDueDate taskList =
    taskList
    |> List.sortBy (fun x -> x.DueDate)

let isExpired task =
    task.DueDate < DateTime.Now

let isDone task =
    task.Done

let searchByTitleAndDescription (search: string) taskList =
    let s = search.ToLower()
    taskList
    |> List.filter (fun x ->
        let title = String3to50.value(x.Title).ToLower()
        let desc = x.LongDescription.ToLower()
        title.Contains(s) || desc.Contains(s))
