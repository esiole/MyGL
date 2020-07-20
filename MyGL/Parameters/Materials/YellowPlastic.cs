using OpenTK;

namespace MyGL
{
    public sealed class YellowPlastic : Material
    {
        public YellowPlastic() : base(
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.5f, 0.5f, 0.0f),
            new Vector3(0.60f, 0.60f, 0.50f),
            32.0f)
        { }
    }
}
