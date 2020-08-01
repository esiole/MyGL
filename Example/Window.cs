using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using MyGL;

namespace Example 
{
    public class Window : Form
    {
        private bool isDisposed = false;
        private bool isLoad = false;                // загружен ли холст
        private Point mouse;                        // координаты мышки
        private bool isMove = false;                // начато ли вращение
        private float xAxisRotation = 0.0f;         // углы поворота
        private float yAxisRotation = 0.0f;
        private float fov = 45.0f;                  // угол просмотра перспективной проекции
        private bool isOrto = false;                // выбранная проекция

        private Scene GraphicScene;

        public readonly GLControl Canvas;
        public readonly ComboBox ComboBoxProjection;
        public readonly ToolStripLabel Status;

        public const string WindowFontName = "Microsoft Sans Serif";
        public const Single WindowFontSize = 14F;

        public Window()
        {
            Canvas = new GLControl
            {
                Dock = DockStyle.Fill,
            };
            Canvas.Load += new EventHandler(CanvasLoad);
            Canvas.Paint += new PaintEventHandler(CanvasPaint);
            Canvas.Resize += (sender, e) => GL.Viewport(0, 0, Canvas.Width, Canvas.Height);
            Canvas.MouseDown += (sender, e) =>
            {
                mouse = e.Location;
                isMove = true;
            };
            Canvas.MouseMove += (sender, e) =>
            {
                if (isMove)
                {
                    float sensitivity = 0.0005f;
                    xAxisRotation += (180 * (e.Location.X - mouse.X)) / (float)Canvas.Width * sensitivity;
                    yAxisRotation += (180 * (e.Location.Y - mouse.Y)) / (float)Canvas.Height * sensitivity;
                }
                Canvas.Invalidate();
            };
            Canvas.MouseUp += (sender, e) => isMove = false;
            Canvas.MouseWheel += (sender, e) =>
            {
                float sensitivity = 0.01f;
                fov -= e.Delta * sensitivity;
                if (fov >= 45.0f)
                {
                    fov = 45.0f;
                }
                if (fov <= 1.0f)
                {
                    fov = 1.0f;
                }
                Canvas.Invalidate();
            };

            ComboBoxProjection = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            ComboBoxProjection.Items.AddRange(new string[] { "Перспективная", "Ортогональная" });
            ComboBoxProjection.SelectedIndex = 0;
            ComboBoxProjection.SelectedIndexChanged += (sender, e) =>
            {
                if (ComboBoxProjection.SelectedItem.ToString() == "Перспективная")
                    isOrto = false;
                if (ComboBoxProjection.SelectedItem.ToString() == "Ортогональная")
                    isOrto = true;
                Canvas.Invalidate();
            };

            var table = new TableLayoutPanel();
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 80));
            table.Controls.Add(Canvas, 0, 0);
            table.SetRowSpan(Canvas, 3);
            table.Controls.Add(CreateLabel("Проекция:"), 1, 0);
            table.Controls.Add(ComboBoxProjection, 1, 1);
            table.Dock = DockStyle.Fill;
            Controls.Add(table);

            var statusStrip = new StatusStrip();
            Status = new ToolStripLabel
            {
                Text = "Загрузка"
            };
            statusStrip.Items.Add(Status);
            statusStrip.RightToLeft = RightToLeft.Yes;
            Controls.Add(statusStrip);

            Font = new Font(WindowFontName, WindowFontSize);
            AutoScaleMode = AutoScaleMode.None;
            Size = new Size(1550, 750);
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Example";
        }

        private Label CreateLabel(string text)
        {
            return new Label
            {
                Dock = DockStyle.Fill,
                Text = text,
            };
        }

        private void CanvasLoad(object sender, EventArgs e)
        {
            GL.ClearColor(0.59f, 0.59f, 0.59f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            var LightPos = new Vector3(0.0f, -0.5f, 0.5f);

            var pointLight = new PointLight
            {
                Ambient = new Vector3(0.2f, 0.4f, 0.6f),
                Diffuse = new Vector3(0.8f, 0.9f, 0.5f),
                Specular = new Vector3(1.0f, 0.8f, 1.0f),
                Position = new Vector3(0.0f, -0.5f, 0.5f),
                Constant = 1.0f,
                Linear = 0.35f,
                Quadratic = 0.44f,
                Source = new Cube(LightPos, 0.05f, Material.LightSource, Matrix4.Identity),
            };
            
            var spotLight = new SpotLight
            {
                Ambient = new Vector3(0.2f, 0.4f, 0.6f),
                Diffuse = new Vector3(0.8f, 0.9f, 0.5f),
                Specular = new Vector3(1.0f, 0.8f, 1.0f),
                Position = LightPos,
                Direction = new Vector3(-0.2f, 0.45f, 0.0f) - LightPos,
                Constant = 1.0f,
                Linear = 0.35f,
                Quadratic = 0.44f,
                CutOff = (float)Math.Cos(MathHelper.DegreesToRadians(18.5)),
                OuterCutOff = (float)Math.Cos(MathHelper.DegreesToRadians(29.5)),
            };

            var spotLight1 = new SpotLight
            {
                Ambient = new Vector3(0.2f, 0.4f, 0.6f),
                Diffuse = new Vector3(0.8f, 0.9f, 0.5f),
                Specular = new Vector3(1.0f, 0.8f, 1.0f),
                Position = LightPos,
                Direction = new Vector3(0.2f, -0.9f, 0.0f) - LightPos,
                Constant = 1.0f,
                Linear = 0.35f,
                Quadratic = 0.44f,
                CutOff = (float)Math.Cos(MathHelper.DegreesToRadians(18.5)),
                OuterCutOff = (float)Math.Cos(MathHelper.DegreesToRadians(29.5)),
            };

            Matrix4 model = Matrix4.Identity;
            Matrix4 cylinder = Matrix4.Mult(Matrix4.CreateTranslation(0.4f, 0.0f, 0.0f), model);
            cylinder = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.1f, 0.0f), cylinder);
            Matrix4 conus = Matrix4.Mult(Matrix4.CreateTranslation(-0.4f, 0.4f, 0.0f), model);
            Matrix4 cube = Matrix4.Mult(Matrix4.CreateTranslation(0.1f, 0.5f, 0.0f), model);

            GraphicScene = new Scene(ShaderSource.Phong(1, 0))
            {
                Shapes = new List<Shape>
                {
                    new Cone(new Vector3(0.0f, 0.0f, 0.0f), 0.1f, 0.3f, Material.Ruby, conus),
                    new Cube(new Vector3(0.0f, 0.0f, 0.15f), 0.3f, Material.Pearl, cube),
                    new Cylinder(new Vector3(0.0f, 0.0f, 0.0f), 0.1f, 0.3f, Material.Bronze, cylinder),
                    new Sheet(new Vector3(1.0f, -1.0f, 0.0f), new Vector3(1.0f, 1.0f, 0.0f), new Vector3(-1.0f, -1.0f, 0.0f), new Vector3(-1.0f, 1.0f, 0.0f), Material.YellowPlastic, Matrix4.Identity),
                    new Sheet(new Vector3(1.0f, 1.0f, 0.0f), new Vector3(1.0f, -1.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, -1.0f, 1.0f), Material.YellowPlastic, Matrix4.Identity),
                    new Sheet(new Vector3(-1.0f, 1.0f, 0.0f), new Vector3(1.0f, 1.0f, 0.0f), new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), Material.YellowPlastic, Matrix4.Identity),
                },
                //Lights = new List<LightSource>
                //{
                //    pointLight,
                //},
            };

            GraphicScene.Add(pointLight);
            //GraphicScene.Add(spotLight);
            //GraphicScene.Add(spotLight1);

            isLoad = true;
            Status.Text = "Готово";
        }

        [Obsolete]
        private void CanvasPaint(object sender, PaintEventArgs e)
        {
            if (!isLoad) return;

            Matrix4 left = Matrix4.CreateRotationX(xAxisRotation);
            Matrix4 right = Matrix4.CreateRotationY(yAxisRotation);
            Matrix4 view = Matrix4.LookAt(GraphicScene.CameraPos, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f));
            view = Matrix4.Mult(left, view);
            view = Matrix4.Mult(right, view);
            Matrix4 projection = isOrto ? Matrix4.CreateOrthographicOffCenter(-1.0f, 1.0f, -1.0f, 1.0f, 0.1f, 100.0f) :
                Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), (float)Canvas.Width / (float)Canvas.Height, 0.1f, 100.0f);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.PointSmooth);
            GL.Enable(EnableCap.LineSmooth);

            GraphicScene.View = view;
            GraphicScene.Projection = projection;
            GraphicScene.CameraPos = new Vector3(-1.0f, -1.0f, 1.0f);
            GraphicScene.Draw();

            GL.Disable(EnableCap.PointSmooth);
            GL.Disable(EnableCap.LineSmooth);
            Canvas.SwapBuffers();
        }

        protected override void Dispose(bool fromDisposeMethod)
        {
            if (!isDisposed)
            {
                if (fromDisposeMethod)
                {
                    GraphicScene.Dispose();
                }
                isDisposed = true;
                base.Dispose(fromDisposeMethod);
            }
        }
    }
}
