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
