using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MyGL
{
    /// <summary>
    /// Представляет собой массив вершин для отрисовки.
    /// </summary>
    public class VertexGroup : IDisposable
    {
        private bool disposedValue;

        private VertexArrayObject VAO { get; set; }
        private List<VertexBufferObject> VBO { get; set; }

        /// <summary>
        /// Примитив для отрисовки массива вершин.
        /// </summary>
        public PrimitiveType DrawType { get; private set; }

        /// <summary>
        /// Создаёт массив вершин для отрисовки.
        /// </summary>
        /// <param name="info">Атрибуты вершин.</param>
        public VertexGroup(VertexArrayInfo info)
        {
            // пройтись по всем свойствам объекта; рефлексия?
            VAO = new VertexArrayObject(info.Count);
            VBO = new List<VertexBufferObject>();
            int i = 0;
            foreach (var e in info.Properties)
            {
                VBO.Add(new VertexBufferObject(e));
                VAO.AttachVBO(i, VBO[i], 3, VertexAttribPointerType.Float, Vector3.SizeInBytes, 0);
                i++;
            }
            DrawType = info.DrawType;
        }

        /// <summary>
        /// Отрисовать массив вершин.
        /// </summary>
        public void Draw()
        {
            VAO.Draw(DrawType);
        }

        /// <summary>
        /// Освободить выделенные ресурсы.
        /// </summary>
        /// <param name="disposing">Вызывается ли метод Dispose вручную.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    VAO.Dispose();
                    foreach (var e in VBO)
                    {
                        e.Dispose();
                    }
                }
                disposedValue = true;
            }
        }

        /// <summary>
        /// Освободить выделенные ресурсы.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
