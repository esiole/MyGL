using System;

namespace MyGL
{
    /// <summary>
    /// Исключение при ошибке компиляции шейдеров.
    /// </summary>
    public sealed class ShaderCompileException : Exception
    {
        /// <summary>
        /// Создаёт новое исключение ошибки компиляции шейдеров.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public ShaderCompileException(string message) : base(message) { }
    }
}
