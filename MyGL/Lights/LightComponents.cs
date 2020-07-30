using System;
using OpenTK;

namespace MyGL
{
    /// <summary>
    /// Абстрактный класс, содержащий различные компоненты света в 3D графике.
    /// </summary>
    public abstract class LightComponents
    {
        private Vector3 ambient;
        private Vector3 diffuse;
        private Vector3 specular;

        /// <summary>
        /// Ambient компонент света.
        /// </summary>
        public Vector3 Ambient
        {
            get => ambient;
            set => ambient = CheckParameter(value, "Компоненты вектора Ambient не должны превышать по модулю 1.");
        }

        /// <summary>
        /// Diffuse компонент света.
        /// </summary>
        public Vector3 Diffuse 
        {
            get => diffuse;
            set => diffuse = CheckParameter(value, "Компоненты вектора Diffuse не должны превышать по модулю 1.");
        }

        /// <summary>
        /// Specular компонент света.
        /// </summary>
        public Vector3 Specular 
        {
            get => specular;
            set => specular = CheckParameter(value, "Компоненты вектора Specular не должны превышать по модулю 1.");
        }

        /// <summary>
        /// Объединяет переданные компоненты света в один класс.
        /// </summary>
        /// <param name="ambient">Ambient компонент света.</param>
        /// <param name="diffuse">Diffuse компонент света.</param>
        /// <param name="specular">Specular компонент света.</param>
        public LightComponents(Vector3 ambient, Vector3 diffuse, Vector3 specular)
        {
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
        }

        /// <summary>
        /// Проверяет, проходит ли валидацию устанавливаемый компонент света.
        /// </summary>
        /// <param name="vector">Вектор-компонент света.</param>
        /// <param name="errorMessage">Сообщение об ошибке, если компонент не проходит валидацию.</param>
        /// <returns>Компонент света, если он прошёл валидацию. Иначе будет выброшено ArgumentException.</returns>
        private Vector3 CheckParameter(Vector3 vector, string errorMessage = "")
        {
            if (IsValid(vector)) return vector;
            else throw new ArgumentException(errorMessage);
        }

        /// <summary>
        /// Проверяет, являются ли компоненты вектора по модулю меньше или равны 1.
        /// </summary>
        /// <param name="vector">Вектор-компонент света.</param>
        /// <returns>True, если компонент прошёл валидацию, иначе false.</returns>
        private bool IsValid(Vector3 vector)
        {
            if (Math.Abs(vector.X) > 1.0f) return false;
            if (Math.Abs(vector.Y) > 1.0f) return false;
            if (Math.Abs(vector.Z) > 1.0f) return false;
            return true;
        }
    }
}
