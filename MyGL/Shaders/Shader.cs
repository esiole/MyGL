using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace MyGL
{
    public class Shader : IDisposable
    {
        private bool isDisposed = false;
        private int Handle { get; set; }

        public Shader(IShaderSource source) : this(source.VertexShaderSource, source.FragmentShaderSource) { }

        public Shader(string vertexShaderSource, string fragmentShaderSource)
        {
            int VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, vertexShaderSource);
            int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, fragmentShaderSource);

            GL.CompileShader(VertexShader);
            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (infoLogVert != string.Empty)
                throw new ShaderCompileException("Error compile vertex shader.");
            GL.CompileShader(FragmentShader);
            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);
            if (infoLogFrag != string.Empty)
                throw new ShaderCompileException("Error compile fragment shader.");

            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);
            GL.LinkProgram(Handle);

            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        public int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(Handle, name);
        }

        public void UniformMatrix4(int unifMatrix, bool transpose, Matrix4 matrix)
        {
            GL.UniformMatrix4(unifMatrix, transpose, ref matrix);
        }

        public void SetUniformMatrix4(string name, bool transpose, Matrix4 matrix)
        {
            int matrixHandle = GL.GetUniformLocation(Handle, name);
            GL.UniformMatrix4(matrixHandle, transpose, ref matrix);
        }

        public void SetUniform3(string name, Vector3 vec)
        {
            int matrixHandle = GL.GetUniformLocation(Handle, name);
            GL.Uniform3(matrixHandle, vec.X, vec.Y, vec.Z);
        }

        public void SetUniform1(string name, float vec)
        {
            int matrixHandle = GL.GetUniformLocation(Handle, name);
            GL.Uniform1(matrixHandle, vec);
        }

        ~Shader()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool fromDisposeMethod)
        {
            if (!isDisposed)
            {
                GL.DeleteProgram(Handle);
                isDisposed = true;
            }
        }
    }
}
