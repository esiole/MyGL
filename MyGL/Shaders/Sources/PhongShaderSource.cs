namespace MyGL
{
    public sealed class PhongShaderSource : IShaderSource
    {
        private const string vertexShaderSource = @"
            #version 330 core

            layout (location = 0) in vec3 aPosition;
            layout (location = 1) in vec3 aColor;
            layout (location = 2) in vec3 aNormal;
            

            uniform mat4 model;
            uniform mat4 view;
            uniform mat4 projection;
            
            varying vec3 FragPos;
            varying vec3 Color;
            varying vec3 Normal;
            
            void main()
            {
                gl_Position = projection * view * model * vec4(aPosition, 1.0);
                Color = aColor;
                Normal = aNormal * mat3(transpose(inverse(model)));
                FragPos = vec3(model * vec4(aPosition, 1.0));
            }
        ";

        private const string fragmentShaderSource = @"
            #version 330 core

            struct Material 
            {
                vec3 ambient;
                vec3 diffuse;
                vec3 specular;
                float shininess;
            };

            struct Light 
            {
                vec3 ambient;
                vec3 diffuse;
                vec3 specular;
                vec3 position;

                // attenuation
                float constant;
                float linear;
                float quadratic;

                // for spotlight
                float isSpotlight;
                vec3 direction;
                float cutOff;
                float outerCutOff;
            };

            varying vec3 Color; 
            varying vec3 Normal;
            varying vec3 FragPos;

            uniform Material material;
            uniform Light light;
            uniform vec3 viewPos;

            void main()
            {
                vec3 ambient = light.ambient * material.ambient;

                vec3 norm = normalize(Normal);
                vec3 lightDir = normalize(light.position - FragPos);
                float diff = max(dot(norm, lightDir), 0.0);
                vec3 diffuse = light.diffuse * (diff * material.diffuse);       

                vec3 viewDir = normalize(viewPos - FragPos);
                vec3 reflectDir = reflect(-lightDir, norm);
                float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
                vec3 specular = light.specular * (spec * material.specular);

                float theta = dot(lightDir, normalize(-light.direction));
                float epsilon = light.cutOff - light.outerCutOff;
                float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

                float distance = length(light.position - FragPos);
                float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

                if(light.isSpotlight > 0.0)
                {
                    diffuse  *= intensity;
                    specular *= intensity;
                }

                ambient  *= attenuation;
                diffuse  *= attenuation;
                specular *= attenuation;
                    
                vec3 result = (ambient + diffuse + specular) * Color;

                gl_FragColor = vec4(result, 1.0f);
            }
        ";

        public string VertexShaderSource => vertexShaderSource;
        public string FragmentShaderSource => fragmentShaderSource;
    }
}
