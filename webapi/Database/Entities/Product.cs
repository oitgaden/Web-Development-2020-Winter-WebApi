using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("product")]
public class Product
{
    [Column("product_id")]
    [Key]
    public long ProductId {get; set;}

    [Column("name")]
    public string Name {get; set;}

    [Column("price")]
    public decimal Price {get; set;}

    [Column("count")]
    public int Count {get; set;}
}