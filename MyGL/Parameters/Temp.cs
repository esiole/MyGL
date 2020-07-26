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
    public sealed class Temp
    {
        public Vector3 Ambient { get; private set; }
        public Vector3 Diffuse { get; private set; }
        public Vector3 Specular { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Direction { get; private set; }
        public Vector3 LightOffsetPos { get; private set; }
        public float CutOff { get; private set; }
        public float OuterCutOff { get; private set; }
        public float IsSpotlight { get; private set; }
        public float Constant { get; private set; }
        public float Linear { get; private set; }
        public float Quadratic { get; private set; }

        public Temp(Vector3 ambient, Vector3 diffuse, Vector3 specular, Vector3 position, Vector3 direction,
            Vector3 lightOffsetPos, float cutOff, float outerCutOff, float isSpotlight, float constant, float linear, float quadratic)
        {
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Position = position;
            Direction = direction;
            LightOffsetPos = lightOffsetPos;
            CutOff = cutOff;
            OuterCutOff = outerCutOff;
            IsSpotlight = isSpotlight;
            Constant = constant;
            Linear = linear;
            Quadratic = quadratic;
        }
        
        public void SetDirectionVector(float? x = null, float? y = null, float? z = null)
        {
            var temp = new Vector3(Direction);
            if (x.HasValue) temp.X = x.Value - Position.X;
            if (y.HasValue) temp.Y = y.Value - Position.Y;
            if (z.HasValue) temp.Z = z.Value - Position.Z;
            Direction = temp;
            if (ParametersChange != null) ParametersChange();
        }

        public void SetLightOffsetPos(float? x = null, float? y = null, float? z = null)
        {
            var temp = new Vector3(LightOffsetPos);
            if (x.HasValue) temp.X = x.Value;
            if (y.HasValue) temp.Y = y.Value;
            if (z.HasValue) temp.Z = z.Value;
            LightOffsetPos = temp;
            if (ParametersChange != null) ParametersChange();
        }

        public event Action ParametersChange;
    }
}
