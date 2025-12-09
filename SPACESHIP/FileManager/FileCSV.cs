using HIGHWAYS.Interfaces;
using HIGHWAYS.Players;

namespace HIGHWAYS.FileManager;

public class FileCSV : IFile
{
    public string Path { get; set; }
    
    public FileCSV()
    {
        Console.WriteLine("FileCSV constructor called");
        Path = "highway.csv";
        Exists();
    }
    
    public void Save (IPlayer player)
    {
        
        string line = $"{player.HighscoreName},{player.Score},{player.NumberOfPowersUps},{player.Difficulty}";
        
        try
        {
            File.AppendAllText(Path, line + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.Write($"An error occured while writing to the CSV file {ex.Message}");
        }
    }
    
    public List <IPlayer> Fetch()
    {
        
        var players = new List<IPlayer>();
        
        try
        {
            var lines = File.ReadAllLines(Path);

            foreach (var line in lines)
            {
                var values = line.Split(','); // Oskar (namn), 20 (Powerups), 2103 (Score), 1 (Difficulty) 
                
                players.Add(new Player(
                    values[0],               
                    int.Parse(values[1]),      
                    int.Parse(values[2]),     
                    int.Parse(values[3])    
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

