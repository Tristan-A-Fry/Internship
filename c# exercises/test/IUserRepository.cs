	public interface IUserRepository
	{
		AuthenticationResult Authenticate(string userName, string password);
	}
	
	public class UserRepository : IUserRepository
	{
		public AuthenticationResult Authenticate(string userName, string password)
		{
			var result = new AuthenticationResult();
			
			var userInfo = GetUsers().FirstOrDefault(x => x.Username == userName
													   && x.Password == password);
			
			if(userInfo == null)
			{
				return result;
			}
		 	Console.WriteLine($"User '{userName}' authenticated with roles: {string.Join(", ", userInfo.Roles)}");
	
			result.IsSuccess = true;
			result.Roles = userInfo.Roles;
			
			return result;
		}
		
		private List<UserInfo> GetUsers()
			=> new List<UserInfo>()
				{
					new UserInfo()
					{
						Username = "jdoe",
						Password = "SB[=D?W_K7Fb",
						Roles = { Roles.Admin }
					},
					new UserInfo()
					{
						Username = "pmaybelle",
						Password = "DRHa]DR:5BJ]",
						Roles = { Roles.Regular }
					}
				};
		
		private class UserInfo
		{
			public required string Username { get; set; }
			public required string Password { get; set; }
			public List<string> Roles { get; set;} = new List<string>();
		}
	}

	public class AuthenticationResult
	{
		public bool IsSuccess {get; set;} = false;
		public List<string> Roles { get; set;} = new List<string>();
	}
	
	public class Roles
	{
		public const string Admin = "Admin";
		public const string Regular = "Regular";
	}