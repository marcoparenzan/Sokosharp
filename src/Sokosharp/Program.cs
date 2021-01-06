using SokoSharp.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SokoSharp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var resources = StageResources.Get();
            var stage = new Stage(resources);

            var form =
                new DoubleBufferForm()
            ;
            form.Initialize(stage, resources);
            //form.FormBorderStyle = FormBorderStyle.None;
            //form.WindowState = FormWindowState.Maximized;
            form.StartPosition = FormStartPosition.CenterScreen;
            //form.Size = new Size(960, 640);
            form.KeyDown += (s, e) => {

                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        form.Close();
                        break;
                    case Keys.Space:
                        form.Jump = true;
                        break;
                    case Keys.Left:
                        form.MoveLeft = true;
                        break;
                    case Keys.Right:
                        form.MoveRight = true;
                        break;
                    case Keys.Up:
                        form.MoveUp = true;
                        break;
                    case Keys.Down:
                        form.MoveDown = true;
                        break;
                    default:
                        break;
                }
            
            };
            form.KeyUp += (s, e) => {

                switch (e.KeyCode)
                {
                    case Keys.Space:
                        form.Jump = false;
                        break;
                    case Keys.Left:
                        form.MoveLeft = false;
                        break;
                    case Keys.Right:
                        form.MoveRight = false;
                        break;
                    case Keys.Up:
                        form.MoveUp = false;
                        break;
                    case Keys.Down:
                        form.MoveDown = false;
                        break;
                    default:
                        break;
                }

            };
            form.Show();

            var refrate = (int) Math.Round(1000.0 / 15, 0);

            var timer = new Timer();
            timer.Interval = refrate;
            timer.Tick += (s, e) =>
            {
                var start = DateTime.Now;

                form.FrameRender();
                form.FrameUpdate();

                var stop = DateTime.Now;
                form.FrameRate = (int)Math.Round(1000.0 / (stop - start).TotalMilliseconds, 0);

            };
            timer.Start();

            Application.Run(form);
        }
    }
}
