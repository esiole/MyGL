using OpenTK;

namespace MyGL
{
    /// <summary>
    /// Представляет собой направленный свет без конкретного источника.
    /// </summary>
    public class DirectionLight : LightComponents
    {
        /// <summary>
        /// Направление света.
        /// </summary>
        public Vector3 Direction { get; set; }

        /// <summary>
        /// Создаёт направленный свет с параметрами по умолчанию.
        /// </summary>
        public DirectionLight() : base(new Vector3(-0.2f, -1.0f, -0.3f), new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.4f, 0.4f, 0.4f))
        {
            Direction = new Vector3(0.5f, 0.5f, 0.5f);
        }

        /// <summary>
        /// Создаёт направленный свет.
        /// </summary>
        /// <param name="ambient">Ambient компонент света.</param>
        /// <param name="diffuse">Diffuse компонент света.</param>
        /// <param name="specular">Specular компонент света.</param>
        /// <param name="direction">Вектор направления света.</param>
        public DirectionLight(Vector3 ambient, Vector3 diffuse, Vector3 specular, Vector3 direction) : base(ambient, diffuse, specular)
        {
            Direction = direction;
        }
    }
}
