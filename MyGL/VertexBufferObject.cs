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
    public class VertexBufferObject : IDisposable
    {
        private bool disposedValue = false;
        public int Handle { get; private set; }
        public BufferTarget Type { get; private set; }

        public VertexBufferObject(BufferTarget type, float[] data)
        {
            Type = type;
            Handle = GL.GenBuffer();
            Use();
            GL.BufferData(Type, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
        }

        public VertexBufferObject(BufferTarget type, Vector3[] data)
        {
            Type = type;
            Handle = GL.GenBuffer();
            Use();
            GL.BufferData(Type, data.Length * Vector3.SizeInBytes, data, BufferUsageHint.StaticDraw);
        }

        public void Use()
        {
            GL.BindBuffer(Type, Handle);
        }

        private void ReleaseHandle()
        {
            if (!disposedValue)
            {
                GL.DeleteBuffer(Handle);
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            ReleaseHandle();
            GC.SuppressFinalize(this);
        }

        ~VertexBufferObject()
        {
            if (GraphicsContext.CurrentContext != null && !GraphicsContext.CurrentContext.IsDisposed)
            {
                ReleaseHandle();
            }
        }
    }
}
