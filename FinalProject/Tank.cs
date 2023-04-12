using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Gaming.Input;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Documents;

namespace FinalProject
{
    public class Tank : IDrawable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Speed { get; set; }

        private CanvasBitmap image;

        public bool TravelingDownward { get; set; }
        public bool TravelingLeftward { get; set; }

        public bool TravelingUpward { get; set; }
        public bool TravelingRightward { get; set; }

        public Tank(int x, int y, int speed, CanvasBitmap image)
        {
            X = x;
            Y = y;
            this.image = image;
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
            }
            if(TravelingUpward)
            {
                Y -= Speed;
            }
            if (TravelingLeftward)
            {
                X -= Speed;
            }
            if (TravelingRightward)
            {
                X += Speed;
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



}
