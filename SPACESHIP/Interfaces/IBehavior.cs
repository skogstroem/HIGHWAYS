namespace HIGHWAYS.Interfaces;
// KRAV #3:
// 1: Bridge Pattern.
// 2: I programmet har vi 2 abstraktioner; IBehavior och IRender, dessa med tillhörande konkretioner.
// I våra konkretioner av IBehavior så använder vi av konkretionerna från IRender för att samtidigt rendera ut effekter beroende på beteende.
// 3: Genom att separera beteenden och rendering i olika hierarkier kan vi enkelt lägga till nya beteenden eller renderingseffekter utan att påverka den andra hierarkin.
// Detta minskar beroenden och gör koden mer modulär och lättare att underhålla.
public interface IBehavior
{
    void OnCollision(IPlayer player);
    void RenderEffect(int x, int y);
}

