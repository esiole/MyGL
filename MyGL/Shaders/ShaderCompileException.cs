using System;

namespace MyGL
{
    public sealed class ShaderCompileException : Exception
    {
        public ShaderCompileException(string message) : base(message) { }
    }
}
