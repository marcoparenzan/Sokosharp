using System;
using System.Drawing;

namespace SokoSharp.Models
{
    public class ScrollTopView
    {
        private Stage stage;
        private StageResources resources;

        private Vec pos;
        private Rectangle rect;

        public ScrollTopView(Stage stage, StageResources resources)
        {
            this.stage = stage;
            this.resources = resources;
        }

        public bool CanUpdate
        {
            get
            {
                return true;
            }
        }

        public Vec Pos { get => pos; private set => pos = value; }

        public void Reset(Rectangle rect)
        {
            this.Pos = resources.InitialPos;
            this.rect = rect;
        }

        public void Update(Stage world)
        {
        }

        public void Render(Graphics g)
        {
            //
            // render world
            //

            // how many rects in the viewport
            var ys = rect.Height / resources.TileSize.y;
            var xs = rect.Width / resources.TileSize.x;

            // the x,y converted to offset in the map
            var yt = (int)(Math.Max(pos.y, 0) / resources.TileSize.y); var ym = pos.y % resources.TileSize.y;
            var xt = (int)(Math.Max(pos.x, 0) / resources.TileSize.x); var xm = pos.x % resources.TileSize.x;
            var offset = (int)(yt * resources.Size.x + xt);

            // render
            var yp = resources.TileSize.y - ym;
            for (var y = yt; y < Math.Min(yt + ys, resources.Size.y); y++)
            {
                var offset_row = offset;
                var xp = xm;
                for (var x = xt; x < Math.Min(xt + xs, resources.Size.x); x++)
                {
                    var tile = resources.DefaultLayer[offset_row++];
                    var rect = new RectangleF((int)xp, (int)yp, resources.TileSize.x, resources.TileSize.y);
                    if (stage.Current == (x, y))
                    {
                        g.DrawImage(resources.Set, rect, resources.TileRectCache[Stage.Player], GraphicsUnit.Pixel);
                    }
                    else if (tile.landing && tile.block == Stage.Free)
                    {
                        g.DrawImage(resources.Set, rect, resources.TileRectCache[Stage.Landing], GraphicsUnit.Pixel);
                    }
                    else
                    {
                        g.DrawImage(resources.Set, rect, resources.TileRectCache[tile.block], GraphicsUnit.Pixel);
                    }
                    xp += resources.TileSize.x;
                }
                offset += resources.Size.x;
                yp += resources.TileSize.y;
            }
        }
    }
}
