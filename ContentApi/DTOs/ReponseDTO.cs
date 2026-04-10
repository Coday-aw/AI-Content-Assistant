namespace K4U2.DTOs;

public record ResponseDto
(
    int Id,
    string Message,
    string Response,
    string Category,
    DateTime CreatedAt
);