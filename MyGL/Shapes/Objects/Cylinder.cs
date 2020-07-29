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
        public Cylinder(Vector3 CenterBottom, float Radius, float Height, Material material, Matrix4 model) : base(material, model)
        {
            Vector3[] CylinderCoord = Drawing.Cylinder(CenterBottom, Radius, Height);
            Vector3[] CylinderNormals = new Vector3[CylinderCoord.Length];
            for (int i = 0; i < CylinderNormals.Length; i += 2)
            {
                CylinderNormals[i] = (new Vector3(0.0f, 0.0f, -1.0f) + (CylinderCoord[i] - CenterBottom)).Normalized();
                CylinderNormals[i + 1] = (new Vector3(0.0f, 0.0f, 1.0f) + (CylinderCoord[i + 1] - (CenterBottom + new Vector3(0.0f, 0.0f, Height)))).Normalized();

            }
            AddVertexGroup(new VertexArrayInfo(CylinderCoord, CylinderNormals, PrimitiveType.TriangleStrip));
            Vector3[] BottomCoord = Drawing.Disc(CenterBottom, Radius);
            Vector3[] BottomNormals = new Vector3[BottomCoord.Length];
            for (int i = 0; i < BottomNormals.Length; i++)
            {
                BottomNormals[i] = new Vector3(0.0f, 0.0f, -1.0f);
            }
            AddVertexGroup(new VertexArrayInfo(BottomCoord, BottomNormals, PrimitiveType.TriangleFan));
            Vector3[] HeadCoord = Drawing.Disc(CenterBottom + new Vector3(0.0f, 0.0f, Height), Radius);
            Vector3[] HeadNormals = new Vector3[HeadCoord.Length];
            for (int i = 0; i < HeadNormals.Length; i++)
            {
                HeadNormals[i] = new Vector3(0.0f, 0.0f, 1.0f);
            }
            AddVertexGroup(new VertexArrayInfo(HeadCoord, HeadNormals, PrimitiveType.TriangleFan));
        }
    }
}
