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
    public abstract class Parameters
    {
        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }

        public Parameters()
        {
            Ambient = new Vector3(1.0f, 1.0f, 1.0f);
            Diffuse = new Vector3(1.0f, 1.0f, 1.0f);
            Specular = new Vector3(1.0f, 1.0f, 1.0f);
        }

        public Parameters(Vector3 ambient, Vector3 diffuse, Vector3 specular)
        {
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
        }
    }
}
