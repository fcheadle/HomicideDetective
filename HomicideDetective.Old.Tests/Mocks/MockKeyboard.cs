using Microsoft.Xna.Framework.Input;
using SadConsole.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HomicideDetective.Old.Tests.Mocks
{
    public class MockKeyboard : SadConsole.Input.Keyboard
    {

        public new ReadOnlyCollection<AsciiKey> KeysPressed => KeysPressedInternal.AsReadOnly();
        public new ReadOnlyCollection<AsciiKey> KeysDown => KeysDownInternal.AsReadOnly();
        public new ReadOnlyCollection<AsciiKey> KeysReleased => KeysReleasedInternal.AsReadOnly();
        private List<AsciiKey> KeysPressedInternal { get; }
        private List<AsciiKey> KeysDownInternal { get; }
        //private List<Keys> UnmappedVirtualKeysDown { get; }
        private List<AsciiKey> KeysReleasedInternal { get; }

        public MockKeyboard()
        {
            KeysPressedInternal = new List<AsciiKey>();
            KeysReleasedInternal = new List<AsciiKey>();
            KeysDownInternal = new List<AsciiKey>();
            //UnmappedVirtualKeysDown = new List<Keys>();
        }
        // internal void AddKeyDown(AsciiKey key, Keys unmappedVirtualKey)
        // {
        //     KeysDownInternal.Add(key);
        //     //UnmappedVirtualKeysDown.Add(unmappedVirtualKey);
        // }

        internal void RemoveKeyDownAt(int i)
        {
            KeysDownInternal.RemoveAt(i);
            //UnmappedVirtualKeysDown.RemoveAt(i);
        }
        internal void AddKeyPressed(AsciiKey key, Keys unmappedVirtualKey)
        {
            KeysPressedInternal.Add(key);
        }

        internal void RemoveKeyPressed(int i)
        {
            KeysPressedInternal.RemoveAt(i);
        }
        //public new bool IsKeyUp(Keys key) => !KeysDownInternal.Contains(AsciiKey.Get(key, _state)) && !KeysDownInternal.Contains(AsciiKey.Get(key, true, _state));
        //public new bool IsKeyUp(AsciiKey key) => !KeysDownInternal.Contains(key);
        //public new bool IsKeyDown(Keys key) => KeysDownInternal.Contains(AsciiKey.Get(key, _state)) || KeysDownInternal.Contains(AsciiKey.Get(key, true, _state));
        //public new bool IsKeyDown(AsciiKey key) => KeysDownInternal.Contains(key);
        //public new bool IsKeyReleased(Keys key) => KeysReleasedInternal.Contains(AsciiKey.Get(key, _state)) || KeysReleasedInternal.Contains(AsciiKey.Get(key, true, _state));
        //public new bool IsKeyReleased(AsciiKey key) => KeysReleasedInternal.Contains(key);
        //public new bool IsKeyPressed(Keys key) => KeysPressedInternal.Contains(AsciiKey.Get(key, _state)) || KeysPressedInternal.Contains(AsciiKey.Get(key, true, _state));
        //public new bool IsKeyPressed(AsciiKey key) => KeysPressedInternal.Contains(key);
    }
}
