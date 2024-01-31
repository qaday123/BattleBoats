using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace BattleBoats
{
    public static class Utilities
    {
        public static void Type(string output, ConsoleColor colour = ConsoleColor.White, int delay = 20)
        {
            Console.ForegroundColor = colour;
            foreach (char c in output)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.Write("\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static string FillSpace(this string input, int length)
        {
            string output = input;
            for (int i = input.Length - 1; i < length; i++)
            {
                output += " ";
            }
            return output;
        }
        public static string SymbolToString(this Symbol me)
        {
            switch (me)
            {
                case Symbol.EMPTY:
                    return ".";
                case Symbol.BOAT:
                    return "B";
                case Symbol.HIT:
                    return "H";
                case Symbol.MISS:
                    return "M";
                default:
                    return "L"; // take the L if something goes wrong
            }
        }
        public static Vector2 ChangeSelection(Vector2 currentSelection, int xBound, int yBound, out bool selected)
        {
            Vector2 newSelection = currentSelection;
            ConsoleKey Input = Console.ReadKey().Key;
            selected = false;

            switch (Input)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    if (currentSelection.Y > 1) newSelection.Y -= 1;
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    if (currentSelection.Y < yBound) newSelection.Y += 1;
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    if (currentSelection.X > 1) newSelection.X -= 1;
                    break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    if (currentSelection.X < xBound) newSelection.X += 1;
                    break;
                case ConsoleKey.Enter:
                case ConsoleKey.Z:
                    selected = true;
                    break;
                default:
                    break;
            }

            return newSelection;
        }
        public static void Continue(string message = "Press any key to continue: ")
        {
            Console.Write("\b"+message);
            Console.ReadKey();
            Console.Clear();
        }
    }
}
