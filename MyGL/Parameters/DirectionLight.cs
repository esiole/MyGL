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
    public class DirectionLight : LightComponents
    {
        public Vector3 Direction { get; set; }

        public DirectionLight() : base(new Vector3(-0.2f, -1.0f, -0.3f), new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.4f, 0.4f, 0.4f))
        {
            Direction = new Vector3(0.5f, 0.5f, 0.5f);
        }

        public DirectionLight(Vector3 ambient, Vector3 diffuse, Vector3 specular, Vector3 direction) : base(ambient, diffuse, specular)
        {
            Direction = direction;
        }
    }
}
