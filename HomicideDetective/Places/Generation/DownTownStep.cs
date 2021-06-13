using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using SadRogue.Integration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Places.Generation
{
    public class DownTownStep : GenerationStep
    {
        private int _wallGlyph = 176;
        private int _floorGlyph = 46;
        private int _doorGlyph = 254;
        private Color _wallColor = Color.Red;
        private Color _floorPrimaryColor = Color.Red;
        private Color _floorSecondaryColor = Color.DarkRed;
        private readonly Color _backgroundColor = Color.Black;

        private int _shortLength = 8;
        private int _longLength = 10;
        private int _themeIndex = 0;
        
        private readonly List<Point> _connections = new List<Point>();
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            var map = context.GetFirstOrNew<ISettableGridView<RogueLikeCell>>
                (() => new ArrayView<RogueLikeCell>(context.Width, context.Height), "downtown");
            
            var shops = context.GetFirstOrNew(() => new List<Region>(), "regions");
            var colCount = map.Width / 2 / _shortLength;
            var rowCount = (map.Height - 5) / (_longLength + 1);

            int skipOne = -1;
            int skipTwo = -1;
            if (colCount % 2 == 0)
            {
                skipOne = colCount / 2 - 1;
            }
            else
            {
                skipOne = colCount / 2;
                skipTwo = colCount / 2 - 1;
            }
            
            for (int j = 0; j < colCount; j++)
            {
                for (int i = 0; i < rowCount; i++)
                {
                    if(j != skipOne && j != skipTwo)
                    {
                        int y = map.Height - 5 - j * _longLength;
                        int x = map.Height - y + i * _shortLength;
                        var shopArea = ParallelogramShop(x, y, _shortLength, _longLength);
                        DrawRegion(shopArea, map);
                        shops.Add(shopArea);
                        
                        foreach (var connection in _connections)
                            if(map.Contains(connection))
                                map[connection] = Door(connection);
                        
                        _connections.Clear();
                        
                        yield return shopArea;
                    }
                }
                SwitchColorTheme();
            }
        }

        private Region ParallelogramShop(int x, int y, int width, int height)
        {
            var plot = Parallelogram(x, y, width, height);
            ConnectOnAllSides(plot);
            return plot;
        }

        private void ConnectOnAllSides(Region plot)
        {
            ConnectOnLeftSide(plot);
            ConnectOnRightSide(plot);
            ConnectOnTopSide(plot);
            ConnectOnBottomSide(plot);
        }

        private void ConnectOnLeftSide(Region plot)
            => plot.AddConnection(MiddlePoint(plot.WestBoundary));
        

        private void ConnectOnRightSide(Region plot)
            => plot.AddConnection(MiddlePoint(plot.EastBoundary));

        private void ConnectOnTopSide(Region plot)
            => plot.AddConnection(MiddlePoint(plot.NorthBoundary));

        private void ConnectOnBottomSide(Region plot)
            => plot.AddConnection(MiddlePoint(plot.SouthBoundary));

        private Point MiddlePoint(IReadOnlyArea area) 
            => area[area.Count() / 2];
        
        private void DrawRegion(Region region, ISettableGridView<RogueLikeCell> map)
        {
            bool isBathroom = region.Name == "bathroom";
            foreach (var point in region.InnerPoints.Where(map.Contains))
                map[point] = isBathroom ? BathroomFloor(point) : Floor(point);
            
            foreach (var point in region.OuterPoints.Where(map.Contains))
                map[point] = Wall(point);

            foreach (var point in region.Connections)
                _connections.Add(point);
        }
        
        private Region Parallelogram(int left, int bottom, int width, int height)
        {
            Point nw = (left + height, bottom - height);
            Point ne = (left + height + width, bottom - height);
            Point se = (left + width, bottom);
            Point sw = (left, bottom);
            return new Region("parallelogram", nw, ne, se, sw);
        }

        private void SwitchColorTheme()
        {
            _themeIndex++;
            switch (_themeIndex % 4)
            {
                default:
                case 0: SetWhiteTheme(); break;
                case 1: SetBrownTheme(); break;
                case 2: SetRedTheme(); break;
                case 3: SetYellowTheme(); break;
            }
        }
        private void SetRedTheme()
        {
            _wallColor = Color.Red;
            _floorPrimaryColor = Color.Red;
            _floorSecondaryColor = Color.Brown;
        }
        private void SetBrownTheme()
        {
            _wallColor = Color.SaddleBrown;
            _floorPrimaryColor = Color.Brown;
            _floorSecondaryColor = Color.DarkRed;
        }
        private void SetWhiteTheme()
        {
            _wallColor = Color.Gray;
            _floorPrimaryColor = Color.DarkGoldenrod;
            _floorSecondaryColor = Color.DarkGray;
        }
        
        private void SetYellowTheme()
        {
            _wallColor = Color.DarkGoldenrod;
            _floorPrimaryColor = Color.Beige;
            _floorSecondaryColor = Color.DarkKhaki;
        }
        private RogueLikeCell Wall(Point point) => new RogueLikeCell(point, _wallColor, _backgroundColor, _wallGlyph, 0, false, false);
        private RogueLikeCell Floor(Point point) => new RogueLikeCell(point, (point.X + point.Y) % 2 == 1 ? _floorPrimaryColor : _floorSecondaryColor, _backgroundColor, _floorGlyph, 0);
        private RogueLikeCell Door(Point point) => new RogueLikeCell(point, _wallColor, _backgroundColor, _doorGlyph, 0);
        private RogueLikeCell BathroomFloor(Point point) => new RogueLikeCell(point, (point.X + point.Y) % 2 == 1 ? _floorPrimaryColor : _floorSecondaryColor, _backgroundColor, 4, 0);
    }
}