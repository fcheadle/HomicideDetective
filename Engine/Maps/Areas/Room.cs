using GoRogue;

namespace Engine.Maps
{
    public enum RoomType
    {
        Parlor,
        BoysBedroom,
        MasterBedroom,
        GuestBathroom,
        Kitchen,
        DiningRoom,
        HallCloset,
        MasterBedCloset,
        ParlorCloset,
        BoysCloset,
        GirlsCloset,
        GuestCloset,
        Hall,
        GirlsBedroom,
        GuestBedroom,
        MasterBathroom
    }
    public class Room : Area
    {
        public readonly RoomType Type;

        public Room(string name, Rectangle area, RoomType type) : base(
            name,
            new Coord(area.MaxExtentX + 1, area.MaxExtentY + 1),
            new Coord(area.MaxExtentX + 1, area.MinExtentY),
            new Coord(area.MinExtentX, area.MinExtentY),
            new Coord(area.MinExtentX, area.MaxExtentY + 1)
            )
        {
            Type = type;
        }

        internal new Room Shift()
        {
            Shift(Origin);
            return this;
        }
    }
}