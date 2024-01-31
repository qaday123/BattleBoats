using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBoats
{
    public class MenuManager
    {
        private int current_selection = 0;
        //event EventHandler<EventArgs> menu_selected;
        public int CurrentSelection {
            get { return current_selection; }
            set { if (value >= 0 && value < SELECT_OPTIONS.Length) current_selection = value; }
        }
        static readonly string[] SELECT_OPTIONS = new string[] { "1. Start a new game", "2. Resume a game", "3. How to play", "4. Quit" };
        const int TEXT_LENGTH = 20;


        public void StartMenu()
        {
            bool selected = false;
            //int selection = CurrentSelection;

            Utilities.Type("WELCOME TO BATTLE BOATS", ConsoleColor.Green);
            Console.ForegroundColor = ConsoleColor.Green;
            while (!selected)
            {
                Console.Clear();

                Console.WriteLine("WELCOME TO BATTLE BOATS");
                for (int i = 0; i < SELECT_OPTIONS.Length; i++)
                {
                    Console.BackgroundColor = (i == CurrentSelection) ? ConsoleColor.Yellow : ConsoleColor.Black;
                    Console.ForegroundColor = (i == CurrentSelection) ? ConsoleColor.DarkGreen : ConsoleColor.Green;
                    Console.WriteLine(SELECT_OPTIONS[i].FillSpace(TEXT_LENGTH));
                }
                Console.BackgroundColor = ConsoleColor.Black;

                GetKeyPress(out selected);
            }
            //Console.WriteLine($"Option {CurrentSelection + 1} selected.");
        }

        private void GetKeyPress(out bool selected) // changes selection stuff
        {
            ConsoleKey key = Console.ReadKey().Key;
            
            if (key == ConsoleKey.DownArrow || key == ConsoleKey.S) // can use WASD or arrow keys to move
            {
                CurrentSelection++;
            }
            else if (key == ConsoleKey.UpArrow || key == ConsoleKey.S)
            {
                CurrentSelection--;
            }

            if (key == ConsoleKey.Enter || key == ConsoleKey.Z) // Enter or Z to select
            { 
                selected = true; 
            }
            else selected = false;
        }
        //private int 
    }
}
