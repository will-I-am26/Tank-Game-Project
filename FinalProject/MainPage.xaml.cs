﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
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
        }

        Tank tank;
        Tank tank2;
        private CanvasBitmap tankimage;
        private CanvasBitmap tankimage2;
        private CanvasBitmap tankimage3;
        private CanvasBitmap tankimage4;
        private CanvasBitmap ballImage;
        Ball bullet;
        Ball bullet2;
        bool isCollides = false;
        Wall leftwall;
        Wall rightwall;
        Wall bottomwall;
        Wall topwall;
        private Gamepad controller;
        private Gamepad controller2;

        private void Canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            tank.Draw(args.DrawingSession);
            tank2.Draw(args.DrawingSession);
            bullet.Draw(args.DrawingSession);
            leftwall.Draw(args.DrawingSession);
            rightwall.Draw(args.DrawingSession);
            bottomwall.Draw(args.DrawingSession);
            topwall.Draw(args.DrawingSession);
            bullet2.Draw(args.DrawingSession);
            args.DrawingSession.DrawText($"Player One's life: {tank.score}", 300, 600, Colors.Red);
            args.DrawingSession.DrawText($"Player Two's life: {tank2.score}", 600, 600, Colors.Red);
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
        public void HandleCollision(Tank tankObj, Rect tankRect, Rect leftwallRect, Rect rightwallRect1, Rect bottomwallRect1, Rect topwallRect1, Rect bulletRect)
        {
            
            if (Intersects(leftwallRect, tankRect))
            {
                isCollides = true;
                tankObj.TravelingLeftward = false;
                tankObj.X = leftwall.X0 + leftwall.WIDTH;

            }
            else if (Intersects(rightwallRect1, tankRect))
            {
                isCollides = true;
                tankObj.TravelingRightward = false;
                tankObj.X=rightwall.X0 - 25*rightwall.WIDTH;
            }
            if (Intersects(bottomwallRect1, tankRect))
            {
                isCollides = true;
                tankObj.TravelingDownward= false;
                //tankObj.Y = bottomwall.Y1 - 100*bottomwall.WIDTH;
                
                tankObj.Y = bottomwall.Y0 - 25 * bottomwall.WIDTH;
                
            }
            else if (Intersects(topwallRect1, tankRect))
            {
                isCollides = true;
                tankObj.TravelingUpward = false;
                tankObj.Y = topwall.Y0 +topwall.WIDTH;
            }

        }
        private void Canvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            tank.Update();
            tank2.Update();
            bullet.Update();
            bullet2.Update();
            
            Rect leftwallrect = new Rect(leftwall.X0, leftwall.Y0, leftwall.WIDTH, leftwall.Y1 - leftwall.Y0);
            Rect rightwallrect = new Rect(rightwall.X0, rightwall.Y0, rightwall.WIDTH, rightwall.Y1 - rightwall.Y0);
            Rect bottomwallrect = new Rect(new Point(bottomwall.X0,bottomwall.Y0), new Point(bottomwall.X1,bottomwall.Y1));
            
            Rect topwallrect = new Rect(new Point(topwall.X0, topwall.Y0), new Point(topwall.X1, topwall.Y1));
            Rect tank1Rect = new Rect(new Point(tank.X, tank.Y), tank.image.Size);
            Rect tank2Rect = new Rect(new Point(tank2.X, tank2.Y), tank2.image.Size);
            Rect bulletRect = new Rect(bullet.X, bullet.Y, 10, 10);
            Rect bullet2Rect = new Rect(bullet2.X, bullet2.Y, 10, 10);
            HandleCollision(tank,tank1Rect,leftwallrect,rightwallrect,bottomwallrect,topwallrect,bulletRect);
            HandleCollision(tank2,tank2Rect, leftwallrect, rightwallrect, bottomwallrect, topwallrect, bulletRect);

           
            
            if(Intersects(bulletRect, tank2Rect))
            {
                isCollides = true;
                tank2.score--;
                bullet.X = 1920;
                bullet.TravelingLeftward = false;
                bullet.TravelingRightward = false;
                bullet.TravelingUpward = false;
                bullet.TravelingDownward = false;
            }

            if (Intersects(bullet2Rect, tank1Rect))
            {
                isCollides = true;
                tank.score--;
                bullet2.X = 2000;
                bullet2.TravelingLeftward = false;
                bullet2.TravelingRightward = false;
                bullet2.TravelingUpward = false;
                bullet2.TravelingDownward = false;
            }

            /*
            if (Gamepad.Gamepads.Count > 0)
            {
                controller = Gamepad.Gamepads.First();
                var reading = controller.GetCurrentReading();
                if((int)reading.LeftThumbstickX < 0)
                {
                    tank.TravelingLeftward = true;
                }
                else
                {
                    tank.TravelingLeftward = false;
                }
                if ((int)reading.LeftThumbstickX > 0)
                {
                    tank.TravelingRightward = true;
                }
                else
                {
                    tank.TravelingRightward = false;
                }
                if ((int)reading.LeftThumbstickY > 0)
                {
                    tank.TravelingUpward = true;
                }
                else
                {
                    tank.TravelingUpward = false;
                }
                if ((int)reading.LeftThumbstickY < 0)
                {
                    tank.TravelingDownward = true;
                }
                else
                {
                    tank.TravelingDownward = false;
                }
                if (reading.RightTrigger > 0)
                {
                    if (bullet.X > 1200 || bullet.X < 0 || bullet.Y < 0 || bullet.Y > 950)
                    {
                        if (tank.image == tankimage4)
                        {
                            bullet.X = tank.X + 60;
                            bullet.Y = tank.Y + 100;
                            bullet.TravelingDownward = true;
                            bullet.TravelingLeftward = false;
                            bullet.TravelingUpward = false;
                            bullet.TravelingRightward = false;
                        }
                        if (tank.image == tankimage2)
                        {
                            bullet.X = tank.X + 15;
                            bullet.Y = tank.Y + 60;
                            bullet.TravelingLeftward = true;
                            bullet.TravelingDownward = false;
                            bullet.TravelingUpward = false;
                            bullet.TravelingRightward = false;
                        }
                        if (tank.image == tankimage3)
                        {
                            bullet.X = tank.X + 60;
                            bullet.Y = tank.Y + 15;
                            bullet.TravelingUpward = true;
                            bullet.TravelingRightward = false;
                            bullet.TravelingLeftward = false;
                            bullet.TravelingDownward = false;
                        }
                        if (tank.image == tankimage)
                        {
                            bullet.X = tank.X + 100;
                            bullet.Y = tank.Y + 60;
                            bullet.TravelingRightward = true;
                            bullet.TravelingLeftward = false;
                            bullet.TravelingDownward = false;
                            bullet.TravelingUpward = false;
                        }
                    }
                    
                }
            }
            /*
            if (Gamepad.Gamepads.Count > 0)
            {
                controller2 = Gamepad.Gamepads.ElementAt(1);
                var reading = controller2.GetCurrentReading();
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
                /*
                if (reading.RightTrigger > 0)
                {
                    if (bullet2.X > 1200 || bullet2.X < 0 || bullet2.Y < 0 || bullet2.Y > 950)
                    {
                        if (tank2.image == tankimage4)
                        {
                            bullet2.X = tank.X + 60;
                            bullet2.Y = tank.Y + 100;
                            bullet2.TravelingDownward = true;
                            bullet2.TravelingLeftward = false;
                            bullet2.TravelingUpward = false;
                            bullet2.TravelingRightward = false;
                        }
                        if (tank2.image == tankimage2)
                        {
                            bullet2.X = tank.X + 15;
                            bullet2.Y = tank.Y + 60;
                            bullet2.TravelingLeftward = true;
                            bullet2.TravelingDownward = false;
                            bullet2.TravelingUpward = false;
                            bullet2.TravelingRightward = false;
                        }
                        if (tank2.image == tankimage3)
                        {
                            bullet2.X = tank.X + 60;
                            bullet2.Y = tank.Y + 15;
                            bullet2.TravelingUpward = true;
                            bullet2.TravelingRightward = false;
                            bullet2.TravelingLeftward = false;
                            bullet2.TravelingDownward = false;
                        }
                        if (tank2.image == tankimage)
                        {
                            bullet.X = tank.X + 100;
                            bullet.Y = tank.Y + 60;
                            bullet.TravelingRightward = true;
                            bullet.TravelingLeftward = false;
                            bullet.TravelingDownward = false;
                            bullet.TravelingUpward = false;
                        }
                    }
                }
            }*/

        }
        private void Canvas_CreateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResources(sender).AsAsyncAction());
        }

        async Task CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender)
        {
            tankimage = await CanvasBitmap.LoadAsync(sender, "Assets/demoTankRight.png");
            tankimage2 = await CanvasBitmap.LoadAsync(sender, "Assets/demoTankLeft.png");
            tankimage3 = await CanvasBitmap.LoadAsync(sender, "Assets/demoTankUp.png");
            tankimage4 = await CanvasBitmap.LoadAsync(sender, "Assets/demoTankDown.png");
            ballImage = await CanvasBitmap.LoadAsync(sender, "Assets/ball.jpg");

            tank = new Tank(50,300,5,tankimage, tankimage2, tankimage, tankimage3, tankimage4);
            tank2 = new Tank(1000, 300, 5, tankimage2, tankimage2, tankimage, tankimage3, tankimage4);
            bullet = new Ball(2200, tank.Y, 10, ballImage);
            leftwall = new Wall(20, 10, 20, 750, Colors.DarkSeaGreen);
            rightwall = new Wall(1510, 10, 1510, 750, Colors.DarkSeaGreen);
            topwall = new Wall(20, 10, 1510, 10, Colors.DarkSeaGreen);
            bottomwall = new Wall(20, 750, 1510, 750, Colors.DarkSeaGreen);
            bullet2 = new Ball(2200, 200, 10, ballImage);
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
                tank.TravelingRightward = true;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.W)
            {
                tank.TravelingUpward = true;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.S)
            {
                tank.TravelingDownward = true;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.A)
            {
                tank.TravelingLeftward = true;
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
                if (bullet.X > 1200 || bullet.X < 0 || bullet.Y < 0 || bullet.Y > 950)
                {
                    if (tank.image == tankimage4)
                    {
                        bullet.X = tank.X + 60;
                        bullet.Y = tank.Y + 100;

                        bullet.TravelingDownward = true;
                        bullet.TravelingLeftward = false;
                        bullet.TravelingUpward = false;
                        bullet.TravelingRightward = false;
                    }
                    if (tank.image == tankimage2)
                    {
                        bullet.X = tank.X + 15;
                        bullet.Y = tank.Y + 60;

                        bullet.TravelingLeftward = true;
                        bullet.TravelingDownward = false;
                        bullet.TravelingUpward = false;
                        bullet.TravelingRightward = false;


                    }
                    if (tank.image == tankimage3)
                    {
                        bullet.X = tank.X + 60;
                        bullet.Y = tank.Y + 15;

                        bullet.TravelingUpward = true;
                        bullet.TravelingRightward = false;
                        bullet.TravelingLeftward = false;
                        bullet.TravelingDownward = false;


                    }
                    if (tank.image == tankimage)
                    {
                        bullet.X = tank.X + 100;
                        bullet.Y = tank.Y + 60;
                        bullet.TravelingRightward = true;
                        bullet.TravelingLeftward = false;
                        bullet.TravelingDownward = false;
                        bullet.TravelingUpward = false;


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
                tank.TravelingRightward = false;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.W)
            {
                tank.TravelingUpward = false;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.S)
            {
                tank.TravelingDownward = false;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.A)
            {
                tank.TravelingLeftward = false;
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
