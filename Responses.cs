// See https://aka.ms/new-console-template for more information
using Postgrest.Attributes;
using Postgrest.Models;

[Table("responses")]
class Responses : BaseModel
{

    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("created_at")]
    public DateTime created_at { get; set; }

    [Column("response")]
    public string? response { get; set; }
}