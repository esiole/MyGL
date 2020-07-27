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
        public Matrix4 View { get; set; }
        public Matrix4 Projection { get; set; }
        public Vector3 CameraPos { get; set; }
        public List<LightSource> Lights { get; private set; }
        public DirectionLight DirectionLight { get; set; }
        public ShaderController Controller { get; private set; }

        public Scene(IShaderSource shaderSource)
        {
            Shapes = new List<Shape>();
            Lights = new List<LightSource>();
            DirectionLight = new DirectionLight();
            Controller = new ShaderController(shaderSource);
        }

        public void Add(Shape shape)
        {
            Shapes.Add(shape);
        }

        public void Add(LightSource light)
        {
            Lights.Add(light);
            if (light is PointLight) Controller.AddPointLight(light as PointLight);
            if (light is SpotLight) Controller.AddSpotLight(light as SpotLight);
        }

        public void Draw()
        {
            Controller.PrepareDraw(View, Projection, CameraPos);
            Controller.EnableDirLight(DirectionLight);
            DrawElements(Shapes);
            Controller.DisableDirLight();
            DrawElements(Lights.Select(light => light.Source));
        }

        private void DrawElements(IEnumerable<Shape> elements)
        {
            foreach (var e in elements)
            {
                Controller.SetModelMatrix(e.Model);
                Controller.SetMaterial(e.Material);
                e.Draw();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Controller.Dispose();
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
