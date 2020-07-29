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
    public class VertexGroup : IDisposable
    {
        private bool disposedValue;

        public VertexArrayObject VAO { get; private set; }
        private List<VertexBufferObject> VBO { get; set; }

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
            //for (int i = 0; i < VAO.Length; i++)
            //{
            //    VAO[i] = new VertexArrayObject(value[3 * i].Length);
            //    VAO[i].AttachVBO(0, VBO[3 * i], 3, VertexAttribPointerType.Float, Vector3.SizeInBytes, 0);      // координаты
            //    VAO[i].AttachVBO(1, VBO[3 * i + 1], 3, VertexAttribPointerType.Float, Vector3.SizeInBytes, 0);  // цвета
            //    VAO[i].AttachVBO(2, VBO[3 * i + 2], 3, VertexAttribPointerType.Float, Vector3.SizeInBytes, 0);  // нормали
            //}
        }

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
