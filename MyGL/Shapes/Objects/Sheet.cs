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
    public class Sheet : Shape
    {
        private Sheet(Vector3 leftBottom, Vector3 rightBottom, Vector3 leftUp, Vector3 rightUp, Material material, Vector3 color, Matrix4 model) 
            : base(material, color, model)
        {
            Vector3[] Coord = { leftBottom, rightBottom, leftUp, rightUp };
            Vector3 first = rightUp - leftBottom;
            Vector3 second = leftUp - rightBottom;
            Vector3 normal = new Vector3(first.Y * second.Z - second.Y * first.Z, second.X * first.Z - first.X * second.Z, first.X * second.Y - second.X * first.Y).Normalized();
            Vector3[] Normals = { normal, normal, normal, normal };
            AddVertexGroup(new VertexArrayInfo(Coord, Normals, PrimitiveType.TriangleStrip));
        }

        public Sheet(Vector3 leftBottom, Vector3 rightBottom, Vector3 leftUp, Vector3 rightUp, Material material, Matrix4 model)
            : this(leftBottom, rightBottom, leftUp, rightUp, material, Vector3.Zero, model) { }

        public Sheet(Vector3 leftBottom, Vector3 rightBottom, Vector3 leftUp, Vector3 rightUp, Vector3 color, Matrix4 model)
            : this(leftBottom, rightBottom, leftUp, rightUp, Material.LightSource, color, model) { }
    }
}
