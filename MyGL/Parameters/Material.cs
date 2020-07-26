using System;
using OpenTK;

namespace MyGL
{
    /// <summary>
    /// Материал, из которого состоят фигуры и их полигоны.
    /// </summary>
    public class Material : LightComponents
    {
        private float shininess;

        /// <summary>
        /// Блеск материала.
        /// </summary>
        public float Shininess 
        {
            get => shininess;
            set
            {
                if (value >= 0 && value <= 128) shininess = value;
                else throw new ArgumentException("Значение свойства Shininess должно находится в пределах [0; 128].");
            }
        }

        /// <summary>
        /// Создаёт материал с заданными параметрами.
        /// </summary>
        /// <param name="ambient">Ambient компонент.</param>
        /// <param name="diffuse">Diffuse компонент.</param>
        /// <param name="specular">Specular компонент.</param>
        /// <param name="shininess">Блеск. Задаётся числом от 0 до 1, как в справочниках.</param>
        public Material(Vector3 ambient, Vector3 diffuse, Vector3 specular, float shininess = 32f)
            : base(ambient, diffuse, specular)
        {
            Shininess = shininess * 128;
        }

        /// <summary>
        /// Создаёт материал с заданными параметрами.
        /// </summary>
        /// <param name="ambient">Ambient компонент.</param>
        /// <param name="diffuse">Diffuse компонент.</param>
        /// <param name="specular">Specular компонент.</param>
        /// <param name="shininess">Блеск. Задаётся числом от 0 до 1, как в справочниках.</param>
        /// <returns>Созданный материал.</returns>
        public static Material CreateMaterial(Vector3 ambient, Vector3 diffuse, Vector3 specular, float shininess)
        {
            return new Material(ambient, diffuse, specular, shininess);
        }

        /// <summary>
        /// Яркий источник света.
        /// </summary>
        public static Material LightSource => new Material(
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),
            0.25f
        );

        /// <summary>
        /// Медь.
        /// </summary>
        public static Material Brass => new Material(
            new Vector3(0.329412f, 0.223529f, 0.027451f),
            new Vector3(0.780392f, 0.568627f, 0.113725f), 
            new Vector3(0.992157f, 0.941176f, 0.807843f),
            0.21794872f
        );

        /// <summary>
        /// Бронза.
        /// </summary>
        public static Material Bronze => new Material(
            new Vector3(0.2125f, 0.1275f, 0.054f),
            new Vector3(0.714f, 0.4284f, 0.18144f),
            new Vector3(0.393548f, 0.271906f, 0.166721f),
            0.2f
        );

        /// <summary>
        /// Нефрит.
        /// </summary>
        public static Material Jade => new Material(
            new Vector3(0.135f, 0.2225f, 0.1575f),
            new Vector3(0.54f, 0.89f, 0.63f),
            new Vector3(0.316228f, 0.316228f, 0.316228f),
            0.1f
        );

        /// <summary>
        /// Жёлтый пластик.
        /// </summary>
        public static Material YellowPlastic => new Material(
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.5f, 0.5f, 0.0f),
            new Vector3(0.60f, 0.60f, 0.50f),
            0.25f
        );

        /// <summary>
        /// Серебро.
        /// </summary>
        public static Material Silver => new Material(
            new Vector3(0.19225f, 0.19225f, 0.19225f),
            new Vector3(0.50754f, 0.50754f, 0.50754f),
            new Vector3(0.508273f, 0.508273f, 0.508273f),
            0.4f
        );
    }
}
