using OpenTK;

namespace MyGL
{
    public sealed class Bronze : Material
    {
        public Bronze() : base(
            new Vector3(0.2125f, 0.1275f, 0.054f), 
            new Vector3(0.714f, 0.4284f, 0.18144f),
            new Vector3(0.393548f, 0.271906f, 0.166721f),
            32.0f) 
        { }
    }
}
