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
            
            writer.WriteLine($"Player Name:  {player.Name}");
            writer.WriteLine($"Player Score: {player.Score}");
            writer.WriteLine();
        }
        catch (Exception ex)
        {
            Console.Write($"An error occured while writing to the CSV file {ex.Message}");
        }
    }

    public List<IPlayer> Fetch()
    {
        
        var players = new List<IPlayer>();
        
        try
        {
            var lines = File.ReadAllLines(Path);

            foreach (var line in lines)
            {
                var values = line.Split(','); // Name: Oskar (namn), Powerups: 20 (Powerups), Score: 2103 (Score), Difficulty: 1 (Difficulty) 
                
                players.Add(new Player(
                    values[0],
                    int.Parse(values[1])
                    
                    // Får parsa fler här 
                ));
            }
        }
        catch (Exception ex)
        {
            Console.Write($"An error occured while reading the CSV file {ex.Message}");
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