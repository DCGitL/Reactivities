using System;
using System.ComponentModel.DataAnnotations;
using FluentEmail.Core;

namespace API.DTOs;

public class ResetPassWordDto
{
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string Token { get; set; }
    [Required]
    public required string NewPassword { get; set; }
}
