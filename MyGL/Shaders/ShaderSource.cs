namespace MyGL
{
    /// <summary>
    /// Генерирует исходные коды различных шейдеров.
    /// </summary>
    public static class ShaderSource 
    {
        /// <summary>
        /// Получение исходного кода базового шейдера.
        /// </summary>
        /// <returns>Исходный код базового шейдера.</returns>
        public static IShaderSource Basic()
        {
            return new BasicShaderSource();
        }

        /// <summary>
        /// Получение исходного кода шейдера Фонга.
        /// </summary>
        /// <param name="countPointLight">Количество точечных источников света.</param>
        /// <param name="countSpotLight">Количество прожекторов.</param>
        /// <returns>Исходный код шейдера Фонга.</returns>
        public static IShaderSource Phong(int countPointLight, int countSpotLight)
        {
            return new PhongShaderSource(countPointLight, countSpotLight);
        }
    }
}
