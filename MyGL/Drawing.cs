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
    static class Drawing
    {
        // рисовать TRIANGLE_FAN
        static public Vector3[] Disc(Vector3 Center, float Radius, int DeltaAngle = 10)
        {
            List<Vector3> coord = new List<Vector3>();
            coord.Add(Center);
            for (int angle = 0; angle <= 360; angle += DeltaAngle)
            {
                float dx = (float)(Radius * Math.Cos(angle * Math.PI / 180));
                float dy = (float)(Radius * Math.Sin(angle * Math.PI / 180));
                coord.Add(Center + new Vector3(dx, dy, 0));
            }
            return coord.ToArray();
        }

        // рисовать TRIANGLE_FAN
        static public Vector3[] Cone(Vector3 CenterBottom, float Radius, float Height, int DeltaAngle = 10)
        {
            List<Vector3> coord = new List<Vector3>();
            coord.Add(CenterBottom + new Vector3(0, 0, Height));
            for (int angle = 0; angle <= 360; angle += DeltaAngle)
            {
                float dx = (float)(Radius * Math.Cos(angle * Math.PI / 180));
                float dy = (float)(Radius * Math.Sin(angle * Math.PI / 180));
                coord.Add(CenterBottom + new Vector3(dx, dy, 0));
            }
            return coord.ToArray();
        }

        // рисовать LINE_LOOP
        static public Vector3[] Circle(Vector3 Center, float Radius, int amountSegments = 50)
        {
            List<Vector3> coord = new List<Vector3>();
            for (int i = 0; i < amountSegments; i++)
            {
                double angle = 2.0 * Math.PI * i / (double)amountSegments;
                float dx = (float)(Radius * Math.Cos(angle));
                float dy = (float)(Radius * Math.Sin(angle));
                coord.Add(Center + new Vector3(dx, dy, 0));
            }
            return coord.ToArray();
        }

        // рисовать QUAD_STRIP
        static public Vector3[] Cylinder(Vector3 CenterBottom, float Radius, float Height, int amountSegments = 50)
        {
            List<Vector3> coord = new List<Vector3>();
            for (int i = 0; i < amountSegments; i++)
            {
                double angle = 2.0 * Math.PI * i / (double)amountSegments;
                float dx = (float)(Radius * Math.Cos(angle));
                float dy = (float)(Radius * Math.Sin(angle));
                coord.Add(CenterBottom + new Vector3(dx, dy, 0));
                coord.Add(CenterBottom + new Vector3(dx, dy, Height));
            }
            coord.Add(coord[0]);
            coord.Add(coord[1]);
            return coord.ToArray();
        }
    }
}

