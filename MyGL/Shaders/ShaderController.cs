using System;
using OpenTK;

namespace MyGL
{
    /// <summary>
    /// Представляет собой объект, создающий шейдер и управляющий им.
    /// </summary>
    public sealed class ShaderController : IDisposable
    {
        private bool disposedValue;
        private int countPointLight = 0;
        private int countSpotLight = 0;

        /// <summary>
        /// Шейдер.
        /// </summary>
        public readonly Shader Shader;

        /// <summary>
        /// Создаёт шейдер из исходного кода.
        /// </summary>
        /// <param name="vertexShaderSource">Исходный код вершинного шейдера.</param>
        /// <param name="fragmentShaderSource">Исходный код фрагментного шейдера.</param>
        public ShaderController(string vertexShaderSource, string fragmentShaderSource)
        {
            Shader = new Shader(vertexShaderSource, fragmentShaderSource);
            Shader.Use();
        }

        /// <summary>
        /// Включаёт влияние фонового направленного света.
        /// </summary>
        /// <param name="light">Направленный свет без источника.</param>
        public void EnableDirLight(DirectionLight light)
        {
            Shader.SetUniform3("dirLight.ambient", light.Ambient);
            Shader.SetUniform3("dirLight.diffuse", light.Diffuse);
            Shader.SetUniform3("dirLight.specular", light.Specular);
            Shader.SetUniform3("dirLight.direction", light.Direction);
        }

        /// <summary>
        /// Отключает влияние фонового направленного света. 
        /// </summary>
        public void DisableDirLight()
        {
            EnableDirLight(new DirectionLight(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), 
                new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.5f, 0.5f, 0.5f)));
        }

        /// <summary>
        /// Добавляет параметрв источника света в шейдер.
        /// </summary>
        /// <param name="light">Источник света.</param>
        public void AddLight(LightSource light)
        {
            if (light is PointLight) AddPointLight(light as PointLight);
            if (light is SpotLight) AddSpotLight(light as SpotLight);
        }

        /// <summary>
        /// Добавляет точечный источник света в шейдер.
        /// </summary>
        /// <param name="light">Точечный источник света.</param>
        private void AddPointLight(PointLight light)
        {
            AddLightSource(light, "pointLights", countPointLight);
            countPointLight++;
        }

        /// <summary>
        /// Добавляет прожектор в шейдер.
        /// </summary>
        /// <param name="light">Прожектор.</param>
        private void AddSpotLight(SpotLight light)
        {
            AddLightSource(light, "spotLights", countSpotLight);
            Shader.SetUniform3($"spotLights[{countSpotLight}].direction", light.Direction);
            Shader.SetUniform1($"spotLights[{countSpotLight}].cutOff", light.CutOff);
            Shader.SetUniform1($"spotLights[{countSpotLight}].outerCutOff", light.OuterCutOff);
            countSpotLight++;
        }

        /// <summary>
        /// Добавляет общие характеристики источника света в шейдер.
        /// </summary>
        /// <param name="light">Источник света.</param>
        /// <param name="nameArrayInShader">Имя массива, куда добавляется источник.</param>
        /// <param name="index">В какую позицию массива добавляется.</param>
        private void AddLightSource(LightSource light, string nameArrayInShader, int index)
        {
            Shader.SetUniform3($"{nameArrayInShader}[{index}].ambient", light.Ambient);
            Shader.SetUniform3($"{nameArrayInShader}[{index}].diffuse", light.Diffuse);
            Shader.SetUniform3($"{nameArrayInShader}[{index}].specular", light.Specular);
            Shader.SetUniform3($"{nameArrayInShader}[{index}].position", light.Position);
            Shader.SetUniform1($"{nameArrayInShader}[{index}].constant", light.Constant);
            Shader.SetUniform1($"{nameArrayInShader}[{index}].linear", light.Linear);
            Shader.SetUniform1($"{nameArrayInShader}[{index}].quadratic", light.Quadratic);
        }

        /// <summary>
        /// Устанавливает необходмые uniform переменные для работы шейдера.
        /// </summary>
        /// <param name="view">Видовая матрица.</param>
        /// <param name="projection">Матрица проекции.</param>
        /// <param name="cameraPos">Позиция камеры.</param>
        public void PrepareDraw(Matrix4 view, Matrix4 projection, Vector3 cameraPos)
        {
            Shader.SetUniformMatrix4("view", false, ref view);
            Shader.SetUniformMatrix4("projection", false, ref projection);
            Shader.SetUniform3("viewPos", cameraPos);
        }

        /// <summary>
        /// Устанавливает модельную матрицу для отрисовки текущей фигуры.
        /// </summary>
        /// <param name="model">Модельная матрица.</param>
        public void SetModelMatrix(Matrix4 model)
        {
            Shader.SetUniformMatrix4("model", false, ref model);
        }

        /// <summary>
        /// Устанавливает материал текущей фигуры для отрисовки.
        /// </summary>
        /// <param name="material">Материал фигуры.</param>
        public void SetMaterial(Material material)
        {
            Shader.SetUniform3("material.ambient", material.Ambient);
            Shader.SetUniform3("material.diffuse", material.Diffuse);
            Shader.SetUniform3("material.specular", material.Specular);
            Shader.SetUniform1("material.shininess", material.Shininess);
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Shader.Dispose();
                }
                disposedValue = true;
            }
        }

        /// <summary>
        /// Освободить все выделенные контроллером ресурсы.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
