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
    public class Cube : Shape
    {
        private Cube(Vector3 center, float side, Material material, Vector3 color, Matrix4 model)
            : base(material, color, model)
        {
            Vector3[] CubeCoord =
            {
                new Vector3(center.X - side/2, center.Y - side/2, center.Z - side/2), new Vector3(center.X + side/2, center.Y - side/2, center.Z - side/2),
                new Vector3(center.X + side/2, center.Y + side/2, center.Z - side/2), new Vector3(center.X - side/2, center.Y + side/2, center.Z - side/2),

                new Vector3(center.X - side/2, center.Y - side/2, center.Z - side/2), new Vector3(center.X + side/2, center.Y - side/2, center.Z - side/2),
                new Vector3(center.X + side/2, center.Y - side/2, center.Z + side/2), new Vector3(center.X - side/2, center.Y - side/2, center.Z + side/2),

                new Vector3(center.X - side/2, center.Y - side/2, center.Z - side/2), new Vector3(center.X - side/2, center.Y + side/2, center.Z - side/2),
                new Vector3(center.X - side/2, center.Y + side/2, center.Z + side/2), new Vector3(center.X - side/2, center.Y - side/2, center.Z + side/2),

                new Vector3(center.X + side/2, center.Y + side/2, center.Z + side/2), new Vector3(center.X - side/2, center.Y + side/2, center.Z + side/2),
                new Vector3(center.X - side/2, center.Y - side/2, center.Z + side/2), new Vector3(center.X + side/2, center.Y - side/2, center.Z + side/2),

                new Vector3(center.X + side/2, center.Y + side/2, center.Z + side/2), new Vector3(center.X - side/2, center.Y + side/2, center.Z + side/2),
                new Vector3(center.X - side/2, center.Y + side/2, center.Z - side/2), new Vector3(center.X + side/2, center.Y + side/2, center.Z - side/2),

                new Vector3(center.X + side/2, center.Y + side/2, center.Z + side/2), new Vector3(center.X + side/2, center.Y - side/2, center.Z + side/2),
                new Vector3(center.X + side/2, center.Y - side/2, center.Z - side/2), new Vector3(center.X + side/2, center.Y + side/2, center.Z - side/2)
            };
            Vector3[] CubeNormals = new Vector3[CubeCoord.Length];
            for (int i = 0; i < CubeNormals.Length; i++)
            {
                CubeNormals[i] = (CubeCoord[i] - center).Normalized();
            }
            AddVertexGroup(new VertexArrayInfo(CubeCoord, CubeNormals, PrimitiveType.Quads));
        }

        public Cube(Vector3 center, float side, Material material, Matrix4 model)
            : this(center, side, material, Vector3.Zero, model) { }

        public Cube(Vector3 center, float side, Vector3 color, Matrix4 model)
            : this(center, side, Material.LightSource, color, model) { }
    }
}
