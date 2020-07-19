using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace MyGL
{
    public class Shader : IDisposable
    {
        private bool disposedValue = false;
        public int Handle { get; private set; }

        public Shader(string VertexShaderSource, string FragmentShaderSource)
        {
            int VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);
            int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            GL.CompileShader(VertexShader);
            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (infoLogVert != System.String.Empty)
                System.Console.WriteLine(infoLogVert);
            GL.CompileShader(FragmentShader);
            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);
            if (infoLogFrag != System.String.Empty)
                System.Console.WriteLine(infoLogFrag);

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

        private void ReleaseHandle()
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            ReleaseHandle();
            GC.SuppressFinalize(this);
        }

        ~Shader()
        {
            if (GraphicsContext.CurrentContext != null && !GraphicsContext.CurrentContext.IsDisposed)
            {
                ReleaseHandle();
            }
        }
    }
}
