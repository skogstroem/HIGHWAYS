using System.Text.RegularExpressions;
using HIGHWAYS.Interfaces;
using HIGHWAYS.Players;

namespace HIGHWAYS.FileManager;

public class FileTxt : IFile
{
    public string Path { get; set; }

    public FileTxt()
    {
        Path = "highway.txt";
        Exists();
    }
    
    public void Save(IPlayer player)
    {
        try
        {
            using var writer = new StreamWriter(Path, true);
            writer.WriteLine($"Name:  {player.HighscoreName} Score: {player.Score} Power-Ups: {player.NumberOfPowersUps} Difficulty: {player.Difficulty}");
        }
        catch (Exception ex)
        {
            Console.Write($"An error occured while writing to the txt. file {ex.Message}");
        }
    }

    public List<IPlayer> Fetch()
    {
        
        var players = new List<IPlayer>();
        
        string pattern =
            @"Name:\s*([\p{L} '-]+)\s*" +
            @"Score:\s*([-−]?\d+)\s*" +
            @"Power-Ups:\s*([-−]?\d+)\s*" +
            @"Difficulty:\s*([-−]?\d+)";       
        
        try
        {
            var lines = File.ReadAllLines(Path);

            foreach (var line in lines)
            {
                
                var match = Regex.Match(line, pattern);

                if (!match.Success)
                    continue;

                players.Add(new Player(
                    match.Groups[1].Value,               
                    int.Parse(match.Groups[2].Value),    
                    int.Parse(match.Groups[3].Value),     
                    int.Parse(match.Groups[4].Value)       
                ));
            }
        }
        catch (Exception ex)
        {
            Console.Write($"An error occured while reading the txt. file {ex.Message}");
        }
        
        return players;
    }

    public void Exists()
    {
        if (File.Exists(Path))
            return;

        using (File.Create((Path))) ;
    }
}