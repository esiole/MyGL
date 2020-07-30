using OpenTK;
using System;

namespace MyGL
{
    /// <summary>
    /// Абстрактный класс, представляющий собой источник света на сцене.
    /// </summary>
    public abstract class LightSource : LightComponents, IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// Фигура-источник света.
        /// </summary>
        public Shape Source { get; set; }

        /// <summary>
        /// Расположение источника
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Постоянная составляющая затухания.
        /// </summary>
        public float Constant { get; set; }

        /// <summary>
        /// Линейная составляющая затухания.
        /// </summary>
        public float Linear { get; set; }

        /// <summary>
        /// Квадратичная составляющая затухания.
        /// </summary>
        public float Quadratic { get; set; }

        /// <summary>
        /// Создание источника света.
        /// </summary>
        /// <param name="ambient">Ambient компонент света.</param>
        /// <param name="diffuse">Diffuse компонент света.</param>
        /// <param name="specular">Specular компонент света.</param>
        /// <param name="position">Расположение источника.</param>
        /// <param name="constant">Постоянная составляющая затухания.</param>
        /// <param name="linear">Линейная составляющая затухания.</param>
        /// <param name="quadratic">Квадратичная составляющая затухания.</param>
        public LightSource(Vector3 ambient, Vector3 diffuse, Vector3 specular, Vector3 position,
            float constant, float linear, float quadratic) : base(ambient, diffuse, specular)
        {
            Position = position;
            Constant = constant;
            Linear = linear;
            Quadratic = quadratic;
        }

        /// <summary>
        /// Освободить выделенные ресурсы на источник света.
        /// </summary>
        /// <param name="disposing">Вызван ли метод Dispose вручную.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Source.Dispose();
                }
                disposedValue = true;
            }
        }

        /// <summary>
        /// Освободить выделенные ресурсы на источник света.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
