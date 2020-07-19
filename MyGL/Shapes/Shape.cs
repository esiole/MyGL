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
    public abstract class Shape
    {
        protected VertexBufferObject[] VBO { get; set; }
        protected VertexArrayObject[] VAO { get; set; }

        public Shape(int CountVBO, int CountVAO)
        {
            VBO = new VertexBufferObject[CountVBO];
            VAO = new VertexArrayObject[CountVAO];
        }

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

        protected void Dispose()
        {
            foreach (VertexArrayObject element in VAO)
            {
                element.Dispose();
            }
            foreach (VertexBufferObject element in VBO)
            {
                element.Dispose();
            }
        }
    }
}
