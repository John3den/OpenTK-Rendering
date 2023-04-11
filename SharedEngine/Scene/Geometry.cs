
using System.Globalization;

namespace Engine
{
    public struct Geometry
    {
        private const int N_OF_VERTICES_WHOLE = 36;
        private const int N_OF_ATTRIBUTES = 6;
        private const int N_OF_VERTICES_SPLIT = 72;
        private const int N_OF_INDICES_SPLIT = 108;
        public readonly uint[] indices;
        public readonly float[] vertices;
        public void LoadGeometry(float[] array,string path)
        {
            string[] lines = File.ReadAllLines(path);
            int i = 0;
            foreach (string line in lines)
            {
                string currentFloat = "";
                for (int counter = 0; counter < line.Length; counter++)
                {

                    if (line[counter] == 'f')
                    {
                        continue;
                    }
                    else
                    if (line[counter] == ',')
                    {
                        float number = Convert.ToSingle(currentFloat, CultureInfo.InvariantCulture);
                        currentFloat = "";
                        array[i++] = number;
                    }
                    else if (line[counter] != ' ')
                    {
                        currentFloat += line[counter];
                    }

                }
            }
        }
        public Geometry(int type)
        {
            vertices = type == 0 ? new float[N_OF_VERTICES_WHOLE * N_OF_ATTRIBUTES] : new float[N_OF_VERTICES_SPLIT * N_OF_ATTRIBUTES];
            indices = type == 0 ? new uint[N_OF_VERTICES_WHOLE] : new uint[N_OF_INDICES_SPLIT];
            if (type == 1)
            for (uint i = 0; i < N_OF_INDICES_SPLIT / N_OF_ATTRIBUTES; i++)
            {
                uint offset = 6 * i;
                indices[offset] = 4 * i;
                indices[offset + 1] = 4 * i + 1;
                indices[offset + 2] = 4 * i + 2;
                indices[offset + 3] = 4 * i;
                indices[offset + 4] = 4 * i + 2;
                indices[offset + 5] = 4 * i + 3;
            }
            else
            {
                for (uint i = 0; i < N_OF_VERTICES_WHOLE; i++)
                {
                    indices[i] = i;
                }
            }
            string file = type == 0 ? "NormalCube.geometry" : "SplitCube.geometry";
            LoadGeometry(vertices, "../../../Resources\\Geometries\\" + file);
        }
    }
}
