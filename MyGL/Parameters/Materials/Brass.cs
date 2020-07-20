using OpenTK;

namespace MyGL
{
    public sealed class Brass : Material
    {
        public Brass() : base(
            new Vector3(0.329412f, 0.223529f, 0.027451f),
            new Vector3(0.780392f, 0.568627f, 0.113725f),
            new Vector3(0.992157f, 0.941176f, 0.807843f),
            32.0f)
        { }
    }
}
