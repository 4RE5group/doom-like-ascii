using System;
using System.Drawing;
using System.Windows.Forms;

namespace DoomLike
{
    public class GameWindow : Form
    {
        public int SCREEN_WIDTH = 800;
        public int SCREEN_HEIGHT = 600;

        public float playerPosX = 5.0f;
        public float playerPosY = 5.0f;
        public float playerRotX = 0.0f;

        public float fieldOfView = 90f;
        public float renderDistance = 20.0f;
		
		
		// wall img
		public static Graphics g;
		public static Bitmap img;
		

        public string[] map = new string[]
        {
            "###################",
            "#                 #",
            "#     ####        #",
            "#     #           #",
            "#     #     ####  #",
            "#                 #",
            "###################"
        };

        private Timer gameLoop;

		public static void drawWallColumn(int column, int outputX, int outputY) 
		{
			for (int y = 0; y < img.Height; y++) {
				Color c = img.GetPixel(column, y);
				using (SolidBrush brush = new SolidBrush(c)) {
					g.FillRectangle(brush, outputX, outputY + y, 1, 1);
				}
			}
		}

        public GameWindow()
        {
            this.DoubleBuffered = true;
            this.Width = SCREEN_WIDTH;
            this.Height = SCREEN_HEIGHT;
            this.Text = "Doom Like";
            this.BackColor = Color.Black;
            this.KeyDown += OnKeyDown;
			this.MouseMove += onMouseMove;

			img = new Bitmap("wall.png");
			g = this.CreateGraphics();
			
            gameLoop = new Timer();
            gameLoop.Interval = 16; // ~60 FPS
            gameLoop.Tick += (s, e) => this.Invalidate(); // Redraw
            gameLoop.Start();
        }

		private void onMouseMove(object sender, MouseEventArgs e) {
			Point mouseDownLocation = new Point(e.X, e.Y);
			Console.WriteLine(e.X.ToString()+","+e.Y.ToString());
		}

        private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Right)
				playerRotX += 0.05f;
			else if (e.KeyCode == Keys.Left)
				playerRotX -= 0.05f;
			else if (e.KeyCode == Keys.W)
			{
				playerPosX += (float)(Math.Cos(playerRotX) * 0.2f);
				playerPosY += (float)(Math.Sin(playerRotX) * 0.2f);
			}
			else if (e.KeyCode == Keys.S)
			{
				playerPosX -= (float)(Math.Cos(playerRotX) * 0.2f);
				playerPosY -= (float)(Math.Sin(playerRotX) * 0.2f);
			} else if(e.KeyCode == Keys.Q) {
				drawWallColumn(0, 0, 0);
				System.Threading.Thread.Sleep(1000);
			}
		}

        protected override void OnPaint(PaintEventArgs e)
        {
            for (int i = 0; i < SCREEN_WIDTH; i += 2)
            {
                float angle = playerRotX - (fieldOfView / 2) * (float)Math.PI / 180f + (i * (fieldOfView / SCREEN_WIDTH) * (float)Math.PI / 180f);
                float rayDistance = CastRay(angle);

                if (rayDistance > 0)
                {
					float[] color = new float[] { 255, 255, 255 };					
					for(int c=0; c<3; c++) {
						float a = 255*(rayDistance/renderDistance)*2;
						if(a>255) 
							a=255;
						color[c] = a;
						// d=0 > c=255 d=RENDER_DIST > c=0
						// (d/renderDistance)*255 = c
					}
					// drawWallColumn(i, );
                    // DisplayVLine(i, rayDistance, Color.FromArgb((int)color[0], (int)color[1], (int)color[2]));
                } else {
					DisplayVLine(i, rayDistance, Color.FromArgb(0, 0, 0));
				}
            }
        }

        public float CastRay(float angle)
        {
            float rayX = playerPosX;
            float rayY = playerPosY;
            float step = 0.05f;
            float dist = 0;

            float dirX = (float)Math.Cos(angle);
            float dirY = (float)Math.Sin(angle);

            while (dist < renderDistance)
            {
                rayX += dirX * step;
                rayY += dirY * step;
                dist += step;

                if (rayX < 0 || rayY < 0 || rayX >= map[0].Length || rayY >= map.Length)
                    return -1;

                if (map[(int)rayY][(int)rayX] == '#')
                    return dist;
            }

            return -1;
        }

        public void DisplayVLine(int x, float distance, Color col)
        {
            int lineHeight = (int)(SCREEN_HEIGHT / distance);
            int startY = SCREEN_HEIGHT / 2 - lineHeight / 2;

            using (Pen pen = new Pen(col))
            {
                g.DrawLine(pen, x, startY, x, startY + lineHeight);
            }
        }
    }

    static class Program
    {
        [STAThread]
        public static void Main()
        {
            Console.WriteLine("Launching game...");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameWindow());
        }
    }
}
