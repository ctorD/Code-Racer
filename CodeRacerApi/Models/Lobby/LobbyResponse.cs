namespace CodeRacerApi.Models.Lobby;

public class LobbyResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<string> Users { get; set; }
}