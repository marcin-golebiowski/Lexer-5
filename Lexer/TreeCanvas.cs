﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lexer
{
    class TreeCanvas : Canvas
    {
        private const int LEVEL_HEIGHT = 50;

        int minX = 1000;
        int maxX = 0;
        int maxY = 0;
        public TreeCanvas(SyntaxTree.Node Root)
        {
            this.Children.Clear();
            Background = Brushes.White;
            //   AddLabel("hello", 50, 50);
            DrawNode(Root, 0, 700, 25);
            minX -= 30;
            maxY += 30;
            maxX += 50;

            Rect rect1 = new Rect(0, 0,0, 0);

            System.Windows.Int32Rect rcFrom = new System.Windows.Int32Rect();
            rcFrom.X = minX;
            rcFrom.Y = 5;
            rcFrom.Width = maxX - minX;
            rcFrom.Height = maxY - 5;


            // VERY important
            Size size = new Size(maxX, maxY);
            this.Measure(size);
            this.Arrange(rect1);

            RenderTargetBitmap renderBitmap =
               new RenderTargetBitmap(
                maxX,
                maxY,
                 96d,
                 96d,
                 PixelFormats.Pbgra32);
            renderBitmap.Render(this);
            BitmapSource bs = new CroppedBitmap(renderBitmap as BitmapSource, rcFrom);

           
            using (FileStream outStream = new FileStream("D:\\file.png", FileMode.Create))
            {
                // Use png encoder for our data
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                // push the rendered bitmap to it
                encoder.Frames.Add(BitmapFrame.Create(bs as BitmapSource));
                // save the data to the stream
                encoder.Save(outStream);
            }
        }

   
        private void DrawNode(SyntaxTree.Node n, int level, int x, int y)
        {

            maxY = (y > maxY) ? y : maxY;
            maxX = (x > maxX) ? x : maxX;
            minX = (x < minX) ? x : minX;

            AddLabel(n.Content, x, y);
            Console.WriteLine(n.Content + " : " + level.ToString());
            if (n.Children.Count == 1)
            {
                AddLine(x+10, y+20, x+10, y + LEVEL_HEIGHT);
                DrawNode(n.Children[0] as SyntaxTree.Node, level + 1, x, y + LEVEL_HEIGHT);
            }
            else if (n.Children.Count == 2)
            {
                int xOffset = level == 4 ? 70 : 70 * (4 - level);
                AddLine(x, y, x - xOffset, y + LEVEL_HEIGHT);
                AddLine(x, y, x + xOffset, y + LEVEL_HEIGHT);
                DrawNode(n.Children[0] as SyntaxTree.Node, level + 1, x - xOffset, y + LEVEL_HEIGHT);
                DrawNode(n.Children[1] as SyntaxTree.Node, level + 1, x + xOffset, y + LEVEL_HEIGHT);

            }
            else if (n.Children.Count == 3)
            {



                int xOffset = level == 4 ? 100 : 100 * (4 - level);
                AddLine(x + 10, y, x - xOffset, y + LEVEL_HEIGHT);
                AddLine(x + 10, y, x + 10, y + LEVEL_HEIGHT);
                AddLine(x + 10, y, x + xOffset, y + LEVEL_HEIGHT);


                DrawNode(n.Children[0] as SyntaxTree.Node, level + 1, x - xOffset, y + LEVEL_HEIGHT);
                DrawNode(n.Children[1] as SyntaxTree.Node, level + 1, x, y + LEVEL_HEIGHT);
                DrawNode(n.Children[2] as SyntaxTree.Node, level + 1, x + xOffset, y + LEVEL_HEIGHT);
            }
        }

        private void AddLabel(String content, int x, int y)
        {
            Ellipse myEllipse = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 2;
            myEllipse.Stroke = Brushes.Black;

            myEllipse.Width = 60;
            myEllipse.Height = 30;
            myEllipse.Margin = new Thickness(x-20, y-10, 0, 0);
            Children.Add(myEllipse);


            TextBlock label = new TextBlock();
            label.Text = content;
            label.Width = 60;
            label.TextAlignment = TextAlignment.Center;
            label.Margin = new Thickness(x-20, y-10, 0, 0);
            Children.Add(label);
        }

        private void AddLine(int stX, int stY, int endX, int endY)
        {
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.Black;
            myLine.X1 = stX;
            myLine.X2 = endX;
            myLine.Y1 = stY;
            myLine.Y2 = endY;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.StrokeThickness = 2;
            //Children.Add(myLine);
            Children.Insert(1, myLine);
        }

       

    }
}
