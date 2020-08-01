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
    public class Cylinder : Shape
    {
        private Cylinder(Vector3 centerBottom, float radius, float height, Material material, Vector3 color, Matrix4 model) 
            : base(material, color, model)
        {
            Vector3[] CylinderCoord = Drawing.Cylinder(centerBottom, radius, height);
            Vector3[] CylinderNormals = new Vector3[CylinderCoord.Length];
            for (int i = 0; i < CylinderNormals.Length; i += 2)
            {
                CylinderNormals[i] = (new Vector3(0.0f, 0.0f, -1.0f) + (CylinderCoord[i] - centerBottom)).Normalized();
                CylinderNormals[i + 1] = (new Vector3(0.0f, 0.0f, 1.0f) + (CylinderCoord[i + 1] - (centerBottom + new Vector3(0.0f, 0.0f, height)))).Normalized();

            }
            AddVertexGroup(new VertexArrayInfo(CylinderCoord, CylinderNormals, PrimitiveType.TriangleStrip));
            Vector3[] BottomCoord = Drawing.Disc(centerBottom, radius);
            Vector3[] BottomNormals = new Vector3[BottomCoord.Length];
            for (int i = 0; i < BottomNormals.Length; i++)
            {
                BottomNormals[i] = new Vector3(0.0f, 0.0f, -1.0f);
            }
            AddVertexGroup(new VertexArrayInfo(BottomCoord, BottomNormals, PrimitiveType.TriangleFan));
            Vector3[] HeadCoord = Drawing.Disc(centerBottom + new Vector3(0.0f, 0.0f, height), radius);
            Vector3[] HeadNormals = new Vector3[HeadCoord.Length];
            for (int i = 0; i < HeadNormals.Length; i++)
            {
                HeadNormals[i] = new Vector3(0.0f, 0.0f, 1.0f);
            }
            AddVertexGroup(new VertexArrayInfo(HeadCoord, HeadNormals, PrimitiveType.TriangleFan));
        }

        public Cylinder(Vector3 centerBottom, float radius, float height, Material material, Matrix4 model)
            : this(centerBottom, radius, height, material, Vector3.Zero, model) { }

        public Cylinder(Vector3 centerBottom, float radius, float height, Vector3 color, Matrix4 model)
            : this(centerBottom, radius, height, Material.LightSource, color, model) { }
    }
}
