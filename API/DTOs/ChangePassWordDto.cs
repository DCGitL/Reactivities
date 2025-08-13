using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class ChangePassWordDto
{
    [Required]
    public required string newPassword { get; set; }

    [Required]
    public required string currentPassword { get; set; }

}
