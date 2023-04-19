﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Gaming.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FinalProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {

        }
        public MainPage()
        {
            this.InitializeComponent();

            Window.Current.CoreWindow.KeyDown += Canvas_KeyDown;
            Window.Current.CoreWindow.KeyUp += Canvas_KeyUp;

            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().SetDesiredBoundsMode(Windows.UI.ViewManagement.ApplicationViewBoundsMode.UseCoreWindow);
        }

        Tank tank1;
        Tank tank2;
        private CanvasBitmap tankimage;
        private CanvasBitmap tankimage2;
        private CanvasBitmap tankimage3;
        private CanvasBitmap tankimage4;
        private CanvasBitmap ballImage;
        private CanvasBitmap bluetankimage;
        private CanvasBitmap bluetankimage2;
        private CanvasBitmap bluetankimage3;
        private CanvasBitmap bluetankimage4;
        Ball bullet1;
        Ball bullet2;
        bool isCollides = false;
        /*Wall leftwall;
        Wall rightwall;
        Wall bottomwall;
        Wall topwall;*/
        WallCollection everyWall;
        private Gamepad controller;
        private Gamepad controller2;
        CanvasTextFormat canvasScoreTextFormat;

        private void Canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            tank1.Draw(args.DrawingSession);
            tank2.Draw(args.DrawingSession);
            bullet1.Draw(args.DrawingSession);
            bullet2.Draw(args.DrawingSession);
            foreach (var wall in everyWall.GetWalls())
            {
                wall.Draw(args.DrawingSession);
            }
            canvasScoreTextFormat.FontSize = 15;
            canvasScoreTextFormat.FontFamily = "cambria";
            args.DrawingSession.DrawText($"Player One's life: {tank1.score}", 70, 10, Colors.Red, canvasScoreTextFormat);
            args.DrawingSession.DrawText($"Player Two's life: {tank2.score}", 600, 10, Colors.Red, canvasScoreTextFormat);

        }

        public bool Intersects(Rect r1, Rect r2)
        {
            r1.Intersect(r2);
            if (r1.IsEmpty)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public void HandleCollision(Tank tankObj, Rect tankRect, GamepadReading reading)
        {
            foreach (var wall in everyWall.GetWalls())
            {
                if (Intersects(wall.rect, tankRect))
                {
                    isCollides = true;
                    if ((int)reading.LeftThumbstickX < 0)
                    {//Left
                        tankObj.TravelingLeftward = false;
                        tankObj.X = wall.X0 + 25 * wall.WIDTH;
                    }
                    else if ((int)reading.LeftThumbstickX > 0)
                    {//Right
                        tankObj.TravelingRightward = false;
                        tankObj.X = wall.X0 - 25 * wall.WIDTH;
                    }
                    if ((int)reading.LeftThumbstickY > 0)
                    {//Up
                        tankObj.TravelingUpward = false;
                        tankObj.Y = wall.Y0 + 25 * wall.WIDTH;
                    }
                    else if ((int)reading.LeftThumbstickY < 0)
                    {//Down
                        tankObj.TravelingDownward = false;
                        tankObj.Y = wall.Y0 - 25 * wall.WIDTH;
                    }
                }
            }
        }
        private void Canvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            tank1.Update();
            tank2.Update();
            bullet1.Update();
            bullet2.Update();
            // tankOneScoreTextBlock.Text = $"Tank two's life: {tank2.score}";
            //tankOneScoreTextBlock.Text = $"Tank one's life: {tank.score}";

            Rect tank1Rect = new Rect(new Point(tank1.X, tank1.Y), tank1.image.Size);
            Rect tank2Rect = new Rect(new Point(tank2.X, tank2.Y), tank2.image.Size);
            Rect bulletRect = new Rect(bullet1.X, bullet1.Y, 10, 10);
            Rect bullet2Rect = new Rect(bullet2.X, bullet2.Y, 10, 10);

            if (Intersects(bulletRect, tank2Rect))
            {
                isCollides = true;
                tank2.score--;
                bullet1.X = 1920;
                bullet1.TravelingLeftward = false;
                bullet1.TravelingRightward = false;
                bullet1.TravelingUpward = false;
                bullet1.TravelingDownward = false;
            }

            if (Intersects(bullet2Rect, tank1Rect))
            {
                isCollides = true;
                tank1.score--;
                bullet2.X = 2000;
                bullet2.TravelingLeftward = false;
                bullet2.TravelingRightward = false;
                bullet2.TravelingUpward = false;
                bullet2.TravelingDownward = false;
            }

            if (Intersects(tank1Rect, tank2Rect))
            {
                tank1.X -= 5;
                tank2.X += 5;
                tank1.Y -= 5;
                tank2.Y += 5;
            }

            // From https://github.com/EricCharnesky/CIS297-Winter2023/blob/main/XAMLAnimatedCanvasPong/XAMLAnimatedCanvasPong/Pong.cs with some modification
            if (Gamepad.Gamepads.Count > 0)
            {
                controller = Gamepad.Gamepads.First();
                var reading = controller.GetCurrentReading();

                HandleCollision(tank1, tank1Rect, reading);

                tank1.X += (int)(reading.LeftThumbstickX * 5);
                tank1.Y += (int)(reading.LeftThumbstickY * -5);

                if ((int)reading.LeftThumbstickX < 0)
                {
                    tank1.TravelingLeftward = true;
                }
                else
                {
                    tank1.TravelingLeftward = false;
                }
                if ((int)reading.LeftThumbstickX > 0)
                {
                    tank1.TravelingRightward = true;
                }
                else
                {
                    tank1.TravelingRightward = false;
                }
                if ((int)reading.LeftThumbstickY > 0)
                {
                    tank1.TravelingUpward = true;
                }
                else
                {
                    tank1.TravelingUpward = false;
                }
                if ((int)reading.LeftThumbstickY < 0)
                {
                    tank1.TravelingDownward = true;
                }
                else
                {
                    tank1.TravelingDownward = false;
                }
                if (reading.RightTrigger > 0)
                {
                    if (bullet1.X > 1200 || bullet1.X < 0 || bullet1.Y < 0 || bullet1.Y > 950)
                    {
                        if (tank1.image == tankimage4)
                        {
                            bullet1.X = tank1.X + 50;
                            bullet1.Y = tank1.Y + 65;
                            bullet1.TravelingDownward = true;
                            bullet1.TravelingLeftward = false;
                            bullet1.TravelingUpward = false;
                            bullet1.TravelingRightward = false;
                        }
                        if (tank1.image == tankimage2)
                        {
                            bullet1.X = tank1.X + 10;
                            bullet1.Y = tank1.Y + 34;
                            bullet1.TravelingLeftward = true;
                            bullet1.TravelingDownward = false;
                            bullet1.TravelingUpward = false;
                            bullet1.TravelingRightward = false;
                        }
                        if (tank1.image == tankimage3)
                        {
                            bullet1.X = tank1.X + 32;
                            bullet1.Y = tank1.Y + 15;
                            bullet1.TravelingUpward = true;
                            bullet1.TravelingRightward = false;
                            bullet1.TravelingLeftward = false;
                            bullet1.TravelingDownward = false;
                        }
                        if (tank1.image == tankimage)
                        {
                            bullet1.X = tank1.X + 80;
                            bullet1.Y = tank1.Y + 30;
                            bullet1.TravelingRightward = true;
                            bullet1.TravelingLeftward = false;
                            bullet1.TravelingDownward = false;
                            bullet1.TravelingUpward = false;
                        }
                    }

                }
            }

            // From https://github.com/EricCharnesky/CIS297-Winter2023/blob/main/XAMLAnimatedCanvasPong/XAMLAnimatedCanvasPong/Pong.cs with some modification
            if (Gamepad.Gamepads.Count > 1)
            {
                controller2 = Gamepad.Gamepads.ElementAt(1);
                var reading = controller2.GetCurrentReading();

                HandleCollision(tank2, tank1Rect, reading);

                tank2.X += (int)(reading.LeftThumbstickX * 5);
                tank2.Y += (int)(reading.LeftThumbstickY * -5);

                if ((int)reading.LeftThumbstickX < 0)
                {
                    tank2.TravelingLeftward = true;
                }
                else
                {
                    tank2.TravelingLeftward = false;
                }
                if ((int)reading.LeftThumbstickX > 0)
                {
                    tank2.TravelingRightward = true;
                }
                else
                {
                    tank2.TravelingRightward = false;
                }
                if ((int)reading.LeftThumbstickY > 0)
                {
                    tank2.TravelingUpward = true;
                }
                else
                {
                    tank2.TravelingUpward = false;
                }
                if ((int)reading.LeftThumbstickY < 0)
                {
                    tank2.TravelingDownward = true;
                }
                else
                {
                    tank2.TravelingDownward = false;
                }

                if (reading.RightTrigger > 0)
                {
                    if (bullet2.X > 1200 || bullet2.X < 0 || bullet2.Y < 0 || bullet2.Y > 950)
                    {
                        if (tank2.image == bluetankimage4)
                        {
                            bullet2.X = tank2.X + 50;
                            bullet2.Y = tank2.Y + 65;
                            bullet2.TravelingDownward = true;
                            bullet2.TravelingLeftward = false;
                            bullet2.TravelingUpward = false;
                            bullet2.TravelingRightward = false;
                        }
                        if (tank2.image == bluetankimage2)
                        {
                            bullet2.X = tank2.X + 10;
                            bullet2.Y = tank2.Y + 34;
                            bullet2.TravelingLeftward = true;
                            bullet2.TravelingDownward = false;
                            bullet2.TravelingUpward = false;
                            bullet2.TravelingRightward = false;
                        }
                        if (tank2.image == bluetankimage3)
                        {
                            bullet2.X = tank2.X + 32;
                            bullet2.Y = tank2.Y + 15;
                            bullet2.TravelingUpward = true;
                            bullet2.TravelingRightward = false;
                            bullet2.TravelingLeftward = false;
                            bullet2.TravelingDownward = false;
                        }
                        if (tank2.image == bluetankimage)
                        {
                            bullet2.X = tank2.X + 80;
                            bullet2.Y = tank2.Y + 30;
                            bullet2.TravelingRightward = true;
                            bullet2.TravelingLeftward = false;
                            bullet2.TravelingDownward = false;
                            bullet2.TravelingUpward = false;
                        }
                    }
                }
            }

        }
        private void Canvas_CreateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResources(sender).AsAsyncAction());
        }

        // ball.jpg From https://github.com/EricCharnesky/CIS297-Winter2023/blob/main/XAMLAnimatedCanvasPong/XAMLAnimatedCanvasPong/Pong.cs 
        // red tank from https://www.stockunlimited.com/vector-illustration/game-tank_1959541.html
        //blue tank and the rotation of it are from image resizer, backgroung remover, and image flipper websites 
        async Task CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender)
        {
            tankimage = await CanvasBitmap.LoadAsync(sender, "Assets/redtankRight.png");
            tankimage2 = await CanvasBitmap.LoadAsync(sender, "Assets/redtankLeft.png");
            tankimage3 = await CanvasBitmap.LoadAsync(sender, "Assets/redtankTop.png");
            tankimage4 = await CanvasBitmap.LoadAsync(sender, "Assets/redtankBottom.png");
            ballImage = await CanvasBitmap.LoadAsync(sender, "Assets/ball.jpg");

            bluetankimage = await CanvasBitmap.LoadAsync(sender, "Assets/bluetankRight.png");
            bluetankimage2 = await CanvasBitmap.LoadAsync(sender, "Assets/bluetankLeft.png");
            bluetankimage3 = await CanvasBitmap.LoadAsync(sender, "Assets/bluetankTop.png");
            bluetankimage4 = await CanvasBitmap.LoadAsync(sender, "Assets/bluetankBottom.png");

            tank1 = new Tank(50, 300, 5, tankimage, tankimage2, tankimage, tankimage3, tankimage4);
            tank2 = new Tank(500, 300, 5, bluetankimage2, bluetankimage2, bluetankimage, bluetankimage3, bluetankimage4);
            bullet1 = new Ball(2200, tank1.Y, 5, ballImage);

            everyWall = new WallCollection();
            everyWall.Add(new Wall(20, 10, 20, 750, Colors.CornflowerBlue));    //left
            everyWall.Add(new Wall(930, 10, 930, 520, Colors.CornflowerBlue));  //right
            everyWall.Add(new Wall(20, 10, 1510, 10, Colors.CornflowerBlue));   //top
            everyWall.Add(new Wall(20, 520, 930, 520, Colors.CornflowerBlue));  //bottom
            bullet2 = new Ball(2200, 200, 5, ballImage);
            //200->tank.Y ?
            canvasScoreTextFormat = new CanvasTextFormat();
        }


        private void Canvas_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs e)
        {
            if (e.VirtualKey == Windows.System.VirtualKey.Left)
            {
                tank2.TravelingLeftward = true;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Right)
            {
                tank2.TravelingRightward = true;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Up)
            {
                tank2.TravelingUpward = true;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Down)
            {
                tank2.TravelingDownward = true;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.D)
            {
                tank1.TravelingRightward = true;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.W)
            {
                tank1.TravelingUpward = true;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.S)
            {
                tank1.TravelingDownward = true;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.A)
            {
                tank1.TravelingLeftward = true;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.NumberPad2)
            {
                if (bullet2.X > 1900 || bullet2.X < 0 || bullet2.Y < 0 || bullet2.Y > 950)
                {
                    if (tank2.image == tankimage4)
                    {
                        bullet2.X = tank2.X + 60;
                        bullet2.Y = tank2.Y + 100;

                        bullet2.TravelingDownward = true;
                        bullet2.TravelingLeftward = false;
                        bullet2.TravelingUpward = false;
                        bullet2.TravelingRightward = false;
                    }
                    if (tank2.image == tankimage2)
                    {
                        bullet2.X = tank2.X + 15;
                        bullet2.Y = tank2.Y + 60;

                        bullet2.TravelingLeftward = true;
                        bullet2.TravelingDownward = false;
                        bullet2.TravelingUpward = false;
                        bullet2.TravelingRightward = false;
                    }
                    if (tank2.image == tankimage3)
                    {
                        bullet2.X = tank2.X + 60;
                        bullet2.Y = tank2.Y + 15;

                        bullet2.TravelingUpward = true;
                        bullet2.TravelingRightward = false;
                        bullet2.TravelingLeftward = false;
                        bullet2.TravelingDownward = false;
                    }
                    if (tank2.image == tankimage)
                    {
                        bullet2.X = tank2.X + 100;
                        bullet2.Y = tank2.Y + 60;
                        bullet2.TravelingRightward = true;
                        bullet2.TravelingLeftward = false;
                        bullet2.TravelingDownward = false;
                        bullet2.TravelingUpward = false;
                    }
                }
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Space)
            {
                if (bullet1.X > 1200 || bullet1.X < 0 || bullet1.Y < 0 || bullet1.Y > 950)
                {
                    if (tank1.image == tankimage4)
                    {
                        bullet1.X = tank1.X + 60;
                        bullet1.Y = tank1.Y + 100;

                        bullet1.TravelingDownward = true;
                        bullet1.TravelingLeftward = false;
                        bullet1.TravelingUpward = false;
                        bullet1.TravelingRightward = false;
                    }
                    if (tank1.image == tankimage2)
                    {
                        bullet1.X = tank1.X + 15;
                        bullet1.Y = tank1.Y + 60;

                        bullet1.TravelingLeftward = true;
                        bullet1.TravelingDownward = false;
                        bullet1.TravelingUpward = false;
                        bullet1.TravelingRightward = false;
                    }
                    if (tank1.image == tankimage3)
                    {
                        bullet1.X = tank1.X + 60;
                        bullet1.Y = tank1.Y + 15;

                        bullet1.TravelingUpward = true;
                        bullet1.TravelingRightward = false;
                        bullet1.TravelingLeftward = false;
                        bullet1.TravelingDownward = false;
                    }
                    if (tank1.image == tankimage)
                    {
                        bullet1.X = tank1.X + 100;
                        bullet1.Y = tank1.Y + 60;
                        bullet1.TravelingRightward = true;
                        bullet1.TravelingLeftward = false;
                        bullet1.TravelingDownward = false;
                        bullet1.TravelingUpward = false;
                    }
                }
            }
        }

        private void Canvas_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs e)
        {
            if (e.VirtualKey == Windows.System.VirtualKey.Left)
            {
                tank2.TravelingLeftward = false;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Right)
            {
                tank2.TravelingRightward = false;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Up)
            {
                tank2.TravelingUpward = false;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Down)
            {
                tank2.TravelingDownward = false;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.D)
            {
                tank1.TravelingRightward = false;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.W)
            {
                tank1.TravelingUpward = false;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.S)
            {
                tank1.TravelingDownward = false;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.A)
            {
                tank1.TravelingLeftward = false;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Space)
            {

            }
            else if (e.VirtualKey == Windows.System.VirtualKey.NumberPad2)
            {

            }
        }

    }
}
