namespace K4U2.DTOs;

public record ResponseDto
(
    string Message,
    string Response,
    string Category,
    DateTime CreatedAt
);