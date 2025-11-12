using System;
using System.Collections.Generic;
using System.Linq;

namespace Slowshooter
{
    internal class Program
    {
        static bool spikePressedP1 = false;
        static bool spikePressedP2 = false;

        static List<(int, int)> spikeListP1 = new List<(int, int)>();
        static List<(int, int)> spikeListP2 = new List<(int, int)>();


        static string playField =
 @"+----------+   +----------+
|          |   |          |
|          |   |          |
|          |   |          |
|          |   |          |
|          |   |          |
|          |   |          |
|          |   |          |
|          |   |          |
|          |   |          |
|          |   |          |
+----------+   +----------+";

        static bool isPlaying = true;

        // player input 
        static int p1_x_input;
        static int p1_y_input;

        static int p2_x_input;
        static int p2_y_input;

        // player 1 pos
        static int p1_x_pos = 5;
        static int p1_y_pos = 5;

        // player 2 pos
        static int p2_x_pos = 20;
        static int p2_y_pos = 5;

        // bounds for player movement
        static (int, int) p1_min_max_x = (1, 10);
        static (int, int) p1_min_max_y = (1, 10);
        static (int, int) p2_min_max_x = (16, 25);
        static (int, int) p2_min_max_y = (1, 10);

        // what turn is it? will be 0 after game is drawn the first time
        static int turn = -1;

        // contains the keys that player 1 and player 2 are allowed to press
        static (char[], char[]) allKeybindings = (new char[]{ 'W', 'A', 'S', 'D', 'E' }, new char[]{ 'J', 'I', 'L', 'K', 'O' });
        static ConsoleColor[] playerColors = { ConsoleColor.Red, ConsoleColor.Blue };

        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            while(isPlaying)
            {
                ProcessInput();
                Update();
                Draw();
            }
        }

        static void ProcessInput()
        {
            // if this isn't here, input will block the game before drawing for the first time
            if (turn == -1) return;

            // reset input
            p1_x_input = 0;
            p1_y_input = 0;
            p2_x_input = 0;
            p2_y_input = 0;

            char[] allowedKeysThisTurn; // different keys allowed on p1 vs. p2 turn

            // choose which keybindings to use
            if (turn % 2 == 0) allowedKeysThisTurn = allKeybindings.Item1;
            else allowedKeysThisTurn = allKeybindings.Item2;

            // get the current player's input
            ConsoleKey input = ConsoleKey.NoName;
            while (!allowedKeysThisTurn.Contains(((char)input)))
            {
                input = Console.ReadKey(true).Key;
            }

            // check all input keys 
            if (input == ConsoleKey.A) p1_x_input = -1;
            if (input == ConsoleKey.D) p1_x_input = 1;
            if (input == ConsoleKey.W) p1_y_input = -1;
            if (input == ConsoleKey.S) p1_y_input = 1;
            if (input == ConsoleKey.E) spikePressedP1 = true;
          
            if (input == ConsoleKey.J) p2_x_input = -1;
            if (input == ConsoleKey.L) p2_x_input = 1;
            if (input == ConsoleKey.I) p2_y_input = -1;
            if (input == ConsoleKey.K) p2_y_input = 1;
            if (input == ConsoleKey.O) spikePressedP2 = true;

        }

        static void Update()
        {
            // update players' positions based on input
            p1_x_pos += p1_x_input;
            p1_x_pos = p1_x_pos.Clamp(p1_min_max_x.Item1, p1_min_max_x.Item2);

            p1_y_pos += p1_y_input;
            p1_y_pos = p1_y_pos.Clamp(p1_min_max_y.Item1, p1_min_max_y.Item2);

            p2_x_pos += p2_x_input;
            p2_x_pos = p2_x_pos.Clamp(p2_min_max_x.Item1, p2_min_max_x.Item2);

            p2_y_pos += p2_y_input;
            p2_y_pos = p2_y_pos.Clamp(p2_min_max_y.Item1, p2_min_max_y.Item2);

            if (spikePressedP1 == true)
            {
                SpawnSpikeP1();
                Console.Write("X");

                spikePressedP1 = false;
            }
            if (spikePressedP2 == true)
            {
                SpawnSpikeP2();
                Console.Write("X");

                spikePressedP2 = false;
            }

            turn += 1;
        }

        static void Draw()
        {
            // draw the background (playfield)
            Console.SetCursorPosition(0, 0);
            Console.Write(playField);

            // draw player 1
            Console.SetCursorPosition(p1_x_pos, p1_y_pos);
            Console.ForegroundColor = playerColors[0];
            Console.Write("O");

            // draw player 2
            Console.SetCursorPosition(p2_x_pos, p2_y_pos);
            Console.ForegroundColor = playerColors[1];
            Console.Write("O");

            // draw the Turn Indicator
            Console.SetCursorPosition(3, 12);
            Console.ForegroundColor = playerColors[turn % 2];

            Console.Write($"PLAYER {turn % 2 + 1}'S TURN!");


            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nUSE WASD or IJKL to move");
            Console.ForegroundColor = ConsoleColor.White;

            //Draw Spikes
            foreach (var spike in spikeListP1)
            {
                Console.SetCursorPosition(spike.Item1, spike.Item2);
                Console.Write("X");
            }

            foreach (var spike in spikeListP2)
            {
                Console.SetCursorPosition(spike.Item1, spike.Item2);
                Console.Write("X");
            }

        }

        static void SpawnSpikeP1()
        {
           
            Random spikeX = new Random();
            Random spikeY = new Random();

            int randomSpikeX = spikeX.Next(16,25);
            int randomSpikeY = spikeY.Next(1, 10);

            spikeListP1.Add((randomSpikeX, randomSpikeY));

        }
        static void SpawnSpikeP2()
        {

            Random spikeX = new Random();
            Random spikeY = new Random();

            int randomSpikeX = spikeX.Next(1, 10);
            int randomSpikeY = spikeY.Next(1, 10);

            spikeListP2.Add((randomSpikeX, randomSpikeY));

        }
    }
}
