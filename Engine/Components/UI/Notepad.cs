using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using SadConsole.Themes;
using System;
using System.Collections.Generic;
using Rectangle = GoRogue.Rectangle;

namespace Engine.Components.UI
{
    public class Notepad : Window
    {
        private const int _width = 24;
        private const int _height = 24;
        private const int _maxLines = 10 * _height;
        public ScrollingConsole Console;
        private readonly string _title;
        private readonly Queue<string> _lines = new Queue<string>();
        //last page button?
        public Button BackPageButton;
        //next page button?
        public Button NextPageButton;
        public Notepad(string title) : base(_width, _height)
        {
            Title = title;
            TitleAlignment = HorizontalAlignment.Stretch;
            GenerateTheme();
            Console = new ScrollingConsole(_width, _maxLines);
            DefaultBackground = Color.Tan;
            Console.Position = new Coord(0, 1);
            Console.ViewPort = new Rectangle(0, 0, _width, _height - 1);
            Console.Fill(Color.Blue, Color.Tan, '_');
            Console.IsExclusiveMouse = true;

            //Print(0, 0, Title.Align(HorizontalAlignment.Stretch, Width), Color.Black, Color.Tan);
            Theme.TitleAreaY = 0;

            //back/next buttons
            BackPageButton = new Button(1) { Position = new Coord(0, _height - 1) , Text = "<" };
            BackPageButton.MouseButtonClicked += BackButton_Clicked;
            Add(BackPageButton);

            NextPageButton = new Button(1) { Position = new Coord(_width - 1, _height - 1), Text = ">" };
            NextPageButton.MouseButtonClicked += NextButton_Clicked;
            Add(NextPageButton);

            ViewPort = new Rectangle(0, 0, _width, _height);

            //mouse
            UseMouse = true;
            CanDrag = true;
            //Show();
        }

        private void GenerateTheme()
        {
            Theme = SadConsole.Themes.Library.Default.WindowTheme.Clone();
            Theme.BorderLineStyle = CellSurface.ConnectedLineThick;
            Theme.FillStyle = new Cell(Color.Blue, Color.Tan, '_');
            
        }

        private void NextButton_Clicked(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BackButton_Clicked(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Add(string message)
        {
            _lines.Enqueue(message);
            // when exceeding the max number of lines remove the oldest one
            if (_lines.Count > _maxLines)
            {
                _lines.Dequeue();
            }

            // Move the cursor to the last line and print the message.
            Console.Cursor.Position = new Point(0, _lines.Count * 3);
            ColoredString output = new ColoredString(message, Color.Black, Color.Tan);
            Add(output);
        }

        public void Add(ColoredString output)
        {
            Console.Cursor.Print(output + "\n");
        }
    }
}
