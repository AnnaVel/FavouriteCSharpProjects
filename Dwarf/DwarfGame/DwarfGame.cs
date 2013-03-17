using System;
using System.Threading;
using System.Collections.Generic;

namespace DwarfGame
{
    class DwarfGame
    {
        static int dwarfPosition = Console.WindowWidth / 2 - 1;
        static List<Rock> rocks = new List<Rock>();
        static int lives = 5;
        static int points = 0;

        static void Main(string[] args)
        {
            RemoveScrollBars();
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key == ConsoleKey.LeftArrow)
                    {
                        DwarfMoveLeft();
                    }
                    else if (key.Key == ConsoleKey.RightArrow)
                    {
                        DwarfMoveRight();
                    }
                }
                MoveRocks();
                GenerateRocks();
                Console.Clear();
                DrawDwarf();
                DrawRocks();
                DrawResult();
                points++;
                if (lives == 0)
                {
                    if (GameOver() == true)
                    {
                        SetInitialPosition();
                    }
                    else
                    {
                        break;
                    }
                }
                Thread.Sleep(150);
            }
        }

        static void RemoveScrollBars()
        {
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
        }

        static void SetInitialPosition()
        {
            points = 0;
            lives = 5;
            dwarfPosition = Console.WindowWidth / 2 - 1;
            rocks = new List<Rock>();
        }

        static void DwarfMoveLeft()
        {
            if (dwarfPosition > 1)
            {
                dwarfPosition--;
            }
        }

        static void DwarfMoveRight()
        {
            if (dwarfPosition < Console.WindowWidth - 3)
            {
                dwarfPosition++;
            }
        }

        static void MoveRocks()
        {
            List<Rock> rocksToRemove = new List<Rock>();
            foreach (Rock rock in rocks)
            {
                if (rock.PositionY < Console.WindowHeight - 1)
                {
                    rock.PositionY++;
                }
                else
                {
                    bool lifeLost = false;
                    for (int i = 0; i < rock.Size; i++)
                    {
                        if (rock.PositionX + i >= dwarfPosition && rock.PositionX + i <= dwarfPosition + 2)
                        {
                            lifeLost = true;
                        }
                    }

                    if (lifeLost)
                    {
                        lives--;
                    }
                    rocksToRemove.Add(rock);
                }
            }

            foreach (Rock rock in rocksToRemove)
            {
                rocks.Remove(rock);
            }
        }

        static void GenerateRocks()
        {
            Random randomizer = new Random();
            int chanceNoRocks = randomizer.Next(0, 2);
            if (chanceNoRocks == 1)
            {
                int numberOfRocks = randomizer.Next(0, 5);
                for (int i = 0; i < numberOfRocks; i++)
                {
                    int positionRockX = randomizer.Next(0 + i * Console.WindowWidth / numberOfRocks, (i + 1) * Console.WindowWidth / numberOfRocks);
                    Rock newRock = new Rock(positionRockX);
                    if (positionRockX + newRock.Size > Console.WindowWidth)
                    {
                        newRock.PositionX = Console.WindowWidth - 2 - newRock.Size;
                    }
                    rocks.Add(newRock);
                }
            }
        }

        static void DrawDwarf()
        {
            PrintAtPosition(dwarfPosition, Console.WindowHeight - 1, '(');
            PrintAtPosition(dwarfPosition + 1, Console.WindowHeight - 1, '0');
            PrintAtPosition(dwarfPosition + 2, Console.WindowHeight - 1, ')');
        }

        static void DrawRocks()
        {
            foreach (Rock rock in rocks)
            {
                for (int i = 0; i < rock.Size; i++)
                {
                    PrintAtPosition(rock.PositionX + i, rock.PositionY, rock.Shape);
                }
            }
        }

        static void DrawResult()
        {
            Console.SetCursorPosition(Console.WindowWidth - 20, 0);
            Console.Write("\u2665 {0}", lives);
            Console.SetCursorPosition(Console.WindowWidth - 20, 1);
            Console.Write("points: {0}", points);
        }

        static void PrintAtPosition(int x, int y, char symbolToPrint)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(symbolToPrint);
        }

        static bool GameOver()
        {
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - 4, Console.WindowHeight / 2);
            Console.WriteLine("Game Over!");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 4, Console.WindowHeight / 2 + 1);
            Console.WriteLine("Points: {0}", points);
            Console.SetCursorPosition(Console.WindowWidth / 2 - 4, Console.WindowHeight / 2 + 2);
            Console.Write("Start new game? Y/N: ");
            string answer = Console.ReadLine();
            if (answer == "N")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

