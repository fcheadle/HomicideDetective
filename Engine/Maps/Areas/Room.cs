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
    internal class Room : Area
    {
        internal readonly RoomType Type;
        public Rectangle OuterRect { get; }
        public Rectangle InnerRect { get; private set; }

        internal Room(string name, Rectangle area, RoomType type) : base(
            name,
            new Coord(area.MaxExtentX, area.MaxExtentY),
            new Coord(area.MaxExtentX, area.MinExtentY),
            new Coord(area.MinExtentX, area.MinExtentY),
            new Coord(area.MinExtentX, area.MaxExtentY)
            )
        {
            OuterRect = area;
            InnerRect = area;
            Type = type;
        }

        internal new Room Shift()
        {
            Shift(Origin);
            return this;
        }
    }
}