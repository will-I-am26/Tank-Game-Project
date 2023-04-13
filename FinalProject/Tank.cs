using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
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
using static System.Net.Mime.MediaTypeNames;

namespace FinalProject
{
    public class Tank : IDrawable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Speed { get; set; }

        public CanvasBitmap image;

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

     public class Ball : IDrawable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Radius { get; set; }

        public int Speed { get; set; }

        public Color Color { get; set; }
        public bool TravelingDownward { get; set; }
        public bool TravelingLeftward { get; set; }

        public bool TravelingUpward { get; set; }
        public bool TravelingRightward { get; set; }

        private CanvasBitmap image;

        public Ball(int x, int y, int radius, CanvasBitmap image)
        {
            X = x;
            Y = y;
            Radius = radius;
            this.image = image;
            Speed = 10;
            ChangeColorRandomly();
            TravelingDownward = false;
            TravelingLeftward = false;
            TravelingUpward = false;
            TravelingRightward= false;
        }

        public void Update()
        {
            if (TravelingDownward)
            {
                Y += Speed;
            }
            if (TravelingUpward)
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

        public void ChangeColorRandomly()
        {
            Random random = new Random();
            Color = Color.FromArgb(255, (byte)random.Next(0, 256), (byte)random.Next(0, 256), (byte)random.Next(0, 256));
        }

        public void Draw(CanvasDrawingSession canvas)
        {
            // when using image, account for x and y being top left
            //canvas.DrawImage(image, X, Y);

            // when using ellipse, account for x and y being the center

            canvas.FillEllipse(X, Y, Radius, Radius, Color);
        }
    }


}
