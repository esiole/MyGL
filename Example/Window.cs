using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using MyGL;
using System.Globalization;

namespace Example 
{
    public class Window : Form
    {
        private bool isLoad = false;                // загружен ли холст
        private Point mouse;                        // координаты мышки
        private bool isMove = false;                // начато ли вращение
        private float xAxisRotation = 0.0f;         // углы поворота
        private float yAxisRotation = 0.0f;
        private float fov = 45.0f;                  // угол просмотра перспективной проекции
        private bool isOrto = false;                // выбранная проекция
        private Shader shader;                      // шейдерная программа
        private LightingParameters light;
        private Cone Conus;
        private Cube Cube;
        private Cylinder Cylinder;
        private Sheet BottomScene;
        private Sheet Wall_1;
        private Sheet Wall_2;

        private Vector3 LightPos;                   // позиция источника света
        private Cube Light;                         // источник света
        private Vector3 LightDirection;             // направление прожектора
        private float isSpotlight;                  // прожектор (> 0) или точечный источник (< 0)
        private Vector3 LightOffsetPos;             // смещение положения источника света
        private Scene GraphicScene;

        private GLControl Canvas;
        private ComboBox comboBox1;
        private ComboBox comboBox2;

        public const string WindowFontName = "Microsoft Sans Serif";
        public const Single WindowFontSize = 14F;

        public Window()
        {
            Canvas = new GLControl()
            {
                Dock = DockStyle.Fill,
            };
            this.Canvas.Load += new System.EventHandler(this.Canvas_Load);
            this.Canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.Canvas_Paint);
            this.Canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDown);
            this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            this.Canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseUp);
            this.Canvas.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Canvas_OnMouseWheel);
            this.Canvas.Resize += new System.EventHandler(this.Canvas_Resize);

            comboBox1 = new ComboBox()
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);

            comboBox2 = new ComboBox()
            {
                Dock = DockStyle.Fill,
                DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList,
            };
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);

            var textBox1 = CreateTextBox(new Action<float>(value => light.SetDirectionVector(x: value)));
            var textBox2 = CreateTextBox(new Action<float>(value => light.SetDirectionVector(y: value)));
            var textBox3 = CreateTextBox(new Action<float>(value => light.SetDirectionVector(z: value)));

            var textBox4 = CreateTextBox(new Action<float>(value => light.SetLightOffsetPos(x: value)));
            var textBox5 = CreateTextBox(new Action<float>(value => light.SetLightOffsetPos(y: value)));
            var textBox6 = CreateTextBox(new Action<float>(value => light.SetLightOffsetPos(z: value)));

            var table = new TableLayoutPanel();
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 25));

            table.Controls.Add(Canvas, 0, 0);
            table.SetRowSpan(Canvas, 4);
            table.Controls.Add(CreateTableLayoutForComboBox("Проекция:", comboBox1), 1, 0);
            table.Controls.Add(CreateTableLayoutForCoord("Направление прожектора:", textBox1, textBox2, textBox3), 1, 1);
            table.Controls.Add(CreateTableLayoutForComboBox("Источник света:", comboBox2), 1, 2);
            table.Controls.Add(CreateTableLayoutForCoord("Координаты лампы:", textBox4, textBox5, textBox6), 1, 3);

            table.Dock = DockStyle.Fill;
            Controls.Add(table);

            Font = new Font(WindowFontName, WindowFontSize);
            AutoScaleMode = AutoScaleMode.None;
            Size = new Size(1550, 750);
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Example";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);

            comboBox1.Items.AddRange(new string[] { "Перспективная", "Ортогональная" });
            comboBox1.SelectedIndex = 0;
            comboBox2.Items.AddRange(new string[] { "Прожектор", "Точечный" });
            comboBox2.SelectedIndex = 0;
            textBox1.Text = "-0,2";
            textBox2.Text = "0,45";
            textBox3.Text = "0,0";
            textBox4.Text = "0,0";
            textBox5.Text = "-0,5";
            textBox6.Text = "0,5";
        }

        private Label CreateLabel(string text)
        {
            return new Label()
            {
                Dock = DockStyle.Fill,
                Text = text,
            };
        }

        private TableLayoutPanel CreateTableLayoutForCoord(string headText, TextBox xBox, TextBox yBox, TextBox zBox)
        {
            var label = CreateLabel(headText);
            var tableCoord = new TableLayoutPanel();
            tableCoord.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tableCoord.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tableCoord.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            tableCoord.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            tableCoord.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            tableCoord.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            tableCoord.Controls.Add(label, 0, 0);
            tableCoord.SetColumnSpan(label, 2);
            tableCoord.Controls.Add(CreateLabel("X:"), 0, 1);
            tableCoord.Controls.Add(xBox, 1, 1);
            tableCoord.Controls.Add(CreateLabel("Y:"), 0, 2);
            tableCoord.Controls.Add(yBox, 1, 2);
            tableCoord.Controls.Add(CreateLabel("Z:"), 0, 3);
            tableCoord.Controls.Add(zBox, 1, 3);
            tableCoord.Dock = DockStyle.Fill;
            return tableCoord;
        }

        private TableLayoutPanel CreateTableLayoutForComboBox(string headText, ComboBox box)
        {
            var tableBox = new TableLayoutPanel();
            tableBox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tableBox.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            tableBox.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            tableBox.Controls.Add(CreateLabel(headText), 0, 0);
            tableBox.Controls.Add(box, 0, 1);
            tableBox.Dock = DockStyle.Fill;
            return tableBox;
        }

        private TextBox CreateTextBox(Action<float> changeLightingVector)
        {
            var textBox = new TextBox()
            {
                Dock = DockStyle.Fill,
            };
            textBox.TextChanged += (sender, e) =>
            {
                float value;
                try
                {
                    value = float.Parse((sender as TextBox).Text);

                }
                catch
                {
                    value = float.Parse((sender as TextBox).Text, CultureInfo.InvariantCulture);
                }
                if (light != null) changeLightingVector(value);
            };
            return textBox;
        }

        private void Canvas_Load(object sender, EventArgs e)
        {
            GL.ClearColor(0.59f, 0.59f, 0.59f, 1.0f);
            GL.Enable(EnableCap.DepthTest);


            shader = new Shader(new PhongShaderSource());
            //CameraPos = new Vector3(-1.0f, -1.0f, 1.0f);  // позиция камеры
            LightPos = new Vector3(0.0f, -0.5f, 0.5f);
            Light = new Cube(LightPos, 0.05f, new Vector3(1.0f, 1.0f, 1.0f), new Material(), Matrix4.Identity);
            LightDirection = new Vector3(-0.2f, 0.45f, 0.0f) - LightPos;
            isSpotlight = 1.0f;
            LightOffsetPos = new Vector3(0.0f, -0.5f, 0.5f);


            var ambient = new Vector3(0.2f, 0.4f, 0.6f);
            var diffuse = new Vector3(0.8f, 0.9f, 0.5f);
            var specular = new Vector3(1.0f, 0.8f, 1.0f);
            var cutOff = (float)Math.Cos(MathHelper.DegreesToRadians(18.5));  // угол прожектора
            var outerCutOff = (float)Math.Cos(MathHelper.DegreesToRadians(29.5));
            var constant = 1.0f;
            var linear = 0.35f;
            var quadratic = 0.44f;

            light = new LightingParameters(ambient, diffuse, specular, LightPos, LightDirection, LightOffsetPos, cutOff, outerCutOff, 
                isSpotlight, constant, linear, quadratic);

            GraphicScene = new Scene(shader);
            Matrix4 model = Matrix4.Identity;

            Matrix4 cylinder = Matrix4.Mult(Matrix4.CreateTranslation(0.4f, 0.0f, 0.0f), model);
            cylinder = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.1f, 0.0f), cylinder);
            Matrix4 conus = Matrix4.Mult(Matrix4.CreateTranslation(-0.4f, 0.4f, 0.0f), model);
            Matrix4 cube = Matrix4.Mult(Matrix4.CreateTranslation(0.1f, 0.5f, 0.0f), model);

            Conus = new Cone(new Vector3(0.0f, 0.0f, 0.0f), 0.1f, 0.3f, new YellowPlastic(), conus);
            Cube = new Cube(new Vector3(0.0f, 0.0f, 0.15f), 0.3f, new Vector3(1.0f, 0.0f, 1.0f), new Jade(), cube);
            Cylinder = new Cylinder(new Vector3(0.0f, 0.0f, 0.0f), 0.1f, 0.3f, new Bronze(), cylinder);
            BottomScene = new Sheet(new Vector3(1.0f, -1.0f, 0.0f), new Vector3(1.0f, 1.0f, 0.0f), new Vector3(-1.0f, -1.0f, 0.0f), new Vector3(-1.0f, 1.0f, 0.0f), new Jade(), Matrix4.Identity);
            Wall_1 = new Sheet(new Vector3(1.0f, 1.0f, 0.0f), new Vector3(1.0f, -1.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, -1.0f, 1.0f), new Jade(), Matrix4.Identity);
            Wall_2 = new Sheet(new Vector3(-1.0f, 1.0f, 0.0f), new Vector3(1.0f, 1.0f, 0.0f), new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Jade(), Matrix4.Identity);
            
            GraphicScene.Add(Conus);
            GraphicScene.Add(Cube);
            GraphicScene.Add(Cylinder);
            GraphicScene.Add(BottomScene);
            GraphicScene.Add(Wall_1);
            GraphicScene.Add(Wall_2);
            GraphicScene.Add(Light);

            light.ParametersChange += () => Canvas.Invalidate();
            isLoad = true;
        }

        private void Canvas_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, Canvas.Width, Canvas.Height);
        }

        [Obsolete]
        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            if (!isLoad) return;
            Matrix4 model = Matrix4.Identity;
            Matrix4 light = Matrix4.Mult(Matrix4.CreateTranslation(this.light.LightOffsetPos - this.light.Position), model);

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

            shader.SetLightingParameters(this.light, this.light.LightOffsetPos);
            GraphicScene.View = view;
            GraphicScene.Projection = projection;
            GraphicScene.CameraPos = new Vector3(-1.0f, -1.0f, 1.0f);
            Light.Model = light;
            //shader.SetUniform3("light.ambient", new Vector3(1.0f, 1.0f, 1.0f));
            //shader.SetUniform3("light.diffuse", new Vector3(1.0f, 1.0f, 1.0f));
            //shader.SetUniform3("light.specular", new Vector3(1.0f, 1.0f, 1.0f));
            GraphicScene.Draw();

            GL.Disable(EnableCap.PointSmooth);
            GL.Disable(EnableCap.LineSmooth);
            Canvas.SwapBuffers();
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            shader.Dispose();
        }

        private void Canvas_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouse = e.Location;
            isMove = true;
        }

        private void Canvas_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (isMove)
            {
                float sensitivity = 0.0005f;
                xAxisRotation += (180 * (e.Location.X - mouse.X)) / (float)Canvas.Width * sensitivity;
                yAxisRotation += (180 * (e.Location.Y - mouse.Y)) / (float)Canvas.Height * sensitivity;
            }
            Canvas.Invalidate();
        }

        private void Canvas_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            isMove = false;
        }

        private void Canvas_OnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
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
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Перспективная")
            {
                isOrto = false;
            }
            if (comboBox1.SelectedItem.ToString() == "Ортогональная")
            {
                isOrto = true;
            }
            Canvas.Invalidate();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "Прожектор")
            {
                isSpotlight = 1.0f;
            }
            if (comboBox2.SelectedItem.ToString() == "Точечный")
            {
                isSpotlight = -1.0f;
            }
            Canvas.Invalidate();
        }
    }
}
