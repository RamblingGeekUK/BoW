// See https://aka.ms/new-console-template for more information
using Postgrest.Attributes;
using Postgrest.Models;

[Table("phrases")]
class Phrases : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("created_at")]
    public DateTime created_at { get; set; }

    [Column("phrase")]
    public string? phrase { get; set; }


}
