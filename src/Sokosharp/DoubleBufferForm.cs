using SokoSharp.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SokoSharp
{
    public partial class DoubleBufferForm : Form
    {
        public DoubleBufferForm()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.Font = new Font("C64 Pro Mono Normale", 12);
        }

        BufferedGraphics bufferedGraphics;

        public DoubleBufferForm Initialize(Stage stage, StageResources resources)
        {
            this.stage = stage;
            this.resources = resources;
            view = new ScrollTopView(stage, resources);
            this.ClientSize = new Size(resources.Size.x * resources.TileSize.x + 8, (resources.Size.y + 1) * resources.TileSize.y);
            this.Text = resources.Title;
            return this;
        }

        private Stage stage;
        private StageResources resources;
        private ScrollTopView view;

        public bool Suspended { get;  set; } = true;
        public int FrameRate { get; set; }

        public bool Jump { get; set; }
        public bool MoveLeft { get; set; }
        public bool MoveRight { get; set; }
        public bool MoveUp { get; set; }
        public bool MoveDown { get; set; }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);

            if (this.bufferedGraphics != null)
            {
                this.Suspended = true;
                this.bufferedGraphics.Dispose();
                this.bufferedGraphics = null;
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.bufferedGraphics == null)
            {
                this.bufferedGraphics = BufferedGraphicsManager.Current.Allocate(e.Graphics,
                    //new Rectangle(resources.TileSize.x, resources.TileSize.y, this.ClientRectangle.Width - resources.TileSize.x * 2, this.ClientRectangle.Height - resources.TileSize.y * 2)
                    this.ClientRectangle
                );

                view.Reset(this.ClientRectangle);
                this.Suspended = false;
            }
            else
            {
                this.bufferedGraphics.Render(e.Graphics);
            }
        }

        public void FrameUpdate()
        {
            if (!view.CanUpdate) return;

            if (MoveDown) stage.MoveUp();
            if (MoveUp) stage.MoveDown();
            if (MoveLeft) stage.MoveLeft();
            if (MoveRight) stage.MoveRight();

            stage.Update();

            view.Update(stage);
        }

        public void FrameRender()
        {
            bufferedGraphics.Graphics.Clear(Color.Black);
            view.Render(bufferedGraphics.Graphics);
            //bufferedGraphics.Graphics.DrawString($"FrameRate={FrameRate} View=({view.Pos.x},{view.Pos.y}) Car=({stage.Current.x},{stage.Current.y})", this.Font, Brushes.White, 32, 32);
            this.Invalidate();
        }
    }
}
