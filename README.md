Imports

System → grundläggande funktioner.
System.Collections.Generic → List<T>.
System.IO → läsa/spara filer.

TaskBase

Abstrakt mall för alla tasks.
Har: Titel, Datum, Projekt, Status.
Konstruktor sätter värden.
Abstrakt metod DisplayTask() → måste göras i subklass.

Task

Ärver TaskBase.
Implementerar DisplayTask() → skriver ut task i konsolen.

taskList

Lista som lagrar alla tasks.

Main

Laddar tasks från fil.
Skapar exempel om listan är tom.
Kör meny i loop tills avslutning.

ShowTasks

Sortering efter datum eller projekt.
Visar alla tasks med DisplayTask().

AddTask

Hämtar input.
Skapar och lägger till ny task.
Felhantering med try/catch.

EditTask

Välj task → uppdatera, markera klar eller ta bort.

SaveTasks

Skriver alla tasks till tasks.txt.

UML Klassdiagram – Textversion

           ┌──────────────────────────┐
           │        TaskBase          │ <<abstract>>
           ├──────────────────────────┤
           │ - Title : string         │
           │ - DueDate : DateTime     │
           │ - Project : string       │
           │ - IsDone : bool          │
           ├──────────────────────────┤
           │ + DisplayTask() : void   │
           └───────────┬──────────────┘
                       │
                       │ Inherits
                       ▼
           ┌──────────────────────────┐
           │          Task            │
           ├──────────────────────────┤
           │  (inherits properties)   │
           ├──────────────────────────┤
           │ + DisplayTask() : void   │
           └──────────────────────────┘

           ┌──────────────────────────┐
           │      TaskService         │
           ├──────────────────────────┤
           │ - TaskList : List<Task>  │
           ├──────────────────────────┤
           │ + ShowTasks()            │
           │ + AddTask()              │
           │ + EditTask()             │
           │ + SaveTasks()            │
           │ + LoadTasks()            │
           └───────────┬──────────────┘
                       │ Uses
                       ▼
           ┌──────────────────────────┐
           │        Program           │
           ├──────────────────────────┤
           │ + Main()                 │
           └──────────────────────────┘

Förklaring av diagrammet

TaskBase → Abstrakt klass med egenskaper och abstrakt metod DisplayTask().

Task → Ärver TaskBase och implementerar DisplayTask().

TaskService → Hanterar listan av tasks och alla operationer (visa, lägga till, redigera, spara, läsa).

Program → Startpunkt (Main()), använder TaskService.


LoadTasks

Läser varje rad från filen och skapar tasks.
