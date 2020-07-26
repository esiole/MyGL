namespace MyGL
{
    /// <summary>
    /// Базовый шейдер. Работает с координатами вершин и цветом фрагментов.
    /// </summary>
    public sealed class BasicShaderSource : IShaderSource
    {
        private const string vertexShaderSource = @"
            #version 330 core
    
            layout (location = 0) in vec3 aPosition;
            layout (location = 1) in vec3 aColor;

            varying vec3 Color;

            void main()
            {
                Color = aColor;
                gl_Position = vec4(aPosition, 1.0);
            }
        ";

        private const string fragmentShaderSource = @"
            #version 330 core

            varying vec3 Color; 

            void main()
            {
                gl_FragColor = vec4(Color, 1.0f);
            }
        ";

        /// <summary>
        /// Получить исходный код вершинного шейдера.
        /// </summary>
        public string VertexShaderSource => vertexShaderSource;

        /// <summary>
        /// Получить исходный код фрагментного шейдера.
        /// </summary>
        public string FragmentShaderSource => fragmentShaderSource;
    }
}
