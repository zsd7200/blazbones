namespace BlazBones.Services
{
    public class ConnectedPlayer
    {
        public ConnectedPlayer(string connectionId, string username) {
            ConnectionId = connectionId;
            Username = username;
        }

        public string ConnectionId { get; set; } = "";
        public string Username { get; set; } = "";
    }
}