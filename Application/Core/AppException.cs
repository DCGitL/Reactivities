using System;

namespace Application.Core;

public record AppException(int statusCode, string message, string? details);

