using System;
using OpenTK;

namespace MyGL
{
    public abstract class Parameters
    {
        private Vector3 ambient;
        public Vector3 Ambient
        {
            get => ambient;
            set => ambient = CheckParameter(value, "Компоненты вектора Ambient не должны превышать по модулю 1");
        }

        private Vector3 diffuse;
        public Vector3 Diffuse 
        {
            get => diffuse;
            set => diffuse = CheckParameter(value, "Компоненты вектора Diffuse не должны превышать по модулю 1");
        }

        private Vector3 specular;
        public Vector3 Specular 
        {
            get => specular;
            set => specular = CheckParameter(value, "Компоненты вектора Specular не должны превышать по модулю 1");
        }

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

        private Vector3 CheckParameter(Vector3 vector, string errorMessage)
        {
            if (IsValid(vector)) return vector;
            else throw new ArgumentException(errorMessage);
        }

        private bool IsValid(Vector3 vector)
        {
            if (Math.Abs(vector.X) > 1.0f) return false;
            if (Math.Abs(vector.Y) > 1.0f) return false;
            if (Math.Abs(vector.Z) > 1.0f) return false;
            return true;
        }
    }
}
