using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace MyGL
{
    public class Shader : IDisposable
    {
        private bool disposedValue;
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

        //public int GetAttribLocation(string attribName)
        //{
        //    return GL.GetAttribLocation(Handle, attribName);
        //}

        //public int GetUniformLocation(string name)
        //{
        //    return GL.GetUniformLocation(Handle, name);
        //}

        //public void UniformMatrix4(int unifMatrix, bool transpose, Matrix4 matrix)
        //{
        //    GL.UniformMatrix4(unifMatrix, transpose, ref matrix);
        //}

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

        // управление шейдером
        public void SetMaterial(Material material)
        {
            SetUniform3("material.ambient", material.Ambient);
            SetUniform3("material.diffuse", material.Diffuse);
            SetUniform3("material.specular", material.Specular);
            SetUniform1("material.shininess", material.Shininess);
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

        public void SetNullDirLight()
        {

        }

        public void SetNullPointLight()
        {

        }

        private int countPointLight = 0;
        public void SetPointLight(PointLight light)
        {
            SetUniform3($"pointLights[{countPointLight}].ambient", light.Ambient);
            SetUniform3($"pointLights[{countPointLight}].diffuse", light.Diffuse);
            SetUniform3($"pointLights[{countPointLight}].specular", light.Specular);
            SetUniform3($"pointLights[{countPointLight}].position", light.Position);
            SetUniform1($"pointLights[{countPointLight}].constant", light.Constant);
            SetUniform1($"pointLights[{countPointLight}].linear", light.Linear);
            SetUniform1($"pointLights[{countPointLight}].quadratic", light.Quadratic);
            countPointLight++;
        }

        private int countSpotLight = 0;

        public void SetSpotLight(SpotLight light)
        {
            SetUniform3($"spotLights[{countSpotLight}].ambient", light.Ambient);
            SetUniform3($"spotLights[{countSpotLight}].diffuse", light.Diffuse);
            SetUniform3($"spotLights[{countSpotLight}].specular", light.Specular);
            SetUniform3($"spotLights[{countSpotLight}].direction", light.Direction);
            SetUniform3($"spotLights[{countSpotLight}].position", light.Position);
            SetUniform1($"spotLights[{countSpotLight}].constant", light.Constant);
            SetUniform1($"spotLights[{countSpotLight}].linear", light.Linear);
            SetUniform1($"spotLights[{countSpotLight}].quadratic", light.Quadratic);
            SetUniform1($"spotLights[{countSpotLight}].cutOff", light.CutOff);
            SetUniform1($"spotLights[{countSpotLight}].outerCutOff", light.OuterCutOff);
            countSpotLight++;
        }

        public void SetDirLight(DirectionLight light)
        {
            SetUniform3("dirLight.ambient", light.Ambient);
            SetUniform3("dirLight.diffuse", light.Diffuse);
            SetUniform3("dirLight.specular", light.Specular);
            SetUniform3("dirLight.direction", light.Direction);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);
                disposedValue = true;
            }
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
    }
}
