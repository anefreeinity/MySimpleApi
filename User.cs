using System.ComponentModel.DataAnnotations.Schema;

[Table("users")]
public class User
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;
}