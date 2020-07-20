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
    public sealed class LightingParameters
    {
        public Vector3 Ambient { get; private set; }
        public Vector3 Diffuse { get; private set; }
        public Vector3 Specular { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Direction { get; private set; }
        public float CutOff { get; private set; }
        public float OuterCutOff { get; private set; }
        public float IsSpotlight { get; private set; }
        public float Constant { get; private set; }
        public float Linear { get; private set; }
        public float Quadratic { get; private set; }

        public LightingParameters(Vector3 ambient, Vector3 diffuse, Vector3 specular, Vector3 position, Vector3 direction,
            float cutOff, float outerCutOff, float isSpotlight, float constant, float linear, float quadratic)
        {
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Position = position;
            Direction = direction;
            CutOff = cutOff;
            OuterCutOff = outerCutOff;
            IsSpotlight = isSpotlight;
            Constant = constant;
            Linear = linear;
            Quadratic = quadratic;
        }
    }
}
