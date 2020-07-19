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
        public Cylinder(Vector3 CenterBottom, float Radius, float Height) : base(9, 3)
        {
            Vector3[] CylinderCoord = Drawing.Cylinder(CenterBottom, Radius, Height);
            Vector3[] CylinderColor = new Vector3[CylinderCoord.Length];
            for (int i = 0; i < CylinderColor.Length; i++)
            {
                CylinderColor[i] = new Vector3(0.2f, 0.5f, 0.8f);
            }
            Vector3[] CylinderNormals = new Vector3[CylinderCoord.Length];
            for (int i = 0; i < CylinderNormals.Length; i += 2)
            {
                CylinderNormals[i] = (new Vector3(0.0f, 0.0f, -1.0f) + (CylinderCoord[i] - CenterBottom)).Normalized();
                CylinderNormals[i + 1] = (new Vector3(0.0f, 0.0f, 1.0f) + (CylinderCoord[i + 1] - (CenterBottom + new Vector3(0.0f, 0.0f, Height)))).Normalized();

            }
            Vector3[] BottomCoord = Drawing.Disc(CenterBottom, Radius);
            Vector3[] BottomColor = new Vector3[BottomCoord.Length];
            for (int i = 0; i < BottomColor.Length; i++)
            {
                BottomColor[i] = new Vector3(0.2f, 0.5f, 0.8f);
            }
            Vector3[] BottomNormals = new Vector3[BottomCoord.Length];
            for (int i = 0; i < BottomNormals.Length; i++)
            {
                BottomNormals[i] = new Vector3(0.0f, 0.0f, -1.0f);
            }
            Vector3[] HeadCoord = Drawing.Disc(CenterBottom + new Vector3(0.0f, 0.0f, Height), Radius);
            Vector3[] HeadColor = new Vector3[HeadCoord.Length];
            for (int i = 0; i < HeadColor.Length; i++)
            {
                HeadColor[i] = new Vector3(0.2f, 0.5f, 0.8f);
            }
            Vector3[] HeadNormals = new Vector3[HeadCoord.Length];
            for (int i = 0; i < HeadNormals.Length; i++)
            {
                HeadNormals[i] = new Vector3(0.0f, 0.0f, 1.0f);
            }

            CreateBuffers(new Vector3[][] { CylinderCoord, CylinderColor, CylinderNormals, BottomCoord, BottomColor, BottomNormals, HeadCoord, HeadColor, HeadNormals });
        }

        public void Draw()
        {
            VAO[0].Draw(PrimitiveType.TriangleStrip);
            VAO[1].Draw(PrimitiveType.TriangleFan);
            VAO[2].Draw(PrimitiveType.TriangleFan);
        }
    }
}
