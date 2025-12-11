using System;                  // Importerar System-namespace som innehåller grundläggande klasser, t.ex. Console och DateTime
using System.Collections.Generic; // Importerar listor och andra generiska samlingar
using System.IO;               // Importerar filhantering, t.ex. StreamReader och StreamWriter

// Abstrakt basklass som representerar en uppgift/task
abstract class TaskBase
{
    public string Title { get; set; }       // Titeln på tasken
    public DateTime DueDate { get; set; }   // Förfallodatum för tasken
    public string Project { get; set; }     // Namnet på projektet tasken tillhör
    public bool IsDone { get; set; }        // Status, true = klar, false = ej klar

    // Konstruktor som initierar taskens egenskaper
    public TaskBase(string title, DateTime dueDate, string project)
    {
        Title = title;       // Sätter taskens titel
        DueDate = dueDate;   // Sätter förfallodatum
        Project = project;   // Sätter projekt
        IsDone = false;      // Standardvärde för ny task är ej klar
    }

    // Abstrakt metod som måste implementeras i subklass
    public abstract void DisplayTask();
}

// Konkret klass som ärver från TaskBase
class Task : TaskBase
{
    // Konstruktor som skickar värden till basklassen
    public Task(string title, DateTime dueDate, string project) : base(title, dueDate, project) { }

    // Implementering av abstrakt metod - visar tasken på skärmen
    public override void DisplayTask()
    {
        string status = IsDone ? "Done" : "ToDo"; // Kontrollera status
        Console.WriteLine($"{Title} | {DueDate.ToShortDateString()} | {Project} | {status}"); // Skriv ut taskinfo
    }
}

class Program
{
    static List<Task> taskList = new List<Task>(); // Skapar en lista som håller alla tasks

    static void Main()
    {
        LoadTasks(); // Försök ladda tasks från fil vid start

        // Om inga tasks finns, fyll på med 5 exempelrader
        if (taskList.Count == 0)
        {
            taskList.Add(new Task("Buy groceries", DateTime.Now.AddDays(1), "Home"));    // Lägg till matinköp
            taskList.Add(new Task("Finish report", DateTime.Now.AddDays(2), "Work"));     // Lägg till rapportarbete
            taskList.Add(new Task("Call John", DateTime.Now.AddDays(1), "Personal"));     // Lägg till samtal
            taskList.Add(new Task("Plan trip", DateTime.Now.AddDays(5), "Leisure"));      // Lägg till resplanering
            taskList.Add(new Task("Clean room", DateTime.Now.AddDays(3), "Home"));        // Lägg till städning
        }

        bool running = true; // Variabel för huvudmeny-loop
        while (running)
        {
            int todoCount = 0; // Räknare för ej klara tasks
            int doneCount = 0; // Räknare för klara tasks

            // Räkna antal ToDo och Done tasks
            foreach (var t in taskList)
            {
                if (t.IsDone) doneCount++;
                else todoCount++;
            }

            // Visa välkomstmeddelande och dynamisk status
            Console.WriteLine(">> Welcome to ToDoLy");
            Console.WriteLine($">> You have {todoCount} tasks todo and {doneCount} tasks are done!");
            Console.WriteLine(">> Pick an option:");
            Console.WriteLine(">> (1) Show Task List (by date or project)");
            Console.WriteLine(">> (2) Add New Task");
            Console.WriteLine(">> (3) Edit Task (update, mark as done, remove)");
            Console.WriteLine(">> (4) Save and Quit");

            string choice = Console.ReadLine(); // Läs användarens val
            switch (choice) // Välj åtgärd baserat på input
            {
                case "1":
                    ShowTasks(); // Visa task-lista
                    break;
                case "2":
                    AddTask();   // Lägg till ny task
                    break;
                case "3":
                    EditTask();  // Redigera task
                    break;
                case "4":
                    SaveTasks(); // Spara tasks till fil
                    running = false; // Avsluta loop och program
                    break;
                default:
                    Console.WriteLine("Invalid option, try again."); // Hantera ogiltigt val
                    break;
            }
        }
    }

    // Metod för att visa alla tasks
    static void ShowTasks()
    {
        Console.WriteLine("Sort by (1) Date or (2) Project?");
        string sortChoice = Console.ReadLine(); // Användarval för sortering
        List<Task> sortedList = new List<Task>(taskList); // Kopia av task-listan

        if (sortChoice == "1")
            sortedList.Sort((x, y) => x.DueDate.CompareTo(y.DueDate)); // Sortera efter datum
        else if (sortChoice == "2")
            sortedList.Sort((x, y) => x.Project.CompareTo(y.Project)); // Sortera efter projekt

        // Visa varje task med foreach-loop
        foreach (var t in sortedList)
        {
            t.DisplayTask(); // Anropa polymorf metod
        }
    }

    // Metod för att lägga till ny task
    static void AddTask()
    {
        try // Hantera fel med try/catch
        {
            Console.Write("Enter title: ");
            string title = Console.ReadLine(); // Läs titel
            Console.Write("Enter due date (yyyy-mm-dd): ");
            DateTime dueDate = DateTime.Parse(Console.ReadLine()); // Läs och konvertera datum
            Console.Write("Enter project: ");
            string project = Console.ReadLine(); // Läs projekt

            taskList.Add(new Task(title, dueDate, project)); // Skapa och lägg till ny task
        }
        catch (Exception ex) // Fångar fel, t.ex. felaktigt datum
        {
            Console.WriteLine("Error adding task: " + ex.Message);
        }
    }

    // Metod för att redigera en task
    static void EditTask()
    {
        ShowTasks(); // Visa först alla tasks
        Console.Write("Enter task number to edit (1-based): ");
        try
        {
            int index = int.Parse(Console.ReadLine()) - 1; // Konvertera till 0-baserat index
            if (index < 0 || index >= taskList.Count) // Kontrollera giltigt index
            {
                Console.WriteLine("Invalid task number.");
                return;
            }

            Task t = taskList[index]; // Hämta vald task
            Console.WriteLine("Choose action: (1) Update (2) Mark Done (3) Remove");
            string action = Console.ReadLine(); // Läs åtgärd

            if (action == "1") // Uppdatera task
            {
                Console.Write("New title: ");
                t.Title = Console.ReadLine();
                Console.Write("New due date (yyyy-mm-dd): ");
                t.DueDate = DateTime.Parse(Console.ReadLine());
                Console.Write("New project: ");
                t.Project = Console.ReadLine();
            }
            else if (action == "2") // Markera som klar
            {
                t.IsDone = true;
            }
            else if (action == "3") // Ta bort task
            {
                taskList.RemoveAt(index);
            }
            else
            {
                Console.WriteLine("Invalid action.");
            }
        }
        catch (Exception ex) // Hantera fel, t.ex. ogiltig input
        {
            Console.WriteLine("Error editing task: " + ex.Message);
        }
    }

    // Metod för att spara tasks till fil
    static void SaveTasks()
    {
        try
        {
            using (StreamWriter sw = new StreamWriter("tasks.txt")) // Öppna fil för skrivning
            {
                foreach (var t in taskList) // Skriv varje task
                {
                    sw.WriteLine($"{t.Title}|{t.DueDate}|{t.Project}|{t.IsDone}");
                }
            }
        }
        catch (Exception ex) // Hantera filfel
        {
            Console.WriteLine("Error saving tasks: " + ex.Message);
        }
    }

    // Metod för att läsa tasks från fil vid programstart
    static void LoadTasks()
    {
        if (File.Exists("tasks.txt")) // Kontrollera att filen finns
        {
            try
            {
                foreach (var line in File.ReadAllLines("tasks.txt")) // Läs rad för rad
                {
                    var parts = line.Split('|'); // Dela upp rad i delar
                    Task t = new Task(parts[0], DateTime.Parse(parts[1]), parts[2]); // Skapa task
                    t.IsDone = bool.Parse(parts[3]); // Sätt status
                    taskList.Add(t); // Lägg till i listan
                }
            }
            catch // Fångar fel under läsning
            {
                Console.WriteLine("Error loading tasks.");
            }
        }
    }
}
