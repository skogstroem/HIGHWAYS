using HIGHWAYS.Interfaces;

namespace HIGHWAYS.FileManager;

public interface IFile
{
    public string Path { get; set; }
    
    void Save(IPlayer player);
    List <IPlayer> Fetch();
    
    void Exists();
    
}