﻿using System;
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
    public class VertexArrayObject : IDisposable
    {
        private bool disposedValue;

        public int Handle { get; private set; }
        public int VertexCount { get; private set; }

        public VertexArrayObject(int CountVertex)
        {
            VertexCount = CountVertex;
            Handle = GL.GenVertexArray();
            Use();
        }

        public void Use()
        {
            GL.BindVertexArray(Handle);
        }

        public void AttachVBO(int index, VertexBufferObject vbo, int elementsPerVertex, VertexAttribPointerType pointerType, int stride, int offset)
        {
            Use();
            vbo.Use();
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, elementsPerVertex, pointerType, false, stride, offset);
        }

        public void Draw(PrimitiveType type)
        {
            Use();
            GL.DrawArrays(type, 0, VertexCount);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteVertexArray(Handle);
                disposedValue = true;
            }
        }

        ~VertexArrayObject()
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