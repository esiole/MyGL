namespace MyGL
{
    /// <summary>
    /// Предоставляет механизм для получения исходного кода вершинного и фрагментного шейдеров.
    /// </summary>
    public interface IShaderSource 
    {
        /// <summary>
        /// Получить исходный код вершинного шейдера.
        /// </summary>
        string VertexShaderSource { get; }

        /// <summary>
        /// Получить исходный код фрагментного шейдера.
        /// </summary>
        string FragmentShaderSource { get; }
    }
}
