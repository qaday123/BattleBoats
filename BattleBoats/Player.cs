using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace BattleBoats
{
    public class Player
    {
        public GameBoard Fleet { get; }
        public GameBoard Targeting { get; }
        public bool isComputer { get; }
        [JsonProperty]
        public readonly int NUM_BOATS;
        public string Name { get; }

        public Player(GameBoard board, GameBoard targeting, bool is_computer, string name)
        {
            Fleet = board;
            Targeting = targeting;
            isComputer = is_computer;
            NUM_BOATS = 5;
            Name = is_computer ? "Bot" : name;
        }
        public Player(bool is_computer)
        {
            Fleet = new GameBoard(8, 8);
            Targeting = new GameBoard(8, 8);
            isComputer = is_computer;
            NUM_BOATS = 5;
            Name = is_computer ? "Bot" : "Player";
        }
        private Player()
        {
        }

        [JsonConstructor]
        public Player(GameBoard Fleet, GameBoard Targeting, bool isComputer, int numBoats, string Name)
        {
            this.NUM_BOATS = numBoats;
            this.Fleet = Fleet;
            this.Targeting = Targeting;
            this.isComputer = isComputer;
            this.Name = Name;
        }

        public void SetUpFleet()
        {
            int boatsPlaced = 0;
            Vector2 currentSelection = new Vector2(1, 1);
            string? message = null;

            if (!isComputer)
            {
                while (boatsPlaced < NUM_BOATS)
                {
                    currentSelection = Fleet.SelectTile(message, "Set up your fleet:", currentSelection);
                    if (Fleet.GetTile(currentSelection) != Symbol.BOAT)
                    {
                        Fleet.SetTile(currentSelection, Symbol.BOAT);
                        boatsPlaced++;
                        message = "Boat placed at " + currentSelection;
                    }
                    else message = "Tile already taken!";
                }
                Utilities.Continue();
            }
            else
            {
                Random rnd = new Random();
                while (boatsPlaced < NUM_BOATS)
                {
                    currentSelection = new Vector2(rnd.Next(Fleet.Width) + 1, rnd.Next(Fleet.Height) + 1);
                    if (Fleet.GetTile(currentSelection) != Symbol.BOAT)
                    {
                        Fleet.SetTile(currentSelection, Symbol.BOAT);
                        boatsPlaced++;
                    }
                }
            }
        }
        public void DoTurn(Player opponent)
        {
            Vector2 Target = new Vector2(1, 1);
            bool validSelection = false;
            string? message = null;
            Random random = new Random();

            Console.Clear();
            while (!validSelection)
            { 
                Target = isComputer ? new Vector2(random.Next(opponent.Fleet.Width) + 1, random.Next(opponent.Fleet.Height) + 1) : Targeting.SelectTile(message, Fleet.ToString(null, "\nTARGETING:", "YOUR FLEET:"), Target);
                if (validSelection = Targeting.GetTile(Target) == Symbol.EMPTY)
                {
                    if (opponent.Fleet.GetTile(Target) == Symbol.BOAT)
                    {
                        opponent.Fleet.SetTile(Target, Symbol.HIT);
                        Targeting.SetTile(Target, Symbol.HIT);
                        message = $"{Target} is a hit!";
                    }
                    else
                    {
                        Targeting.SetTile(Target, Symbol.MISS);
                        message = $"{Target} is a miss!";
                    }
                }
                else
                {
                    message = "You cannot select a position you have fired at before!";
                } 
            }
            message = isComputer ? opponent.Name + " is aiming...\n" + message : message;
            Console.WriteLine("\b" + message);
            Utilities.Continue();
        }
        public bool CheckIfWon(Player opponent)
        {
            bool won = false;
            int boats_remaining = 0;
            for (int i = 1; i <= opponent.Fleet.Width; i++)
            {
                for (int j = 1; j <= opponent.Fleet.Height; j++)
                {
                    if (opponent.Fleet.GetTile(new(i, j)) == Symbol.BOAT) boats_remaining++;
                }
            }
            won = boats_remaining == 0;
            return won;
        }
    }
}
