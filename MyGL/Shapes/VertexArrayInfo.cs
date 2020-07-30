using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MyGL
{
    /// <summary>
    /// Представляет собой объект, который объединяет атрибуты, присущие одному массиву вершин.
    /// </summary>
    public class VertexArrayInfo
    {
        /// <summary>
        /// Количество вершин.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Каким примитивом отрисовывать.
        /// </summary>
        public PrimitiveType DrawType { get; private set; }

        /// <summary>
        /// Массив координат вершин.
        /// </summary>
        public Vector3[] Coord { get; private set; }

        /// <summary>
        /// Массив нормалей вершин.
        /// </summary>
        public Vector3[] Normals { get; private set; }

        /// <summary>
        /// Создаёт объект, объединяющий атрибуты вершин.
        /// </summary>
        /// <param name="coord">Массив координат вершин.</param>
        /// <param name="normals">Массив нормалей вершин.</param>
        /// <param name="drawType">Каким примитивом отрисоывать вершины.</param>
        public VertexArrayInfo(Vector3[] coord, Vector3[] normals, PrimitiveType drawType)
        {
            if (coord.Length != normals.Length)
            {
                throw new ArgumentException("Массивы характеристик вершин должны быть одного размера.");
            }
            Count = coord.Length;
            DrawType = drawType;
            Coord = coord;
            Normals = normals;
        }

        /// <summary>
        /// Возвращает все атрибуты вершин.
        /// </summary>
        public IEnumerable<Vector3[]> Properties
        {
            get
            {
                yield return Coord;
                yield return Normals;
            }
        }
    }
}
