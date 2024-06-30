namespace CodeRacerApi.Models.SQLite;

public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public List<Lobby> Lobbys { get; } = [];
}