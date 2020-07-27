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
        public Material(Vector3 ambient, Vector3 diffuse, Vector3 specular, float shininess)
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
        /// Изумруд.
        /// </summary>
        public static Material Emerald => new Material(
            new Vector3(0.0215f, 0.1745f, 0.0215f),
            new Vector3(0.07568f, 0.61424f, 0.07568f),
            new Vector3(0.633f, 0.727811f, 0.633f),
            0.6f
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
        /// Обсидиан.
        /// </summary>
        public static Material Obsidian => new Material(
            new Vector3(0.05375f, 0.05f, 0.06625f),
            new Vector3(0.18275f, 0.17f, 0.22525f),
            new Vector3(0.332741f, 0.328634f, 0.346435f),
            0.3f
        );

        /// <summary>
        /// Жемчуг.
        /// </summary>
        public static Material Pearl => new Material(
            new Vector3(0.25f, 0.20725f, 0.20725f),
            new Vector3(1.0f, 0.829f, 0.829f),
            new Vector3(0.296648f, 0.296648f, 0.296648f),
            0.088f
        );

        /// <summary>
        /// Рубин.
        /// </summary>
        public static Material Ruby => new Material(
            new Vector3(0.1745f, 0.01175f, 0.01175f),
            new Vector3(0.61424f, 0.04136f, 0.04136f),
            new Vector3(0.727811f, 0.626959f, 0.626959f),
            0.6f
        );

        /// <summary>
        /// Бирюза.
        /// </summary>
        public static Material Turquoise => new Material(
            new Vector3(0.1f, 0.18725f, 0.1745f),
            new Vector3(0.396f, 0.74151f, 0.69102f),
            new Vector3(0.297254f, 0.30829f, 0.306678f),
            0.1f
        );

        /// <summary>
        /// Латунь.
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
        /// Хром.
        /// </summary>
        public static Material Chrome => new Material(
            new Vector3(0.25f, 0.25f, 0.25f),
            new Vector3(0.4f, 0.4f, 0.4f),
            new Vector3(0.774597f, 0.774597f, 0.774597f),
            0.6f
        );

        /// <summary>
        /// Медь.
        /// </summary>
        public static Material Copper => new Material(
            new Vector3(0.19125f, 0.0735f, 0.0225f),
            new Vector3(0.7038f, 0.27048f, 0.0828f),
            new Vector3(0.256777f, 0.137622f, 0.086014f),
            0.1f
        );

        /// <summary>
        /// Золото.
        /// </summary>
        public static Material Gold => new Material(
            new Vector3(0.24725f, 0.1995f, 0.0745f),
            new Vector3(0.75164f, 0.60648f, 0.22648f),
            new Vector3(0.628281f, 0.555802f, 0.366065f),
            0.4f
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

        /// <summary>
        /// Жёлтый пластик.
        /// </summary>
        public static Material YellowPlastic => new Material(
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.5f, 0.5f, 0.0f),
            new Vector3(0.60f, 0.60f, 0.50f),
            0.25f
        );
    }
}
