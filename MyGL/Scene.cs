using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL
{
    public class Scene : IDisposable
    {
        private bool disposedValue;

        public List<Shape> Shapes { get; set; }
        public Shader Shader { get; private set; }
        public Matrix4 View { get; set; }
        public Matrix4 Projection { get; set; }
        public Vector3 CameraPos { get; set; }
        public List<LightSource> Lights { get; private set; }
        public DirectionLight DirectionLight { get; set; }

        public Scene(IShaderSource shaderSource)
        {
            Shapes = new List<Shape>();
            Shader = new Shader(shaderSource);
            Lights = new List<LightSource>();
            DirectionLight = new DirectionLight();
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

            Shader.SetDirLight(DirectionLight);

            foreach (var e in Shapes)
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Shader.Dispose();
                    foreach (var e in Shapes)
                    {
                        e.Dispose();
                    }
                    foreach (var e in Lights)
                    {
                        e.Dispose();
                    }    
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
