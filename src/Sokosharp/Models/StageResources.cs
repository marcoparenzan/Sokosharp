using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;

namespace SokoSharp.Models
{
    public class StageResources
    {
        public string Title { get; private set; }
        public (int block, bool landing)[] DefaultLayer { get; private set; }
        public Vec Size { get; private set; }
        public Vec TileSize { get; private set; }
        public Vec InitialPos { get; private set; }
        public Bitmap Set { get; private set; }
        public int SetTilesPerRow { get; private set; }
        public Rectangle[] TileRectCache { get; private set; }
        public Bitmap SpriteSheet { get; private set; }
        public Dictionary<string, Rectangle> Frames { get; } = new Dictionary<string, Rectangle>();

        public const char Wall = '#';
        public const char Free = ' ';
        public const char Landing = '.';
        public const char Package = '$';
        public const char Player = '@';

        public static StageResources Get(int? stageNumber = null)
        {
            var stage = new StageResources();

            // https://www.sourcecode.se/sokoban/levels
            var stream = typeof(Stage).Assembly.GetManifestResourceStream("Sokosharp.Models.Bugs1005.slc");
            var serializers = XmlSerializer.FromTypes(new[] { typeof(SokobanLevels) });
            var levels = (SokobanLevels)serializers[0].Deserialize(stream);
            
            var random = new Random();
            stageNumber = stageNumber ?? random.Next(0, levels.LevelCollection.Level.Count);
            stage.Title = $"Sokoban - Stage {stageNumber}";
            var selected = levels.LevelCollection.Level[stageNumber.Value];

            stage.DefaultLayer = new (int block, bool landing)[selected.Width * selected.Height];
            stage.Size = (selected.Width, selected.Height);

            var offset = 0;
            for (var y = 0; y < selected.Height; y++)
            {
                var levelRow = selected.L[y];
                for (var x = 0; x < selected.Width; x++)
                {
                    var ch = levelRow[x];
                    if (ch == Player) stage.InitialPos = (x, y);
                    stage.DefaultLayer[offset] = ch switch
                    {
                        Package => (Stage.Package, false),
                        Wall => (Stage.Wall, false),
                        _ => (Stage.Free, ch == Landing)
                    };
                    offset++;
                }
            }

            var setStream = typeof(StageResources).Assembly.GetManifestResourceStream("Sokosharp.Models.TileMap.png");
            stage.Set = (Bitmap)Image.FromStream(setStream);
            stage.SetTilesPerRow = 5;
            stage.TileSize = (24, 16);

            var tileRectCache = new List<Rectangle>();
            for(var tileId = 0; tileId<5; tileId++)
            {
                var (tx, ty) = (tileId % stage.SetTilesPerRow, tileId / stage.SetTilesPerRow);
                tileRectCache.Add(new Rectangle(tx * stage.TileSize.x, ty * stage.TileSize.y, stage.TileSize.x, stage.TileSize.y));
            }
            stage.TileRectCache = tileRectCache.ToArray();

            return stage;
        }
    }
}
