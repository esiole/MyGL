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
        public Vector3 normall;
        public Sheet(Vector3 LeftBottom, Vector3 RightBottom, Vector3 LeftUp, Vector3 RightUp, Material material, Matrix4 model) : base(material, model)
        {
            Vector3[] Coord = { LeftBottom, RightBottom, LeftUp, RightUp };

            Vector3 first = RightUp - LeftBottom;
            Vector3 second = LeftUp - RightBottom;
            Vector3 normal = new Vector3(first.Y * second.Z - second.Y * first.Z, second.X * first.Z - first.X * second.Z, first.X * second.Y - second.X * first.Y).Normalized();
            normall = normal;

            Vector3[] Normals = { normal, normal, normal, normal };

            AddVertexGroup(new VertexArrayInfo(Coord, Normals));
        }

        public override void Draw()
        {
            VertexGroups[0].VAO.Draw(PrimitiveType.TriangleStrip);
        }
    }
}
