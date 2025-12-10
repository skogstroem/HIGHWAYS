using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIGHWAYS.FileManager;


// KRAV #3:
// 1: Bridge Pattern.
//
// 2: Bridge Pattern används i vårat program för att separera hur Scoreboard presenterar data från hur den datan faktiskt sparas och hämtas. De två abstraiktionerna är Scoreboard och IFile.
//    Scoreboard har två konkreta implementationer: NormalScoreboard och AdvancedScoreboard, dessa ansvarar för hur resultaten presenteras
//    IFile har två konkreta implementationer: FileTxt och FileCSV, som ansvarar för hur data sparas och läses beroende på filformat 
//    
//    Scoreboard innehåller en referens (objektkomposition / has-a) till en IFile, vilket gör att scoreboarden kan hämta data oberoende av vilket filformat som används. På så sätt kan 
//    filen bytas vid runtime utan att påverka scoreboard klasserna. 
//    
//    Scoreboards subtyper skiljer i beteenden och inte bara data eftersom de har olika impletnationer av Draw(), vilket betyder att de visualiserar data på olika sätt 
//    IFiles subtyper skiljer sig i beteenden och inte bara data eftersom hur man sparar samt hämtar från olika typer av filer varierar, alla subtyper har olika impletationer av 
//    Save() och Fetch(). 
//
// 3: Motiveringen till att vi använder Bridge Pattern är för att möjliggöra hög flexibilitet och låg koppling (decoupling) mellan presentationen av resultat (Scoreboard) och lagring 
//    av data (IFile). Utan detta mönstret hade vi behövt hårdkoda ett visst filformat, exemplevis CSV, vilket hade gjort systemet svårare att underhålla, utöka och exportera data. Med IFile kan vi exempelvis 
//   hämta information från en databas om vi väljer att lägga till det i framtiden. Scoreboardsen ska inte behöva bry sig över hur data lagras, bara att den kan få datan som ska presenteras. 

namespace HIGHWAYS.Interfaces
{
    public abstract class ScoreBoard
    {

        protected IFile _file;
        
        public virtual void Draw() {}

        public void Save (IPlayer player)
        {
            _file.Save(player);
        }
    }
}
