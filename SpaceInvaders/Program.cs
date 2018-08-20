using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SpaceInvaders
{
    internal static class Program
    {
        private static bool Shooting;

        private static void Main(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            const int width = 800;
            const int height = 800;

            RenderWindow window = new RenderWindow(new VideoMode(width, height), "Space Invaders");
            //window.SetFramerateLimit(30);
            window.Closed += OnClose;
            window.KeyReleased += OnKeyReleased;

            Color windowColor = Color.Black;

            const int playerWidth = width / 10;
            const int playerHeight = height / 20;
            const float playerSpeed = 1;
            var player = new RectangleShape(new Vector2f(playerWidth, playerHeight))
            {
                //Position = new SFML.System.Vector2f(, 50),
                FillColor = Color.Green,
                Origin = new Vector2f(playerWidth / 2, playerHeight / 2),
                Position = new Vector2f(width / 2, height - (playerHeight / 2) - (height / 10))
            };

            const int bulletWidth = 5;
            const int bulletHeight = 20;
            const float bulletSpeed = 1.5f;
            var bullet = new RectangleShape(new Vector2f(bulletWidth, bulletHeight))
            {
                FillColor = Color.Green,
                Origin = new Vector2f(bulletWidth / 2, bulletHeight / 2),
                Position = player.Position
            };

            var clock = new Clock();
            var playerElapsed = clock.ElapsedTime;
            var playerPreviousElapsed = playerElapsed;
            var bulletElapsed = clock.ElapsedTime;
            var bulletPreviousElapsed = playerElapsed;

            var fpsCounter = 0;
            var fps = 0;
            //var fps = new Text(fpsCounter.ToString(), );
            var fpsElapsed = clock.ElapsedTime;
            var fpsPreviousElapsed = fpsElapsed;
            while (window.IsOpen)
            {
                fpsCounter++;
                window.DispatchEvents();

                playerElapsed = clock.ElapsedTime;
                if (playerElapsed.AsMilliseconds() > playerPreviousElapsed.AsMilliseconds())
                {
                    playerPreviousElapsed = playerElapsed;
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
                    {
                        player.Position += new Vector2f(-playerSpeed, 0);
                    }
                    else if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
                    {
                        player.Position += new Vector2f(playerSpeed, 0);
                    }
                    var playerBounds = player.GetGlobalBounds();
                    if (playerBounds.Left < 0)
                    {
                        player.Position = new Vector2f(playerWidth / 2, player.Position.Y);
                    }
                    if (playerBounds.Left + playerBounds.Width > width)
                    {
                        player.Position = new Vector2f(width - (playerWidth / 2), player.Position.Y);
                    }
                }
                bulletElapsed = clock.ElapsedTime;
                if (bulletElapsed.AsMilliseconds() > bulletPreviousElapsed.AsMilliseconds())
                {
                    bulletPreviousElapsed = bulletElapsed;
                    if (Shooting)
                    {
                        bullet.Position += new Vector2f(0, -bulletSpeed);
                        if (bullet.Position.Y + bulletHeight < 0)
                        {
                            Shooting = false;
                        }
                    }
                    else
                    {
                        bullet.Position = player.Position;
                    }
                }
                fpsElapsed = clock.ElapsedTime;
                if (fpsElapsed > fpsPreviousElapsed + Time.FromSeconds(1))
                {
                    fps = fpsCounter;
                    Console.WriteLine(fps);
                    fpsCounter = 0;
                    fpsPreviousElapsed = fpsElapsed;
                }

                window.Clear(windowColor);

                window.Draw(bullet);
                window.Draw(player);

                window.Display();
            }
        }

        private static void OnKeyReleased(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.Escape:
                    var window = (RenderWindow)sender;
                    window.Close();
                    break;
                case Keyboard.Key.Space:
                    Shooting = true;
                    break;
            }
        }

        private static void OnClose(object sender, EventArgs e)
        {
            var window = (RenderWindow)sender;
            window.Close();
        }
    }
}