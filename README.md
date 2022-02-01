# fsharp-agenda

Learning F# by creating an agenda in multiple ways.

## Technologies

F# only, using functional patterns and best practices, without mutable references (if possible).

This selection may change:

- Development tools: Visual Studio Code with Ionide-FSharp extension, dotnet
- Client side: Fable with Elmish, and maybe Bulma CSS
- Server side: Falco, for a more functional approach
- Database:
  - File storage
  - Browser local storage
  - SQLite, MySQL, SQL Server
  - MongoDB
- Dockerfile to make it easy to try it out without installing anything else

## Requirements

Create a single-user agenda application with support for:

- Adding tasks
- Editing tasks
- Removing tasks
- View task details
- Mark as done
- Listing tasks, by due date
- Listing tasks, by priority
- Search for tasks by title and description
- Export list to HTML, TXT, CSV or PDF

The task should be composed of:

- Title: max 50 characters
- Long description
- CreationDate
- DueDate
- Priority: Low, Medium or High

## User interfaces

Create each interface in a separate folder.

Keep a lookout for code we can share between these interfaces, but code duplication is not a problem for this.

1. Console application, saving to a JSON (?) file
2. Web client-side only, saving to localStorage
3. Web API only, saving to SQLite or MongoDB
4. Web full-stack, saving to SQL Server, MySQL or Postgres
5. Desktop?
6. Mobile?
