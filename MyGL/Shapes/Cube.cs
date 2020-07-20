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
        public Cube(Vector3 Center, float Side, Vector3 Color, Material material, Matrix4 model) : base(3, 1, material, model)
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
            Vector3[] CubeColor = new Vector3[CubeCoord.Length];
            for (int i = 0; i < CubeColor.Length; i++)
            {
                CubeColor[i] = Color;
            }
            Vector3[] CubeNormals = new Vector3[CubeCoord.Length];
            for (int i = 0; i < CubeNormals.Length; i++)
            {
                CubeNormals[i] = (CubeCoord[i] - Center).Normalized();
            }

            CreateBuffers(new Vector3[][] { CubeCoord, CubeColor, CubeNormals });
        }

        public override void Draw()
        {
            foreach (VertexArrayObject element in VAO)
            {
                element.Draw(PrimitiveType.Quads);
            }
        }
    }
}
