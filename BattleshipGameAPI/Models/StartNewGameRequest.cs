namespace BattleshipGameAPI.Models
{
    public class StartNewGameRequest
    {
        public string FirstPlayerName { get; set; }
        public string SecondPlayerName { get; set; }
        public int XSize { get; set; }
        public int YSize { get; set; }
    }
}
