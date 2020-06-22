using Engine.Components.UI;
using GoRogue;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using System;
using System.Collections.Generic;

namespace Engine.UI
{
    //shamelessly copied and cobbled together from SC's textbox
    //should probably switch from NewLines to Paragraphs, and have a collection of paragraphs?
    public class TextArea : ControlBase
    {
        private string _editingText = " ";
        //private string _text = "";
        private int _caretPosition
        {
            get => _index;
            set
            {
                if (value >= _editingText.Length)
                    _index = _editingText.Length - 1;
                else if (value < 0)
                    _index = 0;
                else
                    _index = value;                    
            }
        }
        private int _index;

        public Coord CursorPosition => new Coord(_caretPosition % Width, _caretPosition / Width);
        public string Text => _editingText;

        private Dictionary<Keys, (string, string)> _specialCharacters = new Dictionary<Keys, (string, string)>()
        {
            { Keys.Space, (" ", " ") },
            { Keys.OemQuotes, ("'", "\"") },
            { Keys.OemQuestion, ("/","?") },
            { Keys.OemComma, (",","<") },
            { Keys.OemPeriod, (".",">") },
            { Keys.OemSemicolon, (";",":") },
            { Keys.OemBackslash,("\\", "|")},
            { Keys.OemOpenBrackets, ("[","{") },
            { Keys.OemCloseBrackets, ("]","}") },
            { Keys.NumPad0, ("0",")")},
            { Keys.NumPad9, ("9","(")},
            { Keys.NumPad8, ("8","*")},
            { Keys.NumPad7, ("7","&")},
            { Keys.NumPad6, ("6","^")},
            { Keys.NumPad5, ("5","%")},
            { Keys.NumPad4, ("4","$")},
            { Keys.NumPad3, ("3","#")},
            { Keys.NumPad2, ("2","@")},
            { Keys.NumPad1, ("1","!")},
        };

        public TextArea(int width, int height) : base(width, height)
        {
            Theme = new PaperButtonTheme();
            ThemeColors = UI.ThemeColors.Clear;
        }
        
        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (info.KeysPressed.Count > 0)
            {
                if (info.IsKeyPressed(Keys.Enter))
                    return NewLine();

                if (info.IsKeyPressed(Keys.Left))
                {
                    if(_caretPosition > 0)
                        _caretPosition--;
                    return true;
                }
                if (info.IsKeyPressed(Keys.Right))
                {
                    if(_caretPosition < _editingText.Length)
                        _caretPosition++;
                    return true;
                }
                if (info.IsKeyPressed(Keys.Up))
                {
                    _caretPosition -= Width;
                    if (_caretPosition < 0)
                        _caretPosition = 0;
                    return true;
                }
                if (info.IsKeyPressed(Keys.Down))
                {
                    _caretPosition += Width;
                    if (_caretPosition > _editingText.Length)
                        _caretPosition = _editingText.Length - 1;
                    return true;
                }

                if (info.IsKeyPressed(Keys.Escape) || info.IsKeyPressed(Keys.Tab))
                {
                    Game.UIManager.ControlledGameObject.IsFocused = true;
                    return true;
                }

                if (info.IsKeyPressed(Keys.Back))
                {
                    if (_caretPosition == 0)
                        return true;

                    _editingText = _editingText.Remove(_caretPosition - 1, 1);
                    _caretPosition--;
                    return true;
                }
                if (info.IsKeyPressed(Keys.Delete))
                {
                    _editingText = _editingText.Remove(_caretPosition, 1);
                    return true;
                }

                foreach (AsciiKey akey in info.KeysPressed)
                {
                    Keys key = akey.Key;
                    if (IsValidKey(key))
                    {
                        string k = key.ToString();

                        if (info.IsKeyDown(Keys.LeftShift) || info.IsKeyDown(Keys.RightControl))
                        {
                            if(k.Length == 1)
                                _editingText = _editingText.Insert(_caretPosition, key.ToString());
                            else
                                _editingText = _editingText.Insert(_caretPosition, _specialCharacters[key].Item2);
                        }
                        else
                        {
                            if (k.Length == 1)
                                _editingText = _editingText.Insert(_caretPosition, key.ToString().ToLower());
                            else
                                _editingText = _editingText.Insert(_caretPosition, _specialCharacters[key].Item1);
                        }
                        _caretPosition++;
                    }
                }
            }

            return true;
        }
        
        public bool NewLine()
        {
            int lineNumber = _caretPosition / Width;
            int newPos = (lineNumber + 1) * Width;
            string spaces = "";
            for(int i = 0; i < newPos - _caretPosition; i++)
                spaces += " ";

            _editingText = _editingText.Insert(_caretPosition, spaces);
            _caretPosition = newPos;

            return true;
        }

        public bool IsValidKey(Keys key)
        {
            if (key.ToString().Length == 1)
                return true;

            if (!_specialCharacters.ContainsKey(key))
                return false;
            
            return true;
        }

        public void WriteLine(string text)
        {
            for (int i = 0; i < text.Length; i++)
                Write(text.Substring(i,1));
            NewLine();
        }

        public void Write(string c)
        {
            _editingText = _editingText.Insert(_caretPosition, c);
            _caretPosition++;
        }
    }
}