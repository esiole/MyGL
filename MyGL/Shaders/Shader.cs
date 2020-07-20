﻿using OpenTK;
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

        // управление шейдером
        public void SetMaterial(Material material)
        {
            SetUniform3("material.ambient", material.Ambient);
            SetUniform3("material.diffuse", material.Diffuse);
            SetUniform3("material.specular", material.Specular);
            SetUniform1("material.shininess", material.Shininess);
        }

        public void SetLightingParameters(LightingParameters parameters, Vector3 position)
        {
            SetUniform3("light.ambient", parameters.Ambient);
            SetUniform3("light.diffuse", parameters.Diffuse);
            SetUniform3("light.specular", parameters.Specular);
            SetUniform3("light.position", position);
            SetUniform3("light.direction", parameters.Direction);
            SetUniform1("light.cutOff", parameters.CutOff);
            SetUniform1("light.outerCutOff", parameters.OuterCutOff);
            SetUniform1("light.isSpotlight", parameters.IsSpotlight);
            SetUniform1("light.constant", parameters.Constant);
            SetUniform1("light.linear", parameters.Linear);
            SetUniform1("light.quadratic", parameters.Quadratic);
        }

        public void SetViewMatrix(Matrix4 view)
        {
            SetUniformMatrix4("view", false, view);
        }

        public void SetProjectionMatrix(Matrix4 projection)
        {
            SetUniformMatrix4("projection", false, projection);
        }

        public void SetModelMatrix(Matrix4 model)
        {
            SetUniformMatrix4("model", false, model);
        }

        public void SetCameraPos(Vector3 cameraPos)
        {
            SetUniform3("viewPos", cameraPos);
        }
    }
}
