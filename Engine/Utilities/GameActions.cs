using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Utilities
{ 
    public enum GameActions
    {
        LookAtEverythingInSquare,
        LookAtPerson,
        Talk,
        TakePhotograph,
        GetItem,
        RemoveItemFromInventory,
        TogglePause,
        DustItemForPrints, //prints and tracks really just work the same way anyways
        ToggleNotes,
        ToggleInventory,
        ToggleMenu,
        RefocusOnPlayer,
    }
}
