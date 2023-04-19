﻿using Microsoft.Graphics.Canvas;
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

        public int score;


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
            score = 5;
     
        }

        public void Update()
        {

            if (TravelingDownward)
            {
                Y += Speed;
                image = downImage;
            }
            if (TravelingUpward)
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

        public CanvasBitmap image;

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
    public interface ICollidable
    {
        bool CollidesLeftEdge(int x, int y, int speed);
        bool ColllidesRightEdge(int x, int y, int speed);
        bool CollidesTopEdge(int x, int y, int speed);
        bool CoolidesBottomEdge(int x, int y, int speed);
    }
    public class Wall : IDrawable, ICollidable
    {
        public int WIDTH = 5;
        public int X0 { get; set; }
        public int Y0 { get; set; }
        public int X1 { get; set; }

        public int Y1 { get; set; }

        public Color Color { get; set; }

        public Rect rect { get; private set; }

        public Wall(int x0, int y0, int x1, int y1, Color color)
        {
            X0 = x0;
            Y0 = y0;
            X1 = x1;
            Y1 = y1;
            Color = color;

            //Create a rect class (for collision) automatically:
            if (x0 != x1)    //wide
            {
                rect = new Rect(new Point(x0, y0), new Point(x1, y1));
            }
            else            //tall
            {//TODO remove?
                rect = new Rect(X0, Y0, WIDTH, Y1 - Y0);
            }
             
        }


        public void Draw(CanvasDrawingSession canvas)
        {
            canvas.DrawLine(X0, Y0, X1, Y1, Color, WIDTH);
        }

        public bool CollidesLeftEdge(int x, int y, int speed)
        {
            return Math.Abs(x - X0) - speed <= 0 && y >= Y0 && y <= Y1;
        }

        public bool ColllidesRightEdge(int x, int y, int speed)
        {
            return Math.Abs(x - (X0 + WIDTH)) - speed <= 0 && y >= Y0 && y <= Y1;
        }

        public bool CollidesTopEdge(int x, int y, int speed)
        {
            return x >= X0 && x <= X1 && y + speed >= Y1;
        }

        public bool CoolidesBottomEdge(int x, int y, int speed)
        {
            return x >= X0 && x <= X1 && y - speed <= Y0 + WIDTH;
        }
    }

    public class WallCollection
    {
        List<Wall> walls = new List<Wall>();
        ///List<Rect> rects = new List<Rect>();

        public List<Wall> GetWalls()
        {
            return walls;
        }
        /*public List<Rect> GetRects()
        {
            return rects;
        }*/
        public void Add(Wall wall)
        {
            walls.Add(wall);
            /*rects.Add(new Rect(wall.X0, wall.Y0, wall.WIDTH, wall.Y1 - wall.Y0));
           
            Rect leftwallrect = new Rect(leftwall.X0, leftwall.Y0, leftwall.WIDTH, leftwall.Y1 - leftwall.Y0);
            Rect rightwallrect = new Rect(rightwall.X0, rightwall.Y0, rightwall.WIDTH, rightwall.Y1 - rightwall.Y0);
            Rect bottomwallrect = new Rect(new Point(bottomwall.X0, bottomwall.Y0), new Point(bottomwall.X1, bottomwall.Y1));
            Rect topwallrect = new Rect(new Point(topwall.X0, topwall.Y0), new Point(topwall.X1, topwall.Y1));
            */
        }

    }


}
