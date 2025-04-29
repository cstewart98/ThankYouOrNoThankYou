#pragma warning disable CS8616
using System.ComponentModel.DataAnnotations;
namespace ThankYouOrNoThankYou.Models;

public class Contestant
{
    [Key]
    public int ContestantId { get; set; }

    [Required(ErrorMessage ="Name is required!")]
    [MinLength(2,ErrorMessage ="Name must be 2+ characters long!")]
    public string Name { get; set; }

    public int CashValue { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}