
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace PCLLib
{
    public class TestData
    {
        public static void CreateTestData(out int vertexPointer, out int colorPointer, out int verticesLength)
        {
            int CloudSize = 30;

            Vector3d[] vertices = TestData.CreateTestList<Vertex>(CloudSize);
            vertexPointer = TestData.CreateVBO(vertices);
            verticesLength = vertices.Length;
            byte[] colors = TestData.CreateColors(verticesLength);
            colorPointer = TestData.LoadColorsToVBO(colors);
            
        }
        public static int LoadColorsToVBO(byte[] colors)
        {
            int colorPointer = 0;

            GL.GenBuffers(1, out colorPointer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, colorPointer);
            GL.BufferData(BufferTarget.ArrayBuffer,
                          new IntPtr(colors.Length * BlittableValueType.StrideOf(colors)),
                          colors, BufferUsageHint.StaticDraw);
            return colorPointer;

        }
        public static int CreateVBO(Vector3d[] vertices)
        {
            int vertexPointer;

            // Load those vertex coordinates into a VBO
            int verticesLength = vertices.Length; // Necessary for rendering later on
            GL.GenBuffers(1, out vertexPointer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexPointer);
            GL.BufferData(BufferTarget.ArrayBuffer,
                          new IntPtr(vertices.Length * BlittableValueType.StrideOf(vertices)),
                          vertices, BufferUsageHint.StaticDraw);
            return vertexPointer;

        }
        public static Vector3d[] CreateTestList<Vertex>(int myCloudSize)
        {
            // Imagine that the cloud is a bool[CloudSize, CloudSize, CloudSize] array.
            // This code translates the point cloud into vertex coordinates
            Vector3d[] vertices = new Vector3d[myCloudSize * myCloudSize * myCloudSize];
            int index = 0;
            for (int i = 0; i < myCloudSize; i++)
                for (int j = 0; j < myCloudSize; j++)
                    for (int k = 0; k < myCloudSize; k++)
                        if (Math.Sqrt(i * i + j * j + k * k) < myCloudSize) // Point cloud shaped like a sphere
                        {
                            vertices[index++] = new Vector3d(
                                -myCloudSize / 2 + i,
                                -myCloudSize / 2 + j,
                                -myCloudSize / 2 + k);
                        }

            return vertices;
        }
        public static byte[] CreateColors(int myverticesLength)
        {

            byte[] colors = new byte[myverticesLength * 3];
            int indexColor = 0;
            for (int i = 0; i < myverticesLength; i++)
            {
                colors[indexColor] = 255;
                colors[indexColor + 1] = 0;
                colors[indexColor + 2] = 0;
                indexColor += 3;

            }

            return colors;
        }
    }
}
