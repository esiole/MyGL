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
    public sealed class PointLight : LightSource
    {
        public PointLight() : base(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), 1.0f, 1.0f, 1.0f)
        { }

        public PointLight(Vector3 ambient, Vector3 diffuse, Vector3 specular, Vector3 position, Vector3 direction,
            float constant, float linear, float quadratic) 
            : base(ambient, diffuse, specular, position, direction, constant, linear, quadratic)
        { }
    }
}
