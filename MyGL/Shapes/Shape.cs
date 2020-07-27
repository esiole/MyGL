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

        protected VertexBufferObject[] VBO { get; set; }
        protected VertexArrayObject[] VAO { get; set; }
        public Material Material { get; private set; }
        public Matrix4 Model { get; set; }

        public Shape(int CountVBO, int CountVAO, Material material, Matrix4 model)
        {
            VBO = new VertexBufferObject[CountVBO];
            VAO = new VertexArrayObject[CountVAO];
            Material = material;
            Model = model;
        }

        public abstract void Draw();

        protected void CreateBuffers(Vector3[][] Value)
        {
            for (int i = 0; i < VBO.Length; i++)
            {
                VBO[i] = new VertexBufferObject(BufferTarget.ArrayBuffer, Value[i]);
            }
            for (int i = 0; i < VAO.Length; i++)
            {
                VAO[i] = new VertexArrayObject(Value[3 * i].Length);
                VAO[i].AttachVBO(0, VBO[3 * i], 3, VertexAttribPointerType.Float, Vector3.SizeInBytes, 0);      // координаты
                VAO[i].AttachVBO(1, VBO[3 * i + 1], 3, VertexAttribPointerType.Float, Vector3.SizeInBytes, 0);  // цвета
                VAO[i].AttachVBO(2, VBO[3 * i + 2], 3, VertexAttribPointerType.Float, Vector3.SizeInBytes, 0);  // нормали
            }

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var e in VAO)
                    {
                        e.Dispose();
                    }
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
