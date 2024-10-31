using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;
public sealed class User : IdentityUser
{
	public string Name { get; set; } = null!;
	public string? RefreshToken { get; set; }
	public DateTime RefreshTokenExpiryTime { get; set; }


	public User(string name, string user, string email)
	{
		Name = name;
		UserName = user;
		Email = email;
	}

	public static User Create(string name, string user, string email)
	{
		return new User(name, user, email);
	}

	public void UpdateRefreshToken(string token, DateTime expiryTime)
	{
		RefreshToken = token;
		RefreshTokenExpiryTime = expiryTime;
	}
	private User() { }
}
