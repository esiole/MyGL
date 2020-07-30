using OpenTK;
using System;

namespace MyGL
{
    /// <summary>
    /// Прожектор.
    /// </summary>
    public sealed class SpotLight : LightSource
    {
        private float cutOff;
        private float outerCutOff;

        /// <summary>
        /// Направление прожектора.
        /// </summary>
        public Vector3 Direction { get; set; }

        /// <summary>
        /// Внутренний косинус угла прожектора (угол отсечки).
        /// </summary>
        public float CutOff 
        {
            get => cutOff;
            set => cutOff = CheckCos(value);
        }

        /// <summary>
        /// Внешний косинус угла прожектора.
        /// </summary>
        public float OuterCutOff 
        {
            get => outerCutOff;
            set => outerCutOff = CheckCos(value);
        }

        /// <summary>
        /// Создаёт прожектор со значениями свойств по умочанию.
        /// </summary>
        public SpotLight() : base(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(0.0f, 0.0f, 0.0f), 1.0f, 1.0f, 1.0f)
        { }

        /// <summary>
        /// Создаёт прожектор.
        /// </summary>
        /// <param name="ambient">Ambient компонент света.</param>
        /// <param name="diffuse">Diffuse компонент света.</param>
        /// <param name="specular">Specular компонент света.</param>
        /// <param name="position">Расположение прожектора.</param>
        /// <param name="direction">Направление прожектора.</param>
        /// <param name="constant">Постоянная составляющая затухания.</param>
        /// <param name="linear">Линейная составляющая затухания.</param>
        /// <param name="quadratic">Квадратичная составляющая затухания.</param>
        /// <param name="cutOff">Внутренний косинус.</param>
        /// <param name="outerCutOff">Внешний косинус.</param>
        public SpotLight(Vector3 ambient, Vector3 diffuse, Vector3 specular, Vector3 position, Vector3 direction,
            float constant, float linear, float quadratic, float cutOff, float outerCutOff)
            : base(ambient, diffuse, specular, position, constant, linear, quadratic) 
        {
            Direction = direction;
            CutOff = cutOff;
            OuterCutOff = outerCutOff;
        }

        /// <summary>
        /// Проверяет, является ли переданное значение косинуса допустимым.
        /// </summary>
        /// <param name="cos">Косинус угла.</param>
        /// <returns>Косинус угла, если он допустим, иначе будет выброшено ArgumentException.</returns>
        private float CheckCos(float cos)
        {
            if (Math.Abs(cos) > 1.0f) throw new ArgumentException("Косинус больше единицы.");
            else return cos;
        }
    }
}
