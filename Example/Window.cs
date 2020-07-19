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

namespace Example 
{
    public class Window : System.Windows.Forms.Form
    {
        private string VertexShaderSource = @"
            #version 330 core

            layout (location = 0) in vec3 aPosition;
            layout (location = 1) in vec3 aColor;
            layout (location = 2) in vec3 aNormal;
            

            uniform mat4 model;
            uniform mat4 view;
            uniform mat4 projection;
            
            varying vec3 FragPos;
            varying vec3 Color;
            varying vec3 Normal;
            
            void main()
            {
                gl_Position = projection * view * model * vec4(aPosition, 1.0);
                Color = aColor;
                Normal = aNormal * mat3(transpose(inverse(model)));
                FragPos = vec3(model * vec4(aPosition, 1.0));
            }
        ";
        private string FragmentShaderSource = @"
            #version 330 core

            struct Material 
            {
                vec3 ambient;
                vec3 diffuse;
                vec3 specular;
                float shininess;
            };

            struct Light 
            {
                vec3 ambient;
                vec3 diffuse;
                vec3 specular;
                vec3 position;

                // attenuation
                float constant;
                float linear;
                float quadratic;

                // for spotlight
                float isSpotlight;
                vec3 direction;
                float cutOff;
                float outerCutOff;
            };

            varying vec3 Color; 
            varying vec3 Normal;
            varying vec3 FragPos;

            uniform Material material;
            uniform Light light;
            uniform vec3 viewPos;

            void main()
            {
                vec3 ambient = light.ambient * material.ambient;

                vec3 norm = normalize(Normal);
                vec3 lightDir = normalize(light.position - FragPos);
                float diff = max(dot(norm, lightDir), 0.0);
                vec3 diffuse = light.diffuse * (diff * material.diffuse);       

                vec3 viewDir = normalize(viewPos - FragPos);
                vec3 reflectDir = reflect(-lightDir, norm);
                float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
                vec3 specular = light.specular * (spec * material.specular);

                float theta = dot(lightDir, normalize(-light.direction));
                float epsilon = light.cutOff - light.outerCutOff;
                float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

                float distance = length(light.position - FragPos);
                float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

                if(light.isSpotlight > 0.0)
                {
                    diffuse  *= intensity;
                    specular *= intensity;
                }

                ambient  *= attenuation;
                diffuse  *= attenuation;
                specular *= attenuation;
                    
                vec3 result = (ambient + diffuse + specular) * Color;

                gl_FragColor = vec4(result, 1.0f);
            }
        ";
        private bool isLoad = false;                // загружен ли холст
        private Point mouse;                        // координаты мышки
        private bool isMove = false;                // начато ли вращение
        private float xAxisRotation = 0.0f;         // углы поворота
        private float yAxisRotation = 0.0f;
        private float fov = 45.0f;                  // угол просмотра перспективной проекции
        private bool isOrto = false;                // выбранная проекция
        private Shader shader;                      // шейдерная программа
        private Cone Conus;
        private Cube Cube;
        private CoordAxis Axis;
        private Cylinder Cylinder;
        private Sheet BottomScene;
        private Sheet Wall_1;
        private Sheet Wall_2;
        private Sheet Wall_3;
        private Sheet Wall_4;

        private Vector3 CameraPos;                  // позиция камеры
        private Vector3 LightPos;                   // позиция источника света
        private Cube Light;                         // источник света
        private Vector3 LightDirection;             // направление прожектора
        private float cutOff;                       // угол прожектора
        private float outerCutOff;
        private float isSpotlight;                  // прожектор (> 0) или точечный источник (< 0)
        private Vector3 LightOffsetPos;             // смещение положения источника света

        private float constant;
        private float linear;
        private float quadratic;



        private OpenTK.GLControl Canvas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;



        public Window()
        {
            this.Canvas = new OpenTK.GLControl();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.SuspendLayout();

            this.Canvas.BackColor = System.Drawing.Color.Black;
            this.Canvas.Dock = System.Windows.Forms.DockStyle.Left;
            this.Canvas.Location = new System.Drawing.Point(0, 0);
            this.Canvas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(1156, 738);
            this.Canvas.TabIndex = 0;
            this.Canvas.VSync = false;
            this.Canvas.Load += new System.EventHandler(this.Canvas_Load);
            this.Canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.Canvas_Paint);
            this.Canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDown);
            this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            this.Canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseUp);
            this.Canvas.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Canvas_OnMouseWheel);
            this.Canvas.Resize += new System.EventHandler(this.Canvas_Resize);

            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label1.Location = new System.Drawing.Point(1251, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "Проекция:";

            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(1194, 74);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(246, 37);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);

            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label2.Location = new System.Drawing.Point(1233, 232);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 29);
            this.label2.TabIndex = 3;
            this.label2.Text = "Направление";

            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label3.Location = new System.Drawing.Point(1256, 330);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 29);
            this.label3.TabIndex = 4;
            this.label3.Text = "X:";

            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label4.Location = new System.Drawing.Point(1257, 381);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 29);
            this.label4.TabIndex = 5;
            this.label4.Text = "Y:";

            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label5.Location = new System.Drawing.Point(1259, 440);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 29);
            this.label5.TabIndex = 6;
            this.label5.Text = "Z:";

            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.textBox1.Location = new System.Drawing.Point(1300, 327);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(74, 34);
            this.textBox1.TabIndex = 7;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);

            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.textBox2.Location = new System.Drawing.Point(1300, 381);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(74, 34);
            this.textBox2.TabIndex = 8;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);

            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.textBox3.Location = new System.Drawing.Point(1300, 437);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(74, 34);
            this.textBox3.TabIndex = 9;
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);

            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label6.Location = new System.Drawing.Point(1245, 270);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(159, 29);
            this.label6.TabIndex = 10;
            this.label6.Text = "прожектора:";

            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label7.Location = new System.Drawing.Point(1219, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(201, 29);
            this.label7.TabIndex = 11;
            this.label7.Text = "Источник света:";

            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(1194, 172);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(246, 37);
            this.comboBox2.TabIndex = 12;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);

            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label8.Location = new System.Drawing.Point(1206, 490);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(247, 29);
            this.label8.TabIndex = 13;
            this.label8.Text = "Координаты лампы:";

            this.textBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.textBox6.Location = new System.Drawing.Point(1300, 649);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(74, 34);
            this.textBox6.TabIndex = 19;
            this.textBox6.TextChanged += new System.EventHandler(this.textBox6_TextChanged);

            this.textBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.textBox5.Location = new System.Drawing.Point(1300, 593);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(74, 34);
            this.textBox5.TabIndex = 18;
            this.textBox5.TextChanged += new System.EventHandler(this.textBox5_TextChanged);

            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.textBox4.Location = new System.Drawing.Point(1300, 539);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(74, 34);
            this.textBox4.TabIndex = 17;
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);

            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label9.Location = new System.Drawing.Point(1259, 652);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 29);
            this.label9.TabIndex = 16;
            this.label9.Text = "Z:";

            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label10.Location = new System.Drawing.Point(1257, 593);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 29);
            this.label10.TabIndex = 15;
            this.label10.Text = "Y:";

            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label11.Location = new System.Drawing.Point(1256, 542);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(36, 29);
            this.label11.TabIndex = 14;
            this.label11.Text = "X:";

            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1465, 738);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Canvas);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Example";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

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

        private void Canvas_Load(object sender, EventArgs e)
        {
            GL.ClearColor(0.59f, 0.59f, 0.59f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            shader = new Shader(VertexShaderSource, FragmentShaderSource);
            CameraPos = new Vector3(-1.0f, -1.0f, 1.0f);
            LightPos = new Vector3(0.0f, -0.5f, 0.5f);
            Light = new Cube(LightPos, 0.05f, new Vector3(1.0f, 1.0f, 1.0f));
            LightDirection = new Vector3(-0.2f, 0.45f, 0.0f) - LightPos;
            cutOff = (float)Math.Cos(MathHelper.DegreesToRadians(18.5));
            outerCutOff = (float)Math.Cos(MathHelper.DegreesToRadians(29.5));
            isSpotlight = 1.0f;
            LightOffsetPos = new Vector3(0.0f, -0.5f, 0.5f);

            constant = 1.0f;
            linear = 0.35f;
            quadratic = 0.44f;

            Conus = new Cone(new Vector3(0.0f, 0.0f, 0.0f), 0.1f, 0.3f);
            Cube = new Cube(new Vector3(0.0f, 0.0f, 0.15f), 0.3f, new Vector3(1.0f, 0.0f, 1.0f));
            Cylinder = new Cylinder(new Vector3(0.0f, 0.0f, 0.0f), 0.1f, 0.3f);
            Axis = new CoordAxis(1.0f);
            BottomScene = new Sheet(new Vector3(1.0f, -1.0f, 0.0f), new Vector3(1.0f, 1.0f, 0.0f), new Vector3(-1.0f, -1.0f, 0.0f), new Vector3(-1.0f, 1.0f, 0.0f));
            Wall_1 = new Sheet(new Vector3(1.0f, 1.0f, 0.0f), new Vector3(1.0f, -1.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, -1.0f, 1.0f));
            Wall_2 = new Sheet(new Vector3(-1.0f, 1.0f, 0.0f), new Vector3(1.0f, 1.0f, 0.0f), new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f));
            Wall_3 = new Sheet(new Vector3(-1.0f, -1.0f, 0.0f), new Vector3(-1.0f, 1.0f, 0.0f), new Vector3(-1.0f, -1.0f, 1.0f), new Vector3(-1.0f, 1.0f, 1.0f));
            Wall_4 = new Sheet(new Vector3(1.0f, -1.0f, 0.0f), new Vector3(-1.0f, -1.0f, 0.0f), new Vector3(1.0f, -1.0f, 1.0f), new Vector3(-1.0f, -1.0f, 1.0f));

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

            Matrix4 cylinder = Matrix4.Mult(Matrix4.CreateTranslation(0.4f, 0.0f, 0.0f), model);
            cylinder = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.1f, 0.0f), cylinder);
            Matrix4 conus = Matrix4.Mult(Matrix4.CreateTranslation(-0.4f, 0.4f, 0.0f), model);
            Matrix4 cube = Matrix4.Mult(Matrix4.CreateTranslation(0.1f, 0.5f, 0.0f), model);
            Matrix4 light = Matrix4.Mult(Matrix4.CreateTranslation(LightOffsetPos - LightPos), model);

            Matrix4 left = Matrix4.CreateRotationX(xAxisRotation);
            Matrix4 right = Matrix4.CreateRotationY(yAxisRotation);
            Matrix4 view = Matrix4.LookAt(CameraPos, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f));
            view = Matrix4.Mult(left, view);
            view = Matrix4.Mult(right, view);
            Matrix4 projection = isOrto ? Matrix4.CreateOrthographicOffCenter(-1.0f, 1.0f, -1.0f, 1.0f, 0.1f, 100.0f) :
                Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), (float)Canvas.Width / (float)Canvas.Height, 0.1f, 100.0f);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.PointSmooth);
            GL.Enable(EnableCap.LineSmooth);
            shader.Use();
            shader.SetUniformMatrix4("model", false, model);
            shader.SetUniformMatrix4("view", false, view);
            shader.SetUniformMatrix4("projection", false, projection);
            shader.SetUniform3("viewPos", CameraPos);

            shader.SetUniform3("light.ambient", new Vector3(0.2f, 0.4f, 0.6f));
            shader.SetUniform3("light.diffuse", new Vector3(0.8f, 0.9f, 0.5f));
            shader.SetUniform3("light.specular", new Vector3(1.0f, 0.8f, 1.0f));
            shader.SetUniform3("light.position", LightOffsetPos);
            shader.SetUniform3("light.direction", LightDirection);
            shader.SetUniform1("light.cutOff", cutOff);
            shader.SetUniform1("light.outerCutOff", outerCutOff);
            shader.SetUniform1("light.isSpotlight", isSpotlight);
            shader.SetUniform1("light.constant", constant);
            shader.SetUniform1("light.linear", linear);
            shader.SetUniform1("light.quadratic", quadratic);

            shader.SetUniformMatrix4("model", false, cylinder);
            // bronze
            shader.SetUniform3("material.ambient", new Vector3(0.2125f, 0.1275f, 0.054f));
            shader.SetUniform3("material.diffuse", new Vector3(0.714f, 0.4284f, 0.18144f));
            shader.SetUniform3("material.specular", new Vector3(0.393548f, 0.271906f, 0.166721f));
            shader.SetUniform1("material.shininess", 32.0f);
            Cylinder.Draw();
            shader.SetUniformMatrix4("model", false, conus);
            // yellow plastic
            shader.SetUniform3("material.ambient", new Vector3(0.0f, 0.0f, 0.0f));
            shader.SetUniform3("material.diffuse", new Vector3(0.5f, 0.5f, 0.0f));
            shader.SetUniform3("material.specular", new Vector3(0.60f, 0.60f, 0.50f));
            shader.SetUniform1("material.shininess", 32.0f);
            Conus.Draw();
            shader.SetUniformMatrix4("model", false, cube);
            // нефрит
            shader.SetUniform3("material.ambient", new Vector3(0.135f, 0.2225f, 0.1575f));
            shader.SetUniform3("material.diffuse", new Vector3(0.54f, 0.89f, 0.63f));
            shader.SetUniform3("material.specular", new Vector3(0.316228f, 0.316228f, 0.316228f));
            shader.SetUniform1("material.shininess", 32.0f);
            Cube.Draw();
            shader.SetUniformMatrix4("model", false, model);
            // brass
            shader.SetUniform3("material.ambient", new Vector3(0.329412f, 0.223529f, 0.027451f));
            shader.SetUniform3("material.diffuse", new Vector3(0.780392f, 0.568627f, 0.113725f));
            shader.SetUniform3("material.specular", new Vector3(0.992157f, 0.941176f, 0.807843f));
            shader.SetUniform1("material.shininess", 32.0f);
            BottomScene.Draw();
            Wall_1.Draw();
            Wall_2.Draw();

            shader.SetUniform3("material.ambient", new Vector3(1.0f, 1.0f, 1.0f));
            shader.SetUniform3("material.diffuse", new Vector3(1.0f, 1.0f, 1.0f));
            shader.SetUniform3("material.specular", new Vector3(1.0f, 1.0f, 1.0f));
            shader.SetUniform1("material.shininess", 32.0f);
            shader.SetUniform3("light.ambient", new Vector3(1.0f, 1.0f, 1.0f));
            shader.SetUniform3("light.diffuse", new Vector3(1.0f, 1.0f, 1.0f));
            shader.SetUniform3("light.specular", new Vector3(1.0f, 1.0f, 1.0f));
            //Axis.Draw();
            shader.SetUniformMatrix4("model", false, light);
            Light.Draw();

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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float value = float.Parse(textBox1.Text);
                LightDirection.X = value - LightPos.X;
                Canvas.Invalidate();
            }
            catch
            {

            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float value = float.Parse(textBox2.Text);
                LightDirection.Y = value - LightPos.Y;
                Canvas.Invalidate();
            }
            catch
            {

            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float value = float.Parse(textBox3.Text);
                LightDirection.Z = value - LightPos.Z;
                Canvas.Invalidate();
            }
            catch
            {

            }
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

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float value = float.Parse(textBox4.Text);
                LightOffsetPos.X = value;
                Canvas.Invalidate();
            }
            catch
            {

            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float value = float.Parse(textBox5.Text);
                LightOffsetPos.Y = value;
                Canvas.Invalidate();
            }
            catch
            {

            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float value = float.Parse(textBox6.Text);
                LightOffsetPos.Z = value;
                Canvas.Invalidate();
            }
            catch
            {

            }
        }
    }
}
