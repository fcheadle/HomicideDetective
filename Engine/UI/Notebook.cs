using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using SadConsole.Themes;
using System;
using System.Collections.Generic;
using Rectangle = GoRogue.Rectangle;

namespace Engine.UI
{
    public class Notebook : List<Page>
    {
        //last page button?
        public Button BackPageButton;
        //next page button?
        public Button NextPageButton;
        public int PageNumber = 0;
        public Page CurrentPage
        {
            get
            {
                while (PageNumber >= Count)
                    NewPage();
                return this[PageNumber];
            }
        }

        private void NewPage()
        {
            Add(new Page("////////////////////////////////", ""));
        }

        public Notebook()
        {
            //back/next buttons
            BackPageButton = new Button(1) { Position = new Coord(0, CurrentPage.Height - 1), Text = "<" };
            BackPageButton.MouseButtonClicked += BackButton_Clicked;
            CurrentPage.Add(BackPageButton);

            NextPageButton = new Button(1) { Position = new Coord(CurrentPage.Width - 1, CurrentPage.Height - 1), Text = ">" };
            NextPageButton.MouseButtonClicked += NextButton_Clicked;
            CurrentPage.Add(NextPageButton);

            //Show();
        }

        private void NextButton_Clicked(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BackButton_Clicked(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BackOnePage() => PageNumber = PageNumber == 0 ? PageNumber : PageNumber--;
        private void ForwardOnePage() => PageNumber++;
    }
}
