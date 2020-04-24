using GoRogue;

namespace Engine.Maps
{
    internal class Room : Area
    {
        internal readonly RoomTypes Type;

        internal Room(string name, Rectangle area, RoomTypes type) : base(
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
    }
}