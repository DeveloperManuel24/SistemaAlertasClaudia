using Microsoft.AspNetCore.Identity;

public class Token
{
    public int Id { get; set; }
    public string TokenValue { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public IdentityUser User { get; set; } = null!;
    public DateTime Expiration { get; set; } = DateTime.UtcNow.AddHours(2); // Token expira en 2 horas
}
