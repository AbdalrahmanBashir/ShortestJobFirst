using System;
using System.Collections.Generic;
using System.Linq;

class Process
{
    public int ArrivalTime { get; set; }
    public int BurstTime { get; set; }
    public int Priority { get; set; }
    public int CompletionTime { get; set; }
    public int WaitingTime { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        // Define a list of processes with predefined arrival times, burst times, and priorities.
        List<Process> processes = new List<Process>
        {
            new Process { ArrivalTime = 0, BurstTime = 2, Priority = 2 },
            new Process { ArrivalTime = 1, BurstTime = 1, Priority = 1 },
            new Process { ArrivalTime = 2, BurstTime = 8, Priority = 4 },
            new Process { ArrivalTime = 3, BurstTime = 4, Priority = 2 },
            new Process { ArrivalTime = 4, BurstTime = 5, Priority = 3 }
        };

        // Call the SJF scheduling algorithm to reorder the processes based on SJF.
        PerformSJFScheduling(processes);

        // Display the results, including completion times and waiting times.
        DisplayResults(processes);
    }

    static void PerformSJFScheduling(List<Process> processes)
    {
        // Sort the processes by arrival time to handle the order in which they arrive.
        processes = processes.OrderBy(p => p.ArrivalTime).ToList();

        // Create a list to hold the scheduled processes.
        List<Process> scheduledProcesses = new List<Process>();

        // Initialize the current time to zero.
        int currentTime = 0;

        // Continue scheduling until all processes are completed.
        while (processes.Count > 0)
        {
            // Find the processes that have arrived by comparing their arrival times to the current time.
            List<Process> arrivedProcesses = processes.Where(p => p.ArrivalTime <= currentTime).ToList();

            if (arrivedProcesses.Count == 0)
            {
                // If no process has arrived, increment the current time.
                currentTime++;
            }
            else
            {
                // Select the process with the shortest burst time and the highest priority among the arrived processes.
                Process shortestJob = arrivedProcesses.OrderBy(p => p.BurstTime).ThenBy(p => p.Priority).First();

                // Update the current time and process the selected job.
                currentTime += shortestJob.BurstTime;

                // Update the completion time and waiting time for the selected process.
                foreach (Process p in processes)
                {
                    if (p.Priority == shortestJob.Priority)
                    {
                        p.CompletionTime = currentTime;
                        p.WaitingTime = p.CompletionTime - p.ArrivalTime - p.BurstTime;

                        // Add the process to the list of scheduled processes and remove it from the list of unprocessed processes.
                        scheduledProcesses.Add(p);
                        processes.Remove(p);
                        break;
                    }
                }
            }
        }
    }

    static void DisplayResults(List<Process> scheduledProcesses)
    {
        double totalWaitingTime = 0.0;
        int n = scheduledProcesses.Count;

        Console.WriteLine("Process\tCompletion Time\tWaiting Time");
        foreach (Process p in scheduledProcesses)
        {
            Console.WriteLine($"P{p.Priority}\t{p.CompletionTime}\t\t{p.WaitingTime}");
            totalWaitingTime += p.WaitingTime;
        }

        double averageWaitingTime = totalWaitingTime / n;

        Console.WriteLine($"Average Waiting Time: {averageWaitingTime}");
    }
}
