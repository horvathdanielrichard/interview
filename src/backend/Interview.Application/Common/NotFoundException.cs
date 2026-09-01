namespace Interview.Application.Common;

/// <summary>Thrown when a requested entity cannot be found.</summary>
public class NotFoundException(string message) : Exception(message);
