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

            const int width = 1200;
            const int height = 1200;

            RenderWindow window = new RenderWindow(new VideoMode(width, height), "Space Invaders");
            window.Closed += OnClose;
            window.KeyReleased += OnKeyReleased;

            Color windowColor = Color.Black;

            const int playerWidth = 80;
            const int playerHeight = 40;
            const float playerSpeed = 1;
            var player = new RectangleShape(new Vector2f(playerWidth, playerHeight))
            {
                //Position = new SFML.System.Vector2f(, 50),
                FillColor = Color.Green,
                Origin = new Vector2f(playerWidth / 2, playerHeight / 2),
                Position = new Vector2f(width / 2, height - (playerHeight / 2) - 100)
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

            var playerClock = new Clock();
            var playerElapsed = playerClock.ElapsedTime;
            var playerPreviousElapsed = playerElapsed;

            var bulletClock = new Clock();
            var bulletElapsed = playerClock.ElapsedTime;
            var bulletPreviousElapsed = playerElapsed;

            while (window.IsOpen)
            {
                window.DispatchEvents();

                playerElapsed = playerClock.ElapsedTime;
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
                    if (player.Position.X < playerWidth / 2)
                    {
                        player.Position = new Vector2f(playerWidth / 2, player.Position.Y);
                    }
                    if (player.Position.X > width - (playerWidth / 2))
                    {
                        player.Position = new Vector2f(width - (playerWidth / 2), player.Position.Y);
                    }
                }
                bulletElapsed = bulletClock.ElapsedTime;
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