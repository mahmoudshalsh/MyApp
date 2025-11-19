namespace MyApp.ApplicationServices.Dtos;

public record ProductDto(int Id, string Name, decimal Price, string? Category);
public record CreateProductDto(int Id, string Name, decimal Price, int CategoryId);
