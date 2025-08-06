using System;
using System.ComponentModel.DataAnnotations;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json;

namespace API.DTOs;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [MinLength(8, ErrorMessage = "Min characters are 8")]

    public required string Password { get; set; }

}
