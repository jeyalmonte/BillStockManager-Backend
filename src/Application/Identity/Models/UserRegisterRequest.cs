namespace Application.Identity.Models;
public class UserRegisterRequest(string name, string username, string email, string password)
	: UserRequest(username, password)
{
	public string Name { get; } = name;
	public string Email { get; } = email;
}