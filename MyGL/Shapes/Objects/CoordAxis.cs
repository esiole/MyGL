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
        private CoordAxis(float length, Material material, Vector3 color, Matrix4 model) 
            : base(material, color, model)
        {
            Vector3[] AxisCoord =
            {
                new Vector3(-length, 0.0f, 0.0f), new Vector3(length, 0.0f, 0.0f),
                new Vector3(0.0f, -length, 0.0f), new Vector3(0.0f, length, 0.0f),
                new Vector3(0.0f, 0.0f, -length), new Vector3(0.0f, 0.0f, length),
                new Vector3(length, 0.0f, 0.0f), new Vector3(0.98f, 0.02f, 0.0f), new Vector3(length, 0.0f, 0.0f), new Vector3(0.98f, -0.02f, 0.0f),
                new Vector3(0.0f, length, 0.0f), new Vector3(0.02f, 0.98f, 0.0f), new Vector3(0.0f, length, 0.0f), new Vector3(-0.02f, 0.98f, 0.0f),
                new Vector3(0.0f, 0.0f, length), new Vector3(0.0f, 0.02f, 0.98f), new Vector3(0.0f, 0.0f, length), new Vector3(0.0f, -0.02f, 0.98f)
            };
            Vector3[] AxisNormal = new Vector3[AxisCoord.Length];
            for (int i = 0; i < AxisNormal.Length; i++)
            {
                AxisNormal[i] = new Vector3(0.0f, 0.0f, 1.0f);
            }
            AddVertexGroup(new VertexArrayInfo(AxisCoord, AxisNormal, PrimitiveType.Lines));
        }

        public CoordAxis(float length, Material material, Matrix4 model) 
            : this(length, material, Vector3.Zero, model) { }

        public CoordAxis(float length, Vector3 color, Matrix4 model)
            : this(length, Material.LightSource, color, model) { }
    }
}