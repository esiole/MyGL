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

        public Scene(Shader shader)
        {
            Shapes = new List<Shape>();
            Shader = shader;
            Lights = new List<LightSource>();
        }

        public void Add(Shape shape)
        {
            Shapes.Add(shape);
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

            Shader.SetDirLight();

            foreach (var e in Shapes.Where(e => !e.IsPointLight && !e.IsSpotLight))
            {
                Shader.SetModelMatrix(e.Model);
                Shader.SetMaterial(e.Material);
                e.Draw();
            }
            Shader.SetUniform3("dirLight.ambient", new Vector3(1.0f, 1.0f, 1.0f));
            Shader.SetUniform3("dirLight.diffuse", new Vector3(1.0f, 1.0f, 1.0f));
            Shader.SetUniform3("dirLight.specular", new Vector3(1.0f, 1.0f, 1.0f));
            foreach (var e in Lights)
            {
                Shader.SetModelMatrix(e.Source.Model);
                Shader.SetMaterial(e.Source.Material);
                e.Source.Draw();
            }
        }
    }
}
