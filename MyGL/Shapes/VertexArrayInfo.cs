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
    public class VertexArrayInfo
    {
        public int Count { get; }
        public Vector3[] Coord { get; private set; }
        public Vector3[] Normals { get; private set; }

        public VertexArrayInfo(Vector3[] coord, Vector3[] normals)
        {
            if (coord.Length != normals.Length)
            {
                throw new ArgumentException("Массивы характеристик вершин должны быть одного размера.");
            }
            Count = coord.Length;
            Coord = coord;
            Normals = normals;
        }

        public IEnumerable<Vector3[]> Properties
        {
            get
            {
                yield return Coord;
                yield return Normals;
            }
        }

        //public IEnumerable<Vector3[]> Properties()
        //{
        //    yield return Coord;
        //    yield return Normals;
        //}
    }
}
