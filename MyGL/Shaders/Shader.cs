using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace MyGL
{
    /// <summary>
    /// Представляет собой шейдер - исполняемую программу на языке GLSL.
    /// </summary>
    public class Shader : IDisposable
    {
        private bool disposedValue;
        private int Id { get; set; }

        /// <summary>
        /// Компилирует шейдер из исходного кода.
        /// </summary>
        /// <param name="source">Источник исходного когда шейдера.</param>
        public Shader(IShaderSource source) : this(source.VertexShaderSource, source.FragmentShaderSource) { }

        /// <summary>
        /// Компилирует шейдер из исходного кода.
        /// </summary>
        /// <param name="vertexShaderSource">Исходный код вершинного шейдера.</param>
        /// <param name="fragmentShaderSource">Исходный код фрагментного шейдера.</param>
        public Shader(string vertexShaderSource, string fragmentShaderSource)
        {
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);

            GL.CompileShader(vertexShader);
            string infoLogVert = GL.GetShaderInfoLog(vertexShader);
            if (infoLogVert != string.Empty)
                throw new ShaderCompileException("Error compile vertex shader.");
            GL.CompileShader(fragmentShader);
            string infoLogFrag = GL.GetShaderInfoLog(fragmentShader);
            if (infoLogFrag != string.Empty)
                throw new ShaderCompileException("Error compile fragment shader.");

            Id = GL.CreateProgram();
            GL.AttachShader(Id, vertexShader);
            GL.AttachShader(Id, fragmentShader);
            GL.LinkProgram(Id);

            GL.DetachShader(Id, vertexShader);
            GL.DetachShader(Id, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);
        }

        /// <summary>
        /// Делает шейдер текущим для выполнения на видеокарте.
        /// </summary>
        public void Use()
        {
            GL.UseProgram(Id);
        }

        /// <summary>
        /// Установка значения uniform матрицы в шейдере.
        /// </summary>
        /// <param name="name">Имя uniform матрицы в шейдере.</param>
        /// <param name="transpose">Следует ли транспонировать матрицу.</param>
        /// <param name="matrix">Матрица.</param>
        public void SetUniformMatrix4(string name, bool transpose, ref Matrix4 matrix)
        {
            int matrixHandle = GL.GetUniformLocation(Id, name);
            GL.UniformMatrix4(matrixHandle, transpose, ref matrix);
        }

        /// <summary>
        /// Утановка значения uniform вектора в шейдере.
        /// </summary>
        /// <param name="name">Имя uniform вектора в шейдере.</param>
        /// <param name="vec">Вектор.</param>
        public void SetUniform3(string name, Vector3 vec)
        {
            int vectorHandle = GL.GetUniformLocation(Id, name);
            GL.Uniform3(vectorHandle, vec.X, vec.Y, vec.Z);
        }

        /// <summary>
        /// Утановка значения uniform float в шейдере.
        /// </summary>
        /// <param name="name">Имя uniform float в шейдере.</param>
        /// <param name="number">Число типа float.</param>
        public void SetUniform1(string name, float number)
        {
            int floatHandle = GL.GetUniformLocation(Id, name);
            GL.Uniform1(floatHandle, number);
        }

        /// <summary>
        /// Освободить все выделенные шейдером ресурсы.
        /// </summary>
        /// <param name="disposing">Был ли вызван Dispose метод вручную.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Id);
                disposedValue = true;
            }
        }

        /// <summary>
        /// Финализатор, вызывающий Dispose метод.
        /// </summary>
        ~Shader()
        {
            Dispose(false);
        }

        /// <summary>
        /// Освободить все выделенные шейдером ресурсы.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
