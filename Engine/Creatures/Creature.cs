using System.Collections.Generic;
using GoRogue;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Maps;
using SadConsole.Tiles;

namespace Engine.Creatures
{
    internal class GameFrameTileVisibilityRefresher : SadConsole.Components.GoRogue.GameFrameProcessor
    {
        public new Creature Parent => (Creature)base.Parent;
        public override void ProcessGameFrame() => Parent.RefreshVisibilityTiles();
    }

    internal abstract class Creature : BasicEntity
    {
        protected int baseVisibilityDistance = 20;
        protected int baseLightSourceDistance = 20;
        public int VisibilityDistance => baseVisibilityDistance + GetInventoryVisibilityMods();
        public int LightSourceDistance => baseLightSourceDistance + GetInventoryLightingMods();
        public string Title { get; set; }
        public string Description { get; set; }

        public new TileMap CurrentMap => (TileMap)base.CurrentMap;
        public Items.Inventory Inventory = new Items.Inventory();

        public List<Tile> VisibleTiles = new List<Tile>();
        public List<Tile> LightSourceTiles = new List<Tile>();
        protected Region currentRegion;
        protected List<Tile> newVisibleTiles = new List<Tile>();
        protected List<Tile> newLightSourceTiles = new List<Tile>();

        //protected Region currentRegion;

        protected GoRogue.FOV FOVSight;
        protected GoRogue.FOV FOVLighted;

        protected Creature(TileMap map, Coord position, Color foreground, int glyph)
            : base(foreground, Color.Black, glyph, position, 1, isWalkable: false, isTransparent: true)
        {
            Title = "Creature";
            Description = "A creature of some sort. Everything from ants to people.";

            FOVSight = new GoRogue.FOV(map.TransparencyView);
            FOVLighted = new GoRogue.FOV(map.TransparencyView);

            map.AddEntity(this);

            //AddComponent(new GameFrameTileVisibilityRefresher());
        }

        public void RefreshVisibilityTiles()
        {

            // Check to see if have left a room
            if (currentRegion != null && !currentRegion.InnerPoints.Contains(Position))
            {
                // If player, handle room lighting
                if (this == CurrentMap.ControlledGameObject)
                {
                    foreach (Coord point in currentRegion.InnerPoints)
                    {
                        CurrentMap.GetTerrain<Tile>(point).UnsetFlag(TileFlags.Lighted, TileFlags.InLOS);
                    }
                    foreach (Coord point in currentRegion.OuterPoints)
                    {
                        CurrentMap.GetTerrain<Tile>(point).UnsetFlag(TileFlags.Lighted, TileFlags.InLOS);
                    }

                    foreach (Coord tile in FOVSight.CurrentFOV)
                    {
                        CurrentMap.GetTerrain<Tile>(tile).SetFlag(TileFlags.InLOS);
                    }

                    foreach (Coord tile in FOVLighted.CurrentFOV)
                    {
                        CurrentMap.GetTerrain<Tile>(tile).SetFlag(TileFlags.Lighted);
                    }
                }

                // We're not in this region anymore
                currentRegion = null;
            }

            //Not in a region, so find one.
            if (currentRegion == null)
            {
                // See if we're in a different region
                foreach (Region region in CurrentMap.Regions)
                {
                    if (region.InnerPoints.Contains(Position))
                    {
                        currentRegion = region;
                        break;
                    }
                }
            }

            // Visibility
            FOVSight.Calculate(Position, VisibilityDistance);

            // If player, handle LOS flags for tiles.
            if (this == CurrentMap.ControlledGameObject)
            {
                foreach (Coord tile in FOVSight.NewlyUnseen)
                {
                    Tile t = CurrentMap.GetTerrain<Tile>(tile);
                    t.UnsetFlag(TileFlags.InLOS);
                }

                foreach (Coord tile in FOVSight.NewlySeen)
                {
                    CurrentMap.GetTerrain<Tile>(tile).SetFlag(TileFlags.InLOS);
                }
            }

            // Lighting
            FOVLighted.Calculate(Position, LightSourceDistance);

            if (this == CurrentMap.ControlledGameObject)
            {
                foreach (Coord tile in FOVLighted.NewlyUnseen)
                {
                    CurrentMap.GetTerrain<Tile>(tile).UnsetFlag(TileFlags.Lighted);
                }

                foreach (Coord tile in FOVLighted.NewlySeen)
                {
                    CurrentMap.GetTerrain<Tile>(tile).SetFlag(TileFlags.Lighted, TileFlags.Seen);
                }
            }


            // Check and see if we're in a region, ensure these tiles are always visible and lighted.
            if (currentRegion != null)
            {
                Tile tile;

                // Make sure these are lit
                foreach (Coord point in currentRegion.InnerPoints)
                {
                    tile = CurrentMap.GetTerrain<Tile>(point);

                    // If player, handle room lighting
                    if (this == CurrentMap.ControlledGameObject)
                    {
                        tile.SetFlag(TileFlags.Lighted, TileFlags.InLOS, TileFlags.Seen);
                    }

                    // Add tile to visible list, for calculating if the player can see.
                    VisibleTiles.Add(tile);
                }

                foreach (Coord point in currentRegion.OuterPoints)
                {
                    tile = CurrentMap.GetTerrain<Tile>(point);

                    // If player, handle room lighting
                    if (this == CurrentMap.ControlledGameObject)
                    {
                        tile.SetFlag(TileFlags.Lighted, TileFlags.InLOS, TileFlags.Seen);
                    }

                    // Add tile to visible list, for calculating if the player can see.
                    VisibleTiles.Add(tile);
                }
            }
        }

        internal void DoSomething()
        {
            //throw new NotImplementedException();
        }

        protected void AddVisibleTile(int x, int y)
        {
            if (CurrentMap.Bounds().Contains(x, y))
            {
                Tile tile = CurrentMap.GetTerrain<Tile>(x, y);
                tile.SetFlag(TileFlags.InLOS);
                newVisibleTiles.Add(tile);
            }
        }

        protected void AddLightVisbilityTile(int x, int y)
        {
            if (CurrentMap.Bounds().Contains(x, y))
            {
                Tile tile = CurrentMap.GetTerrain<Tile>(x, y);
                tile.SetFlag(TileFlags.Lighted);
                newLightSourceTiles.Add(tile);
            }
        }

        protected int GetInventoryLightingMods()
        {
            int result = 0;

            //foreach (Items.Item item in Inventory.GetEquippedItems())
            //{
            //    result += item.LightingModifier;
            //}

            return result;
        }
        private int GetInventoryVisibilityMods()
        {
            int result = 0;

            //foreach (Items.Item item in Inventory.GetEquippedItems())
            //{
            //    result += item.LightingModifier;
            //}

            return result;
        }
    }
}
