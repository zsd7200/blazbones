using BlazBones.Services;
using Microsoft.AspNetCore.SignalR;

namespace BlazBones.Hubs
{
    public class RoomHub : Hub
    {
        
        private static readonly Dictionary<string, List<ConnectedPlayer>> Rooms = [];

        private string generateRandomRoomCode() {
            string guid = Guid.NewGuid().ToString("N");
            return guid.Substring(0, 5);
        }

        public async Task CreateRoom(string user) {
            string roomCode = generateRandomRoomCode();
            await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);
            Rooms[roomCode] = [new ConnectedPlayer(Context.ConnectionId, user)];
            await Clients.Caller.SendAsync("CreatedRoom", roomCode);
        }

        public async Task JoinRoom(string roomCode, string user) {
            if (!Rooms.ContainsKey(roomCode)) {
                await Clients.Caller.SendAsync("JoinFailed", "Room not found.");
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);
            Rooms[roomCode].Add(new ConnectedPlayer(Context.ConnectionId, user));

            await Clients.Group(roomCode).SendAsync("UserJoined", user, Rooms[roomCode]);
            await Clients.Caller.SendAsync("JoinedRoom", roomCode);
        }

        public override async Task OnDisconnectedAsync(Exception? exception) {
            List<string> roomsToDelete = [];

            foreach (var room in Rooms) {
                ConnectedPlayer? foundPlayer = room.Value.FirstOrDefault(player => player.ConnectionId == Context.ConnectionId);
                if (foundPlayer != null) {
                    await Clients.Group(room.Key).SendAsync("UserLeft", foundPlayer.Username);
                    room.Value.Remove(foundPlayer);
                }

                if (room.Value.Count == 0) {
                    roomsToDelete.Add(room.Key);
                }
            }

            foreach (string roomKey in roomsToDelete) {
                Rooms.Remove(roomKey);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task CreateGame(string roomCode) {
            await Clients.Group(roomCode).SendAsync("CreatedGame", roomCode);
        }

        public async Task GetRoomData(string roomCode) {
            await Clients.Group(roomCode).SendAsync("ReceiveRoomData", Rooms[roomCode]);
        }

        public async Task SendMessage(string roomCode, string user, string message) {
            await Clients.Group(roomCode).SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendPlayerSingleton(string roomCode, PlayerSingleton playerSingleton) {
            await Clients.Group(roomCode).SendAsync("RecievePlayerSingleton", playerSingleton);
        }

        public async Task SendDiceState(string roomCode, int[] diceCount) {
            await Clients.Group(roomCode).SendAsync("ReceiveDiceState", diceCount);
        }

        public async Task SendScoreUpdate(string roomCode, int score, string user) {
            await Clients.Group(roomCode).SendAsync("ReceiveScoreUpdate", score, user);
        }
    }
}
