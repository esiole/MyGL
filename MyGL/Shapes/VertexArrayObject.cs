using System;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace MyGL
{
    /// <summary>
    /// Представляет собой массив атрибутов вершин в памяти GPU.
    /// </summary>
    public class VertexArrayObject : IDisposable
    {
        private bool disposedValue;

        private int Id { get; set; }

        /// <summary>
        /// Количество вершин.
        /// </summary>
        public int VertexCount { get; private set; }

        /// <summary>
        /// Создаёт массив атрибутов вершин.
        /// </summary>
        /// <param name="CountVertex">Количество вершин.</param>
        public VertexArrayObject(int CountVertex)
        {
            VertexCount = CountVertex;
            Id = GL.GenVertexArray();
            Use();
        }

        /// <summary>
        /// Делает массив атрибутов вершин текущим.
        /// </summary>
        public void Use()
        {
            GL.BindVertexArray(Id);
        }

        /// <summary>
        /// Связать массив атрибутов вершин с буфером вершин.
        /// </summary>
        /// <param name="index">Индекс атрибута в шейдере.</param>
        /// <param name="vbo">Буфер вершин.</param>
        /// <param name="elementsPerVertex">Количество элементов, относящихся к одной вершине.</param>
        /// <param name="pointerType">Указатель на тип данных.</param>
        /// <param name="stride">Сколько байт приходится на одну вершину</param>
        /// <param name="offset">Смещение между значениями разных вершин. Применяется, когда в одном VBO несколько разных атрибутов для вершины.</param>
        public void AttachVBO(int index, VertexBufferObject vbo, int elementsPerVertex, VertexAttribPointerType pointerType, int stride, int offset)
        {
            Use();
            vbo.Use();
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, elementsPerVertex, pointerType, false, stride, offset);
        }

        /// <summary>
        /// Отрисовать массив вершин.
        /// </summary>
        /// <param name="type">Какой примитив использовать при отрисовке.</param>
        public void Draw(PrimitiveType type)
        {
            Use();
            GL.DrawArrays(type, 0, VertexCount);
        }

        /// <summary>
        /// Освободить выделенные буфером ресурсы.
        /// </summary>
        /// <param name="disposing">Вызывается ли метод Dispose вручную.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteVertexArray(Id);
                disposedValue = true;
            }
        }

        /// <summary>
        /// Финализатор, вызывающий Dispose метод.
        /// </summary>
        ~VertexArrayObject()
        {
            if (GraphicsContext.CurrentContext != null && !GraphicsContext.CurrentContext.IsDisposed)
            {
                Dispose(false);
            }
        }

        /// <summary>
        /// Освободить выделенные массивом ресурсы.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
