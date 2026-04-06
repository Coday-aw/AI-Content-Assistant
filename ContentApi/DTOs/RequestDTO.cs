using System.ComponentModel.DataAnnotations;

namespace K4U2.DTOs;

public record RequestDto
{
    [Required(ErrorMessage = "Message is required")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Message must be at least 1 character long")]
    public string Message { get; set; } =  string.Empty;
    [Required(ErrorMessage = "Category is required")]
    public string Category { get; set; } = string.Empty;
}