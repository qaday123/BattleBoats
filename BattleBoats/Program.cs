using Newtonsoft.Json;

namespace BattleBoats
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Run();
        }
        static void Run() // put all the main body and menu stuff out here because it was easier to organise
        {
            int initialSelection;
            
            MenuManager Menu = new MenuManager();
            Menu.StartMenu();
            initialSelection = Menu.CurrentSelection;

            switch (initialSelection)
            {
                case 0:
                    Console.WriteLine("\bNew game selected");
                    NewGame();
                    break;
                case 1:
                    Console.WriteLine("\bResume game selected");
                    LoadGame();
                    break;
                case 2:
                    Instructions();
                    break;
                case 3:
                    Quit(); break;
                default:
                    Console.WriteLine("SOMETHING WENT WRONG");
                    break;
            }

            Console.Clear();
            Console.WriteLine("Thank you for playing BattleBoats.");
        }
        static void NewGame()
        {
            GameManager Game = new GameManager();
            Game.NewGame();
        }
        static void LoadGame()
        {
            GameManager Game = GameManager.Load("default.json");
            Game.GameLoop();
        }
        static void Instructions()
        {
            Vector2 ExampleSelection = new Vector2(1, 1);
            GameBoard Example = new GameBoard(8, 8);
            Random FancyRandom = new Random();

            ExampleSelection = Example.SelectTile(null, "Use the arrow keys or WASD to move around and make a selection. Press [Z] or [ENTER] to make a selection. Try it on this blank grid:");
            Console.WriteLine("\bYou selected the tile " + ExampleSelection);
            Utilities.Continue();

            ExampleSelection = new Vector2(FancyRandom.Next(8) + 1, FancyRandom.Next(8) + 1);
            Example.SetTile(ExampleSelection, Symbol.BOAT);
            Example.Display(null, null, "Your fleet will look like this, with a boat marked with a B, as shown.");
            Utilities.Continue();

            Example.SetTile(ExampleSelection, Symbol.HIT);
            Example.Display(null, "If you lose all your ships, you lose.", "If your opponent hits a boat, it will be replaced with an H.");
            Utilities.Continue();

            ExampleSelection = new Vector2(FancyRandom.Next(8) + 1, FancyRandom.Next(8) + 1);
            Example.SetTile(ExampleSelection, Symbol.MISS);
            ExampleSelection = new Vector2(FancyRandom.Next(8) + 1, FancyRandom.Next(8) + 1);
            Example.SetTile(ExampleSelection, Symbol.HIT);
            Example.Display(null, "Hit all of your opponents boats to win.", "On your targeting grid, you will be shown all of the spots you have hit and missed.");
            Utilities.Continue();

            Console.WriteLine("May the RNG gods be blessed on ye, and try to have fun.");
            Utilities.Continue();

            Run();
        }
        static void Quit()
        {
            Console.WriteLine("Quitting...");
            Environment.Exit(0);
        }
        public static void Test()
        {
            /*Player Player1 = new Player(new GameBoard(8, 8), new GameBoard(8, 9), false);
            Player1.SetUpFleet();
            Console.Clear();
            Player1.Fleet.Display();
            Console.WriteLine("Setup complete");//*/

            Player Player1 = new Player(true);
            Player Player2 = new Player(true);
            Player? winner = null;

            Player1.SetUpFleet();
            Player2.SetUpFleet();

            Player1.Fleet.Display();
            string fileName = "test.json";
            File.Create(fileName).Close();

            var json = JsonConvert.SerializeObject(Player1, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            File.WriteAllText(fileName, string.Empty);
            File.WriteAllText(fileName, json);

            Console.WriteLine("Data saved");

            Console.ReadLine();

            Player players;
            using (StreamReader r = new StreamReader(fileName))
            {
                string jsonS = r.ReadToEnd();
                players = JsonConvert.DeserializeObject(jsonS, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                }) as Player;
            }
            //Console.WriteLine(players.Name);
            players.Fleet.Display(null, "\n");

            //players[1].Fleet.Display();

            /*while (winner == null)
            {
                Player1.DoTurn(Player2);
                winner = Player1.CheckIfWon(Player2) ? Player1 : null; // then save
                Player2.DoTurn(Player1);
                winner = Player2.CheckIfWon(Player1) ? Player1 : null; // then save
            }
            Console.Clear();
            Console.WriteLine("Winner is " + winner);

            /*Player1.SetUpFleet();

            Console.Clear();
            Console.WriteLine(Player1.Fleet.ToString());
            Console.WriteLine("Setup complete");*/

        }

        /*public static void Test1()
        {
            Player Player1 = new Player(new GameBoard(8, 8), new GameBoard(8, 9), false);

            Player1.Fleet.SetTile(new Vector2(3, 2), Symbol.BOAT);
            Player1.Fleet.SetTile(new Vector2(6, 7), Symbol.BOAT);
            Player1.Fleet.SetTile(new Vector2(10, 2), Symbol.BOAT);

            Player1.Targeting.SetTile(new Vector2(1, 7), Symbol.MISS);
            Player1.Targeting.SetTile(new Vector2(6, 3), Symbol.HIT);

            Vector2 Selecting = new Vector2(1, 1);
            bool selected = false;

            while (!selected)
            {
                Console.Clear();
                Player1.Fleet.Display();
                Console.WriteLine("");
                Player1.Targeting.Display(Selecting);
                Selecting = Utilities.ChangeSelection(Selecting, Player1.Targeting.Width, Player1.Targeting.Height, out selected);
            }
            Console.WriteLine(Player1.Fleet.GetTile(Selecting) + " " + Selecting);

            if (Player1.Fleet.GetTile(Selecting) == Symbol.BOAT) Console.WriteLine("\nHit!");
            else Console.WriteLine("Miss!");
        }*/
    }
}