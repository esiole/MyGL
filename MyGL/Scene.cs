using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL
{
    public class Scene
    {
        public List<Shape> Shapes { get; private set; }
        public Shader Shader { get; private set; }
        public Matrix4 View { get; set; }
        public Matrix4 Projection { get; set; }
        public Vector3 CameraPos { get; set; }

        public Vector3 Test = new Vector3();
        public event Action<float> XChange;
        public void Event(float a)
        {
            Test.X = a;
            if (XChange != null) XChange(a);
        }

        public Scene(Shader shader/*, Matrix4 view, Matrix4 projection*/)
        {
            Shapes = new List<Shape>();
            Shader = shader;
            //View = view;
            //Projection = projection;
        }

        public void Add(Shape shape)
        {
            Shapes.Add(shape);
        }

        public void Draw()
        {
            Shader.Use();
            Shader.SetViewMatrix(View);
            Shader.SetProjectionMatrix(Projection);
            Shader.SetCameraPos(CameraPos);
            foreach (var e in Shapes)
            {
                Shader.SetModelMatrix(e.Model);
                Shader.SetMaterial(e.Material);
                e.Draw();
            }
        }
    }
}
