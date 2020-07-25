using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGL
{
    public sealed class UniversalShader : IShaderSource
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
        private string fragmentShaderSource = "";
        /*
        private string fragmentShaderSource = string.Format(@"
            #version 330 core

            struct Material 
            {
                vec3 ambient;
                vec3 diffuse;
                vec3 specular;
                float shininess;
            };

            struct DirLight
            {
                vec3 ambient;
                vec3 diffuse;
                vec3 specular;
                vec3 direction;
            };

            struct PointLight
            {
                vec3 ambient;
                vec3 diffuse;
                vec3 specular;
                vec3 direction;
                vec3 position;
                float constant;
                float linear;
                float quadratic;
            };

            struct SpotLight
            {
                vec3 ambient;
                vec3 diffuse;
                vec3 specular;
                vec3 direction;
                vec3 position;
                float cutOff;
                float outerCutOff;
                float constant;
                float linear;
                float quadratic;
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

            uniform DirLight dirLight;
            uniform PointLight pointLights[{0}];
            uniform SpotLight spotLights[{1}];

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
        ", 1, 1);
        */
        public string VertexShaderSource => vertexShaderSource;
        public string FragmentShaderSource => fragmentShaderSource;

        //public int CountPointLight { get; set; }
        //public int CountSpotLight { get; set; }

        public UniversalShader(int countPointLight, int countSpotLight)
        {
            //CountPointLight = countPointLight;
            //CountSpotLight = countSpotLight;
            //string.Format("{0}", countSpotLight);

            //if (countPointLight == 0)
            //{
            //    countPointLight++;
            //}
            //if (countSpotLight == 0)
            //{
            //    countSpotLight++;
            //}

            fragmentShaderSource = $@"
            #version 330 core

            struct Material 
            {{
                vec3 ambient;
                vec3 diffuse;
                vec3 specular;
                float shininess;
            }};

            struct DirLight
            {{
                vec3 ambient;
                vec3 diffuse;
                vec3 specular;
                vec3 direction;
            }};

            struct PointLight
            {{
                vec3 ambient;
                vec3 diffuse;
                vec3 specular;
                vec3 direction;
                vec3 position;
                float constant;
                float linear;
                float quadratic;
            }};

            struct SpotLight
            {{
                vec3 ambient;
                vec3 diffuse;
                vec3 specular;
                vec3 direction;
                vec3 position;
                float cutOff;
                float outerCutOff;
                float constant;
                float linear;
                float quadratic;
            }};

            varying vec3 Color; 
            varying vec3 Normal;
            varying vec3 FragPos;

            uniform DirLight dirLight;
            uniform PointLight pointLights[{countPointLight}];
            uniform SpotLight spotLights[{countSpotLight}];

            uniform Material material;
            uniform vec3 viewPos;

            vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir)
            {{
                vec3 lightDir = normalize(-light.direction);
                float diff = max(dot(normal, lightDir), 0.0);
                vec3 reflectDir = reflect(-lightDir, normal);
                float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
                vec3 ambient = light.ambient * material.ambient;
                vec3 diffuse = light.diffuse * (diff * material.diffuse);
                vec3 specular = light.specular * (spec * material.specular);
                return (ambient + diffuse + specular);
            }}

            vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
            {{
                vec3 lightDir = normalize(light.position - fragPos);
                float diff = max(dot(normal, lightDir), 0.0);
                vec3 reflectDir = reflect(-lightDir, normal);
                float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
                float distance = length(light.position - fragPos);
                float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
                vec3 ambient = light.ambient * material.ambient;
                vec3 diffuse = light.diffuse * (diff * material.diffuse);
                vec3 specular = light.specular * (spec * material.specular);
                ambient *= attenuation;
                diffuse *= attenuation;
                specular *= attenuation;
                return (ambient + diffuse + specular);
            }} 

            vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
            {{
                vec3 lightDir = normalize(light.position - FragPos);
                float diff = max(dot(normal, lightDir), 0.0);
                vec3 reflectDir = reflect(-lightDir, normal);
                float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
                float distance = length(light.position - FragPos);
                float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
                float theta = dot(lightDir, normalize(-light.direction));
                float epsilon = light.cutOff - light.outerCutOff;
                float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);
                vec3 ambient = light.ambient * material.ambient;
                vec3 diffuse = light.diffuse * (diff * material.diffuse);
                vec3 specular = light.specular * (spec * material.specular);
                ambient *= attenuation;
                diffuse *= attenuation * intensity;
                specular *= attenuation * intensity;
                return (ambient + diffuse + specular);
            }}

            void main()
            {{
                vec3 norm = normalize(Normal);
                vec3 viewDir = normalize(viewPos - FragPos);


                vec3 result = CalcDirLight(dirLight, norm, viewDir);
                //vec3 result = vec3(1.0f, 1.0f, 1.0f);


                for(int i = 0; i < {countPointLight}; i++)
                    result += CalcPointLight(pointLights[i], norm, FragPos, viewDir);
                for(int i = 0; i < {countSpotLight}; i++)
                    result += CalcSpotLight(spotLights[i], norm, FragPos, viewDir);
                
                result *= Color;
                gl_FragColor = vec4(result, 1.0f);
            }}
            ";

            if (countPointLight == 0)
            {
                fragmentShaderSource = fragmentShaderSource.Replace("uniform PointLight pointLights[0];", "");
                fragmentShaderSource = fragmentShaderSource.Replace("for(int i = 0; i < 0; i++)", "");
                fragmentShaderSource = fragmentShaderSource.Replace("result += CalcPointLight(pointLights[i], norm, FragPos, viewDir);", "");
            }

            if (countSpotLight == 0)
            {
                fragmentShaderSource = fragmentShaderSource.Replace("uniform SpotLight spotLights[0];", "");
                fragmentShaderSource = fragmentShaderSource.Replace("for(int i = 0; i < 0; i++)", "");
                fragmentShaderSource = fragmentShaderSource.Replace("result += CalcSpotLight(spotLights[i], norm, FragPos, viewDir);", "");
            }

            /*
            fragmentShaderSource = string.Format(@"
            #version 330 core

            struct Material 
            {
                vec3 ambient;
                vec3 diffuse;
                vec3 specular;
                float shininess;
            };

            struct DirLight
            {
                vec3 ambient;
                vec3 diffuse;
                vec3 specular;
                vec3 direction;
            };

            struct PointLight
            {
                vec3 ambient;
                vec3 diffuse;
                vec3 specular;
                vec3 direction;
                vec3 position;
                float constant;
                float linear;
                float quadratic;
            };

            struct SpotLight
            {
                vec3 ambient;
                vec3 diffuse;
                vec3 specular;
                vec3 direction;
                vec3 position;
                float cutOff;
                float outerCutOff;
                float constant;
                float linear;
                float quadratic;
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

            uniform DirLight dirLight;
            uniform PointLight pointLights[{0}];
            uniform SpotLight spotLights[{1}];

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
            ", countPointLight, countSpotLight);
            */
        }
    }
}
