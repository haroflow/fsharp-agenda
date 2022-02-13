# fsharp-agenda

Learning F# by creating an agenda in multiple ways.

## Technologies

F# only, using functional-first, without mutable references (if possible), and with tests.

This selection may change:

- Development tools: Visual Studio Code with Ionide-FSharp extension, dotnet
- Client side: Fable with Elmish, and maybe Bulma CSS
- Server side: Falco, for a more functional approach
- Database:
  - File storage
  - Browser local storage
  - SQLite, MySQL, SQL Server
  - MongoDB
- Dockerfile to make it easier to try it out

## Requirements

Create a single-user agenda application with support for:

- Adding tasks
- Editing tasks
- Removing tasks
- View task details
- Mark as done
- Listing tasks, by descending due date
- Listing tasks, by descending priority
- Search for tasks by title and description
- Export list to HTML, TXT, CSV or PDF

The task should be composed of:

- Title: 3 to 50 characters
- Long description: string
- CreationDate: DateTime
- DueDate: DateTime
- Priority: Low, Medium or High
- Done: boolean

## User interfaces

Create each user interface in a separate folder.

Keep a lookout for code we can share between these interfaces, but code duplication is not a strong concern in this project.

1. Console application, saving to a JSON file
2. Web client-side only, saving to localStorage
3. Web API only, saving to SQLite or MongoDB
4. Web full-stack, saving to SQL Server, MySQL or Postgres
5. Desktop? Maybe Avalonia.FuncUI?
6. Mobile?
