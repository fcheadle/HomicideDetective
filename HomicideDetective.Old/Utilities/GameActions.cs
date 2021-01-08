namespace HomicideDetective.Old.Utilities
{ 
    public enum GameAction
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
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
    }
    //public class GameActions
    //{
    //    public readonly GameAction[] Queries = 
    //    { 
    //        GameAction.LookAtEverythingInSquare,
    //        GameAction.LookAtPerson,
    //        GameAction.Talk,

    //    };
    //}
}
