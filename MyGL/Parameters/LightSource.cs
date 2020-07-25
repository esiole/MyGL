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
    public abstract class LightSource : DirectionLight
    {
        public Shape Source { get; set; }
        public Vector3 Position { get; set; }
        public float Constant { get; set; }
        public float Linear { get; set; }
        public float Quadratic { get; set; }

        public LightSource(Vector3 ambient, Vector3 diffuse, Vector3 specular, Vector3 position, Vector3 direction,
            float constant, float linear, float quadratic) : base(ambient, diffuse, specular, direction)
        {
            Position = position;
            Constant = constant;
            Linear = linear;
            Quadratic = quadratic;
        }
    }
}
