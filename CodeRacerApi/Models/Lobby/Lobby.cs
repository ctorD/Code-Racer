namespace CodeRacerApi.Models.Lobby;

public class Lobby
{
    public int Id { get; set; }
    public string Snippet { get; set; }
    public string Lang { get; set; }
    public string[] Users { get; set; }
}