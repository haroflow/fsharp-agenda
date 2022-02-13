module Program

open System
open Domain
open FsToolkit.ErrorHandling

let tasksFile = @"tasks.json"

let printAgendaListItem i task =
    sprintf "%-3d | %-7s %s %s"
        <| i + 1
        <| task.Priority.ToString()
        <| task.DueDate.ToShortDateString()
        <| String3to50.value task.Title

let getTaskListAsString = function
    | [] -> "-- Empty --"
    | taskList ->
        taskList
        |> List.mapi printAgendaListItem
        |> String.concat "\n"

let getTaskDetails task =
    $"Title: {String3to50.value task.Title}\n" +
    $"Description: {task.LongDescription}\n" +
    $"Creation Date: {task.CreationDate.ToShortDateString()}\n" +
    $"Due Date: {task.DueDate.ToShortDateString()}\n" +
    $"Priority: {task.Priority}\n" +
    $"Done: {task.Done}"

let askTaskNumberFromList taskList =
    printf "Enter the task number: "
    let taskNbrStr = Console.ReadLine()
    Console.Clear()
    match Int32.TryParse taskNbrStr with
    | false, _ ->
        Error "Invalid number"
    | true, taskNumber ->
        match List.tryItem (taskNumber - 1) taskList with
        | None ->
            Error "There's no task for this number"
        | Some task ->
            Ok task

let cmdRemoveTask taskList =
    match askTaskNumberFromList taskList with
    | Error err ->
        printfn "%s" err
        taskList
    | Ok task ->
        removeTask task.Id taskList

let askTaskTitle () =
    printf "Title (3 to 50 characters): "
    match Console.ReadLine() |> String3to50.create with
    | Some title -> Ok title
    | None -> Error "The title must be 3 to 50 characters long..."

let askDueDate () =
    printf "Due date (dd/mm/yyyy HH:MM:SS): "
    match Console.ReadLine() |> DateTime.TryParse with
    | true, dueDate -> Ok dueDate
    | false, _ -> Error "Invalid date"

let askLongDescription () =
    printf "Long description: "
    Console.ReadLine ()

let askPriority () =
    printf "Priority (Low, Medium, High): "
    match Console.ReadLine().ToLower() with
    | "l" | "low" -> Ok Low
    | "m" | "medium" -> Ok Medium
    | "h" | "high" -> Ok High
    | _ -> Error "Invalid priority"

let askTaskData () =
    result {
        let! title = askTaskTitle ()
        let longDescription = askLongDescription ()
        let! dueDate = askDueDate ()
        let! priority = askPriority ()
        return {
            Id = Guid.NewGuid()
            Title = title
            LongDescription = longDescription
            CreationDate = DateTime.Now
            DueDate = dueDate
            Priority = priority
            Done = false
        }
    }

let cmdAddTask taskList =
    let newTask = askTaskData ()
    match newTask with
    | Error e ->
        printfn "%s" e
        taskList
    | Ok t ->
        Console.Clear()
        printfn "Task added\n"
        addTask t taskList

let cmdEditTask taskList =
    match askTaskNumberFromList taskList with
    | Error err ->
        printfn "%s" err
        taskList
    | Ok task ->
        getTaskDetails task |> printfn "%s"
        printfn "\n# Enter the new values"
        match askTaskData () with
        | Ok newTask ->
            editTask
                { newTask with Id = task.Id; CreationDate = task.CreationDate }
                taskList
        | Error err ->
            printfn "%s" err
            taskList

let cmdViewDetails taskList =
    match askTaskNumberFromList taskList with
    | Error err ->
        printfn "%s\n" err
    | Ok task ->
        getTaskDetails task |> printfn "%s\n"

let save tasks =
    Persistence.JSON.saveTaskList tasksFile tasks

type SortAgenda =
    | ByPriority
    | ByDueDate

let sortAgenda taskList sortBy =
    match sortBy with
    | ByPriority -> sortByPriority taskList
    | ByDueDate -> sortByDueDate taskList

let rec run tasks sortBy =
    let sortedTaskList = sortAgenda tasks sortBy
    printfn "# Your agenda"
    printfn "%s" <| getTaskListAsString sortedTaskList
    printfn "\n\
        C - Create new task   | \
        R - Remove task       | \
        E - Edit task         |\n\
        V - View task details | \
        P - Sort by Priority  | \
        D - Sort by Due Date  | \
        Q - Quit"
    printf "What would you like to do? "
    let cmd = Console.ReadLine()
    match cmd.ToLower() with
    | "q" -> ()
    | "c" ->
        let tasks = cmdAddTask sortedTaskList
        save tasks
        run tasks sortBy
    | "e" ->
        let tasks = cmdEditTask sortedTaskList
        save tasks
        run tasks sortBy
    | "r" ->
        let tasks = cmdRemoveTask sortedTaskList
        save tasks
        run tasks sortBy
    | "v" ->
        cmdViewDetails sortedTaskList
        run tasks sortBy
    | "p" ->
        run tasks ByPriority
    | "d" ->
        run tasks ByDueDate
    | _ ->
        Console.Clear()
        printfn "Invalid choice\n"
        run tasks sortBy

[<EntryPoint>]
let main _ =
    Console.Clear()
    let tasks = Persistence.JSON.loadTaskList tasksFile
    run tasks ByDueDate
    0