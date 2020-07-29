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
        private bool disposedValue;

        public int Handle { get; private set; }
        public BufferTarget Type { get; private set; }

        public VertexBufferObject(BufferTarget type, float[] data)
        {
            Type = type;
            Handle = GL.GenBuffer();
            Use();
            GL.BufferData(Type, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
        }

        public VertexBufferObject(Vector3[] data, BufferTarget type = BufferTarget.ArrayBuffer)
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteBuffer(Handle);
                disposedValue = true;
            }
        }

        ~VertexBufferObject()
        {
            //if (GraphicsContext.CurrentContext != null && !GraphicsContext.CurrentContext.IsDisposed)
            //{
                Dispose(false);
            //}
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
