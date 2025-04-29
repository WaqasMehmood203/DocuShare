namespace DMS.Backend.API.Service.Dtos.Login
{
    public class LoginResponseDto
    {
        public string JwtToken { get; set; }  // JWT Token property
        public string Username { get; set; }  // Username property
        public string Message { get; set; }   // Message property (e.g., "Login successful")
        public bool Success { get; set; }     // Success flag (true/false)
        public string UserId { get; set; }
    }
}
