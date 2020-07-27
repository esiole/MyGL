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
    public sealed class ShaderController : IDisposable
    {
        private bool disposedValue;
        private Shader shader;
        private int countPointLight = 0;
        private int countSpotLight = 0;

        public ShaderController(IShaderSource shaderSource)
        {
            shader = new Shader(shaderSource);
            shader.Use();
        }

        public void EnableDirLight(DirectionLight light)
        {
            shader.SetUniform3("dirLight.ambient", light.Ambient);
            shader.SetUniform3("dirLight.diffuse", light.Diffuse);
            shader.SetUniform3("dirLight.specular", light.Specular);
            shader.SetUniform3("dirLight.direction", light.Direction);
        }

        public void DisableDirLight()
        {
            EnableDirLight(new DirectionLight(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), 
                new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.5f, 0.5f, 0.5f)));
        }

        public void AddPointLight(PointLight light)
        {
            AddLightSource(light, "pointLights", countPointLight);
            countPointLight++;
        }

        public void AddSpotLight(SpotLight light)
        {
            AddLightSource(light, "spotLights", countSpotLight);
            shader.SetUniform3($"spotLights[{countSpotLight}].direction", light.Direction);
            shader.SetUniform1($"spotLights[{countSpotLight}].cutOff", light.CutOff);
            shader.SetUniform1($"spotLights[{countSpotLight}].outerCutOff", light.OuterCutOff);
            countSpotLight++;
        }

        private void AddLightSource(LightSource light, string nameArrayInShader, int index)
        {
            shader.SetUniform3($"{nameArrayInShader}[{index}].ambient", light.Ambient);
            shader.SetUniform3($"{nameArrayInShader}[{index}].diffuse", light.Diffuse);
            shader.SetUniform3($"{nameArrayInShader}[{index}].specular", light.Specular);
            shader.SetUniform3($"{nameArrayInShader}[{index}].position", light.Position);
            shader.SetUniform1($"{nameArrayInShader}[{index}].constant", light.Constant);
            shader.SetUniform1($"{nameArrayInShader}[{index}].linear", light.Linear);
            shader.SetUniform1($"{nameArrayInShader}[{index}].quadratic", light.Quadratic);
        }

        public void PrepareDraw(Matrix4 view, Matrix4 projection, Vector3 cameraPos)
        {
            SetViewMatrix(view);
            SetProjectionMatrix(projection);
            SetCameraPos(cameraPos);
        }

        public void SetViewMatrix(Matrix4 view)
        {
            shader.SetUniformMatrix4("view", false, view);
        }

        public void SetProjectionMatrix(Matrix4 projection)
        {
            shader.SetUniformMatrix4("projection", false, projection);
        }

        public void SetCameraPos(Vector3 cameraPos)
        {
            shader.SetUniform3("viewPos", cameraPos);
        }

        public void SetModelMatrix(Matrix4 model)
        {
            shader.SetUniformMatrix4("model", false, model);
        }

        public void SetMaterial(Material material)
        {
            shader.SetUniform3("material.ambient", material.Ambient);
            shader.SetUniform3("material.diffuse", material.Diffuse);
            shader.SetUniform3("material.specular", material.Specular);
            shader.SetUniform1("material.shininess", material.Shininess);
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    shader.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
