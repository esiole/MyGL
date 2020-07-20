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
    public class Material
    {
        public Vector3 Ambient { get; private set; }
        public Vector3 Diffuse { get; private set; }
        public Vector3 Specular { get; private set; }
        public float Shininess { get; private set; }

        public Material()
        {
            Ambient = new Vector3(1.0f, 1.0f, 1.0f);
            Diffuse = new Vector3(1.0f, 1.0f, 1.0f);
            Specular = new Vector3(1.0f, 1.0f, 1.0f);
            Shininess = 32.0f;
        }

        public Material(Vector3 ambient, Vector3 diffuse, Vector3 specular, float shininess)
        {
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
        }

        public static Material CreateMaterial(Vector3 ambient, Vector3 diffuse, Vector3 specular, float shininess)
        {
            return new Material(ambient, diffuse, specular, shininess);
        }
    }
}
