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

        public List<LightSource> Lights { get; private set; }

        //public Vector3 Test = new Vector3();
        //public event Action<float> XChange;
        //public void Event(float a)
        //{
        //    Test.X = a;
        //    if (XChange != null) XChange(a);
        //}

        public Scene(Shader shader/*, Matrix4 view, Matrix4 projection*/)
        {
            Shapes = new List<Shape>();
            Shader = shader;

            Lights = new List<LightSource>();
            //View = view;
            //Projection = projection;
        }

        public void Add(Shape shape)
        {
            //Shader.Use();
            Shapes.Add(shape);
            //if (shape.IsPointLight) Shader.SetPointLight(shape);
            //if (shape.IsSpotLight) Shader.SetSpotLight(shape);
        }

        public void Add(LightSource light)
        {
            Shader.Use();
            Lights.Add(light);
            if (light is PointLight) Shader.SetPointLight(light as PointLight);
            if (light is SpotLight) Shader.SetSpotLight(light as SpotLight);
        }

        public void Draw()
        {
            Shader.Use();
            Shader.SetViewMatrix(View);
            Shader.SetProjectionMatrix(Projection);
            Shader.SetCameraPos(CameraPos);


            //foreach (var e in Shapes.Where(e => e.IsPointLight))
            //{
            //    Shader.SetPointLight(e);
            //}
            //foreach (var e in Shapes.Where(e => e.IsSpotLight))
            //{
            //    Shader.SetSpotLight(e);
            //}
            Shader.SetDirLight();




            foreach (var e in Shapes.Where(e => !e.IsPointLight && !e.IsSpotLight))
            {
                Shader.SetModelMatrix(e.Model);
                Shader.SetMaterial(e.Material);
                e.Draw();
            }
            foreach (var e in Shapes.Where(e => e.IsPointLight || e.IsSpotLight))
            {
                Shader.SetModelMatrix(e.Model);
                Shader.SetMaterial(e.Material);

                Shader.SetUniform3("light.ambient", new Vector3(1.0f, 1.0f, 1.0f));
                Shader.SetUniform3("light.diffuse", new Vector3(1.0f, 1.0f, 1.0f));
                Shader.SetUniform3("light.specular", new Vector3(1.0f, 1.0f, 1.0f));

                e.Draw();
            }
        }
    }
}
