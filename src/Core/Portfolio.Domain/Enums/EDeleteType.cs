using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Portfolio.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EDeleteType
{
    [Display(Name = "Hard Delete")]
    Hard,
    [Display(Name = "Soft Delete")]
    Soft,
    [Display(Name = "Reverse Delete")]
    Reverse
}