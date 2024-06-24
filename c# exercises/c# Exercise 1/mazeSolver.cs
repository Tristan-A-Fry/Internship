using System;
using System.Collections.Generic;
using System.IO;

public class MazeSolver
{
    private const char WALL = '0';
    private const char PATH = '1';
    private const char START = '*';
    private const char PRIZE = '$';
    private const char VISITED = 'x';
    private const char PATH_TO_PRIZE = '2';

    private char[][] maze;
    private int rows;
    private int cols;
    private int startRow;
    private int startCol;
    private int prizeRow;
    private int prizeCol;

    public MazeSolver(string fileName)
    {
        ReadMazeFromFile(fileName);
    }

    public void SolveMaze()
    {
        if (!FindStartAndPrize())
        {
            Console.WriteLine("Maze doesn't have a start or prize.");
            return;
        }

        if (FindPathToPrize(startRow, startCol))
        {
            maze[prizeRow][prizeCol] = PRIZE; // restore the original prize location
            PrintMazeToFile("maze_result.txt");
        }
        else
        {
            Console.WriteLine("Maze doesn't have a valid path to the prize.");
        }
    }

    private void ReadMazeFromFile(string fileName)
    {
        try
        {
            string[] lines = File.ReadAllLines(fileName);
            rows = lines.Length;
            cols = lines[0].Length;
            maze = new char[rows][];

            for (int i = 0; i < rows; i++)
            {
                maze[i] = lines[i].ToCharArray();
            }
        }
        catch (IOException e)
        {
            Console.WriteLine($"Error reading the file: {e.Message}");
        }
    }

    private bool FindStartAndPrize()
    {
        bool foundStart = false;
        bool foundPrize = false;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (maze[i][j] == START)
                {
                    startRow = i;
                    startCol = j;
                    foundStart = true;
                }
                else if (maze[i][j] == PRIZE)
                {
                    prizeRow = i;
                    prizeCol = j;
                    foundPrize = true;
                }

                if (foundStart && foundPrize)
                    return true;
            }
        }

        return foundStart && foundPrize;
    }

    private bool FindPathToPrize(int row, int col)
    {
        if (row < 0 || row >= rows || col < 0 || col >= cols)
        {
            return false;
        }

        if (maze[row][col] == PRIZE)
        {
            return true;
        }

        if (maze[row][col] != PATH && maze[row][col] != START)
        {
            return false;
        }

        maze[row][col] = PATH_TO_PRIZE;

        if (FindPathToPrize(row - 1, col) || // Up
            FindPathToPrize(row + 1, col) || // Down
            FindPathToPrize(row, col - 1) || // Left
            FindPathToPrize(row, col + 1))   // Right
        {
            return true;
        }

        maze[row][col] = PATH; // Unmark path
        return false;
    }

    private void PrintMazeToFile(string fileName)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                for (int i = 0; i < rows; i++)
                {
                    writer.WriteLine(new string(maze[i]));
                }
            }

            Console.WriteLine($"Result maze saved to {fileName}");
        }
        catch (IOException e)
        {
            Console.WriteLine($"Error writing to file: {e.Message}");
        }
    }
}

public static class MazeSolverManager
{
    public static void Run()
    {
        MazeSolver solver = new MazeSolver("maze.txt");
        solver.SolveMaze();
    }
}
