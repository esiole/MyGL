using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace MyGL
{
    public abstract class Shape : IDisposable
    {
        private bool disposedValue;

        protected List<VertexGroup> VertexGroups { get; set; }
        public Material Material { get; set; }
        public Vector3 Color { get; set; }
        public Matrix4 Model { get; set; }

        public Shape(Material material, Vector3 color, Matrix4 model)
        {
            VertexGroups = new List<VertexGroup>();
            Material = material;
            Color = color;
            Model = model;
        }

        public virtual void Draw()
        {
            foreach (var e in VertexGroups)
            {
                e.Draw();
            }
        }

        protected void AddVertexGroup(VertexArrayInfo info)
        {
            VertexGroups.Add(new VertexGroup(info));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var e in VertexGroups)
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
