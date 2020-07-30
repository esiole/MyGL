using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace MyGL
{
    /// <summary>
    /// Представляет собой буфер вершин в памяти GPU.
    /// </summary>
    public class VertexBufferObject : IDisposable
    {
        private bool disposedValue;

        private int Id { get; set; }

        /// <summary>
        /// Тип объекта буфера.
        /// </summary>
        public BufferTarget Type { get; private set; }

        /// <summary>
        /// Создаёт буфер вершин.
        /// </summary>
        /// <param name="data">Данные вершин.</param>
        /// <param name="type">Тип объекта буфера.</param>
        public VertexBufferObject(float[] data, BufferTarget type = BufferTarget.ArrayBuffer) : this(type)
        {
            GL.BufferData(Type, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
        }

        /// <summary>
        /// Создаёт буфер вершин.
        /// </summary>
        /// <param name="data">Данные вершин.</param>
        /// <param name="type">Тип объекта буфера.</param>
        public VertexBufferObject(Vector3[] data, BufferTarget type = BufferTarget.ArrayBuffer) : this(type)
        {
            GL.BufferData(Type, data.Length * Vector3.SizeInBytes, data, BufferUsageHint.StaticDraw);
        }

        private VertexBufferObject(BufferTarget type)
        {
            Type = type;
            Id = GL.GenBuffer();
            Use();
        }

        /// <summary>
        /// Связать буфер.
        /// </summary>
        public void Use()
        {
            GL.BindBuffer(Type, Id);
        }

        /// <summary>
        /// Освободить выделенные буфером ресурсы.
        /// </summary>
        /// <param name="disposing">Вызывается ли метод Dispose вручную.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteBuffer(Id);
                disposedValue = true;
            }
        }

        /// <summary>
        /// Финализатор, вызывающий Dispose метод.
        /// </summary>
        ~VertexBufferObject()
        {
            if (GraphicsContext.CurrentContext != null && !GraphicsContext.CurrentContext.IsDisposed)
            {
                Dispose(false);
            }
        }

        /// <summary>
        /// Освободить выделенные буфером ресурсы.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
