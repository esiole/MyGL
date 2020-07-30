using OpenTK;

namespace MyGL
{
    /// <summary>
    /// Точечный источник света на сцене.
    /// </summary>
    public sealed class PointLight : LightSource
    {
        /// <summary>
        /// Создаёт точечный источник света со значениями свойств по умочанию.
        /// </summary>
        public PointLight() : base(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(0.0f, 0.0f, 0.0f), 1.0f, 1.0f, 1.0f)
        { }

        /// <summary>
        /// Создаёт точечный источник света.
        /// </summary>
        /// <param name="ambient">Ambient компонент света.</param>
        /// <param name="diffuse">Diffuse компонент света.</param>
        /// <param name="specular">Specular компонент света.</param>
        /// <param name="position">Расположение источника.</param>
        /// <param name="constant">Постоянная составляющая затухания.</param>
        /// <param name="linear">Линейная составляющая затухания.</param>
        /// <param name="quadratic">Квадратичная составляющая затухания.</param>
        public PointLight(Vector3 ambient, Vector3 diffuse, Vector3 specular, Vector3 position,
            float constant, float linear, float quadratic) 
            : base(ambient, diffuse, specular, position, constant, linear, quadratic)
        { }
    }
}
