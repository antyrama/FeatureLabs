using System.ComponentModel.DataAnnotations;
using FeatureLabs.AppServices.Configuration.Attributes;

namespace FeatureLabs.AppServices.Configuration.Options;

[OptionsPosition("AzureAdB2C")]
public class AzureB2COptions
{
    [Required]
    public string Instance { get; set; } = default!;

    [Required]
    public string Domain { get; set; } = default!;

    [Required]
    public string ClientId { get; set; } = default!;

    [Required]
    public string ClientSecret { get; set; } = default!;

    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = default!;
}
