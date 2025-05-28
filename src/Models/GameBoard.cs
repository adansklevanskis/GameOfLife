using GameOfLife.Serialization;
using System.Text.Json.Serialization;

namespace GameOfLife.Models;

public class GameBoard
{
    public int BoardId { get; set; }
    [JsonConverter(typeof(IntArrayJsonConverter))]

    public int[,] CellGrid { get; set; }

}