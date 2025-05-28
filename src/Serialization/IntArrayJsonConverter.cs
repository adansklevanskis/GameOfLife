using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameOfLife.Serialization;

public class IntArrayJsonConverter : JsonConverter<int[,]>
{
    public override int[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var listGrid = JsonSerializer.Deserialize<List<List<int>>>(ref reader, options);

        int rowCount = listGrid.Count;
        int columnCount = listGrid[0].Count;
        int[,] cellGrid = new int[rowCount, columnCount];

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                cellGrid[i, j] = listGrid[i][j];
            }
        }

        return cellGrid;
    }

    public override void Write(Utf8JsonWriter writer, int[,] value, JsonSerializerOptions options)
    {
        int rowCount = value.GetLength(0);
        int columnCount = value.GetLength(1);
        var listGrid = new List<List<int>>();

        for (int i = 0; i < rowCount; i++)
        {
            var rowList = new List<int>();
            for (int j = 0; j < columnCount; j++)
            {
                rowList.Add(value[i, j]);
            }
            listGrid.Add(rowList);
        }

        JsonSerializer.Serialize(writer, listGrid, options);
    }
}