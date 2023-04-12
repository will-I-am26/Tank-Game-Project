﻿using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Gaming.Input;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media.Imaging;

namespace FinalProject
{
    public class Tank : IDrawable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Speed { get; set; }

        private CanvasBitmap image;

        private CanvasBitmap leftImage;

        private CanvasBitmap rightImage;

        private CanvasBitmap upImage;

        private CanvasBitmap downImage;

        public bool TravelingDownward { get; set; }
        public bool TravelingLeftward { get; set; }

        public bool TravelingUpward { get; set; }
        public bool TravelingRightward { get; set; }

        public Tank(int x, int y, int speed, CanvasBitmap image, CanvasBitmap leftImage, CanvasBitmap rightImage, CanvasBitmap upImage, CanvasBitmap downImage)
        {
            X = x;
            Y = y;
            this.image = image;
            this.leftImage = leftImage;
            this.rightImage = rightImage;
            this.upImage = upImage;
            this.downImage = downImage;
            Speed = speed;
            TravelingDownward = false;
            TravelingLeftward = false;
            TravelingRightward = false;
            TravelingUpward= false;
        }

        public void Update()
        {

            if (TravelingDownward)
            {
                Y += Speed;
                image = downImage;
            }
            if(TravelingUpward)
            {
                Y -= Speed;
                image = upImage;
            }
            if (TravelingLeftward)
            {
                X -= Speed;
                image = leftImage;
            }
            if (TravelingRightward)
            {
                X += Speed;
                image = rightImage;
            }

        }

        public void Draw(CanvasDrawingSession canvas)
        {
            // when using image, account for x and y being top left
            canvas.DrawImage(image, X, Y);

        }

    }

    public interface IDrawable
    {
        void Draw(CanvasDrawingSession canvas);
    }

    /*
    public class tankGame
    {
        private Tank tank;
        private List<Tank> drawables;
        private Gamepad controller;

        public tankGame(CanvasBitmap tankimage)
        {
            

            drawables = new List<Tank>();

            tank = new Tank(100, 100, 5, tankimage);

        }

        public void goingUp(bool TravelingUpward)
        {
            tank.TravelingUpward = TravelingUpward;
        }

        public void goingDown(bool TravelingDownward)
        {
            tank.TravelingDownward = TravelingDownward;
        }

        public void goingLeft(bool TravelingLeftward)
        {
            tank.TravelingLeftward = TravelingLeftward;
        }

        public void goingRight(bool TravelingRightward)
        {
            tank.TravelingRightward = TravelingRightward;
        }

        public void Update()
        {
            if (Gamepad.Gamepads.Count > 0)
            {
                controller = Gamepad.Gamepads.First();
                var reading = controller.GetCurrentReading();
                tank.X += (int)(reading.LeftThumbstickX * 5);
                tank.Y += (int)(reading.LeftThumbstickY * -5);
            }

            tank.Update();
        }

        public void DrawGame(CanvasDrawingSession canvas)
        {
            foreach (var drawable in drawables)
            {
                drawable.Draw(canvas);
            }
        }


    }
    */


}