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
        public Cube(Vector3 Center, float Side, Vector3 Color, Material material, Matrix4 model) : base(material, model)
        {
            Vector3[] CubeCoord =
            {
                new Vector3(Center.X - Side/2, Center.Y - Side/2, Center.Z - Side/2), new Vector3(Center.X + Side/2, Center.Y - Side/2, Center.Z - Side/2),
                new Vector3(Center.X + Side/2, Center.Y + Side/2, Center.Z - Side/2), new Vector3(Center.X - Side/2, Center.Y + Side/2, Center.Z - Side/2),

                new Vector3(Center.X - Side/2, Center.Y - Side/2, Center.Z - Side/2), new Vector3(Center.X + Side/2, Center.Y - Side/2, Center.Z - Side/2),
                new Vector3(Center.X + Side/2, Center.Y - Side/2, Center.Z + Side/2), new Vector3(Center.X - Side/2, Center.Y - Side/2, Center.Z + Side/2),

                new Vector3(Center.X - Side/2, Center.Y - Side/2, Center.Z - Side/2), new Vector3(Center.X - Side/2, Center.Y + Side/2, Center.Z - Side/2),
                new Vector3(Center.X - Side/2, Center.Y + Side/2, Center.Z + Side/2), new Vector3(Center.X - Side/2, Center.Y - Side/2, Center.Z + Side/2),

                new Vector3(Center.X + Side/2, Center.Y + Side/2, Center.Z + Side/2), new Vector3(Center.X - Side/2, Center.Y + Side/2, Center.Z + Side/2),
                new Vector3(Center.X - Side/2, Center.Y - Side/2, Center.Z + Side/2), new Vector3(Center.X + Side/2, Center.Y - Side/2, Center.Z + Side/2),

                new Vector3(Center.X + Side/2, Center.Y + Side/2, Center.Z + Side/2), new Vector3(Center.X - Side/2, Center.Y + Side/2, Center.Z + Side/2),
                new Vector3(Center.X - Side/2, Center.Y + Side/2, Center.Z - Side/2), new Vector3(Center.X + Side/2, Center.Y + Side/2, Center.Z - Side/2),

                new Vector3(Center.X + Side/2, Center.Y + Side/2, Center.Z + Side/2), new Vector3(Center.X + Side/2, Center.Y - Side/2, Center.Z + Side/2),
                new Vector3(Center.X + Side/2, Center.Y - Side/2, Center.Z - Side/2), new Vector3(Center.X + Side/2, Center.Y + Side/2, Center.Z - Side/2)
            };
            Vector3[] CubeNormals = new Vector3[CubeCoord.Length];
            for (int i = 0; i < CubeNormals.Length; i++)
            {
                CubeNormals[i] = (CubeCoord[i] - Center).Normalized();
            }

            AddVertexGroup(new VertexArrayInfo(CubeCoord, CubeNormals));
        }

        public override void Draw()
        {
            foreach (var e in VertexGroups.Select(group => group.VAO))
            {
                e.Draw(PrimitiveType.Quads);
            }
        }
    }
}
