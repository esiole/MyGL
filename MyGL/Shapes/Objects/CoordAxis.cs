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
    public class CoordAxis : Shape
    {
        public CoordAxis(float Length, Material material, Matrix4 model) : base(material, model)
        {
            Vector3[] AxisCoord =
            {
                new Vector3(-Length, 0.0f, 0.0f), new Vector3(Length, 0.0f, 0.0f),
                new Vector3(0.0f, -Length, 0.0f), new Vector3(0.0f, Length, 0.0f),
                new Vector3(0.0f, 0.0f, -Length), new Vector3(0.0f, 0.0f, Length),
                new Vector3(Length, 0.0f, 0.0f), new Vector3(0.98f, 0.02f, 0.0f), new Vector3(Length, 0.0f, 0.0f), new Vector3(0.98f, -0.02f, 0.0f),
                new Vector3(0.0f, Length, 0.0f), new Vector3(0.02f, 0.98f, 0.0f), new Vector3(0.0f, Length, 0.0f), new Vector3(-0.02f, 0.98f, 0.0f),
                new Vector3(0.0f, 0.0f, Length), new Vector3(0.0f, 0.02f, 0.98f), new Vector3(0.0f, 0.0f, Length), new Vector3(0.0f, -0.02f, 0.98f)
            };
            Vector3[] AxisNormal = new Vector3[AxisCoord.Length];
            for (int i = 0; i < AxisNormal.Length; i++)
            {
                AxisNormal[i] = new Vector3(0.0f, 0.0f, 1.0f);
            }
            AddVertexGroup(new VertexArrayInfo(AxisCoord, AxisNormal));
        }

        public override void Draw()
        {
            //foreach (VertexArrayObject element in VAO)
            //{
            //    element.Draw(PrimitiveType.Lines);
            //}
            foreach (var e in VertexGroups.Select(group => group.VAO))
            {
                e.Draw(PrimitiveType.Lines);
            }
        }
    }
}