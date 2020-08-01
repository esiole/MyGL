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
    public class Cone : Shape
    {
        private Cone(Vector3 centerBottom, float radius, float height, Material material, Vector3 color, Matrix4 model) 
            : base(material, color, model)
        {
            Vector3[] ConeCoord = Drawing.Cone(centerBottom, radius, height);
            Vector3[] ConeNormals = new Vector3[ConeCoord.Length];
            ConeNormals[0] = new Vector3(0.0f, 0.0f, 1.0f);
            for (int i = 1; i < ConeNormals.Length; i++)
            {
                ConeNormals[i] = (ConeCoord[i] - centerBottom).Normalized();
            }
            AddVertexGroup(new VertexArrayInfo(ConeCoord, ConeNormals, PrimitiveType.TriangleFan));
            Vector3[] ConeBottomCoord = Drawing.Disc(new Vector3(0.0f, 0.0f, 0.0f), radius);
            Vector3[] ConeBottomNormals = new Vector3[ConeBottomCoord.Length];
            for (int i = 0; i < ConeBottomNormals.Length; i++)
            {
                ConeBottomNormals[i] = new Vector3(0.0f, 0.0f, -1.0f);
            }
            AddVertexGroup(new VertexArrayInfo(ConeBottomCoord, ConeBottomNormals, PrimitiveType.TriangleFan));
        }

        public Cone(Vector3 centerBottom, float radius, float height, Material material, Matrix4 model) 
            : this(centerBottom, radius, height, material, Vector3.Zero, model) { }

        public Cone(Vector3 centerBottom, float radius, float height, Vector3 color, Matrix4 model)
            : this(centerBottom, radius, height, Material.LightSource, color, model) { }
    }
}
