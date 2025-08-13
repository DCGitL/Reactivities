using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class ForgotPassWordDto
{
    [Required]
    public required string Email { get; set; }

}
