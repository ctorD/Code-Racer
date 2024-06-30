using System.ComponentModel.DataAnnotations.Schema;

namespace CodeRacerApi.Models.SQLite;

public class Lobby
{
    public int Id { get; set; } 
    public string LobbyName { get; set; } 
    public DateTime CreationTime { get; set; } = DateTime.Now;
    public string Snippet { get; set; }
    public List<User> Users { get; } = [];
    
}