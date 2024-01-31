using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace BattleBoats
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Symbol
    {
        EMPTY,
        BOAT,
        HIT,
        MISS,
    }

    public class GameBoard
    {
        [JsonProperty]
        private Symbol[,] Board { get; set; }
        public int Width { get; }
        public int Height { get; }

        public GameBoard(int width, int height) // make sure to fill out the constructor later
        {
            Board = new Symbol[height, width];
            Width = width;
            Height = height;
        }
        private GameBoard() { Board = new Symbol[1,1];  Width = 1; Height = 1; }
        [JsonConstructor]
        public GameBoard(Symbol[,] board, int width, int height)
        {
            Board = board;
            Width = width;
            Height = height;
        }
        public void Display(Vector2? Selection = null, string? footer = null, string? header = null) // displays the board
        {
            if (header != null) Console.WriteLine(header);

            for (int i = 0; i < Height; i++)        
            {
                string toWrite = ""; // I concacenated a string here because it was faster to output and had less delay present between updating displays.
                for (int j = 0; j < Width; j++)
                {
                    bool CurrentlySelected;  // for your readability :)
                    if (Selection != null) 
                    {
                        if (CurrentlySelected = Selection.X == j + 1 && Selection.Y == i + 1)
                            toWrite += "# "; // what's nice about this is that there's no error checking needed if its out of range :)
                        else toWrite += Board[i, j].SymbolToString() + " ";
                    }
                    else toWrite += Board[i, j].SymbolToString() + " ";
                }
                Console.WriteLine(toWrite);
            }

            if (footer != null) Console.WriteLine(footer);
        }
        public string ToString(Vector2? Selection = null, string ? footer = null, string? header = null) // converts the board to a string and returns it
        {
            string output = "";
            if (header != null) output += header + "\n";

            for (int i = 0; i < Height; i++)
            {
                string toWrite = "";
                for (int j = 0; j < Width; j++)
                {
                    bool CurrentlySelected;
                    if (Selection != null)
                    {
                        if (CurrentlySelected = Selection.X == j + 1 && Selection.Y == i + 1)
                            toWrite += "# ";
                        else toWrite += Board[i, j].SymbolToString() + " ";
                    }
                    else toWrite += Board[i, j].SymbolToString() + " ";
                }
                output += toWrite + "\n";
            }

            if (footer != null) output += footer;
            return output;
        }
        public void SetTile(Vector2 position, Symbol newTile)
        {
            bool withinBounds = position.X <= Width && position.Y <= Height;
            if (withinBounds)
            {
                Board[position.Y - 1, position.X - 1] = newTile;
            }
            else
            {
                Console.WriteLine(position + " is out of range!");
            }
        }
        public Symbol GetTile(Vector2 position)
        {
            bool withinBounds = position.X <= Width && position.Y <= Height;
            Symbol Tile = Symbol.EMPTY;
            if (withinBounds)
            {
                Tile = Board[position.Y - 1,position.X - 1];
            }
            return Tile;
        }
        public Vector2 SelectTile(string? messageAfter = null, string? messageBefore = null, Vector2? initialSelection = null)
        {
            Vector2 selection = initialSelection != null ? initialSelection : new(1,1);
            bool selected = false;
            while (!selected)
            {
                Console.Clear();
                if (messageBefore != null) Console.WriteLine(messageBefore);
                Display(selection);
                if (messageAfter != null) Console.WriteLine(messageAfter);
                selection = Utilities.ChangeSelection(selection, Width, Height, out selected);
            }
            return selection;
        }
    }
}
