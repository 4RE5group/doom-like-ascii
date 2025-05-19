using System;

namespace DoomLike
{
    public class Program
    {
        public static int SCREEN_WIDTH = 100;
        public static int SCREEN_HEIGHT = 25;
        public static int MAP_SIZE = 10;

        public static float playerPosX = 5.0f;
        public static float playerPosY = 5.0f;
        public static float playerRotX = 0.0f; // Player rotation (in radians)

        public static float fieldOfView = 90f; // Field of view in degrees
        public static float renderDistance = 10.0f;

        public static string[,] screenBuffer = new string[SCREEN_WIDTH, SCREEN_HEIGHT];
        // Map is a 2D array where '#' is a wall and ' ' is empty space
        public static string[] map = new string[]
        {
            "###################",
            "#                 #",
            "#     ####        #",
            "#     #           #",
            "#     #     ####  #",
            "#                 #",
            "###################"
        };

        // Clear the screen buffer
        public static void clearScreen()
        {
            // No need to manually clear the screen buffer as we will use Console.Clear() directly
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                for (int y = 0; y < SCREEN_HEIGHT; y++)
                {
                    screenBuffer[x, y] = " ";  // Fill the screen buffer with empty spaces
                }
            }
        }

        public static void displayScreen()
        {
            Console.SetCursorPosition(0, 0);  // clear screen
            // Display the screen buffer
            for (int y = 0; y < SCREEN_HEIGHT; y++)
            {
                string line = "";
                for (int x = 0; x < SCREEN_WIDTH; x++)
                {
                    line += screenBuffer[x, y];
                }
                Console.WriteLine(line);
            }
        }

        public static void displayVLine(int c, float rayDistance, string character)
        {
            // Ensure the column `c` is within valid screen boundaries
            if (c < 0 || c >= SCREEN_WIDTH || rayDistance <= 0)
                return;

            // Convert the ray distance to a screen height
            int height = (int)(SCREEN_HEIGHT / rayDistance); // The closer the wall, the taller it appears

            // Draw the vertical line for the column `c`
            int startRow = SCREEN_HEIGHT / 2 - height / 2;  // The start row of the vertical line
            int endRow = startRow + height - 1;             // The end row of the vertical line

            // Bound the rows to the valid screen height
            startRow = Math.Max(0, startRow);
            endRow = Math.Min(SCREEN_HEIGHT - 1, endRow);

            // Draw the line in the screen buffer
            for (int row = startRow; row <= endRow; row++)
            {
                screenBuffer[c, row] = character;
            }
        }

        public static float castRay(float playerRot)
        {
            float rayStep = 0.1f;

            // Calculate the direction of the ray
            float rayDirX = (float)Math.Cos(playerRot);
            float rayDirY = (float)Math.Sin(playerRot);

            // Initialize ray position starting from the player's position
            float rayPosX = playerPosX;
            float rayPosY = playerPosY;

            float distance = 0.0f;

            while (true)
            {
                rayPosX += rayDirX * rayStep;
                rayPosY += rayDirY * rayStep;
                distance += rayStep; // Increase the distance

                // Check for out-of-bounds or distance exceeded
                if ((int)rayPosX < 0 || (int)rayPosY < 0 || (int)rayPosX >= map[0].Length || (int)rayPosY >= map.Length || distance > renderDistance)
                    return -1.0f; // Return -1 to signify no wall hit

                // Check if the ray hits a wall (not a space)
                if (map[(int)rayPosY][(int)rayPosX] == '#')
                {
                    break; // Wall hit
                }
            }

            return distance; // Return the distance to the wall
        }

        public static void Main(string[] args)
        {
            // Main loop
            while (true)
            {
                // Clear the screen buffer to prepare for the next frame
                clearScreen();

                // Check if a key is pressed to adjust the player rotation
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;

                    if (key == ConsoleKey.RightArrow)
                    {
                        playerRotX += 0.05f; // Rotate right
                    }
                    else if (key == ConsoleKey.LeftArrow)
                    {
                        playerRotX -= 0.05f; // Rotate left
                    }
                }

                // Cast rays and display vertical lines based on ray distances
                for (int i = 0; i < SCREEN_WIDTH; i++)
                {
                    float angle = playerRotX - (fieldOfView / 2) * (float)Math.PI / 180f + (i * (fieldOfView / SCREEN_WIDTH) * (float)Math.PI / 180f); // Convert to radians
                    float rayDistance = castRay(angle); // Cast the ray and get the distance
                    if (rayDistance > 0)
                    {
                        displayVLine(i, rayDistance, "#"); // Display the vertical line
                    }
                }

                // Display the screen buffer
                displayScreen();

                // Slow down the loop to reduce CPU usage
                System.Threading.Thread.Sleep(10);
            }
        }
    }
}
