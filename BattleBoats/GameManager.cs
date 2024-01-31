using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBoats
{
    public class GameManager
    {
        public Player[] Players { get; set; }
        public int turnNum { get; set; } = 0;
        public string saveName { get; set; }
        public GameManager() 
        {
            Players = new Player[2];
            saveName = "default.json";
        }
        [JsonConstructor]
        public GameManager(Player[] players, int turn_num, string name)
        {
            Players = players;
            turnNum = turn_num;
            saveName = name;
        }
        public void GameLoop() // main loop of the game
        {
            Player? winner = null;
            Save();
            while (winner == null)
            {
                Player Playing = Players[turnNum % 2]; // MODULOO FOR THE WIIIn
                Player Opponent = Players[(turnNum + 1) % 2];
                Playing.DoTurn(Opponent);
                winner = Playing.CheckIfWon(Opponent) ? Playing : null;
                turnNum++;
                Save();
            } 
        }
        public void DoWinScreen(Player winner) // what happens when win
        {
            Console.Clear();
            if (winner == Players[0])
            {
                Console.WriteLine("gratz you win");
            }
            else Console.WriteLine("oops you lost");
            Utilities.Continue();

            Players[0].Fleet.Display(null, null, "ENEMY FLEET:");
            Players[0].Targeting.Display(null, null, "ENEMY TARGETING");
        }
        public void NewGame()
        {
            Console.Write("Name your opponent: ");
            string name = Console.ReadLine();
            if (name == "") name = "Bobot";

            Players[0] = new Player(new GameBoard(8,8), new GameBoard(8,8), false, name);
            Players[1] = new Player(true);

            Players[0].SetUpFleet();
            Players[1].SetUpFleet();

            GameLoop();
        }
        private void Save()
        {
            string json = JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            File.WriteAllText(saveName, json);
        }
        public static GameManager Load(string fileName)
        {
            string json = File.ReadAllText(fileName);
            GameManager toReturn = JsonConvert.DeserializeObject<GameManager>(json, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
            return toReturn;
        }
    }
}
