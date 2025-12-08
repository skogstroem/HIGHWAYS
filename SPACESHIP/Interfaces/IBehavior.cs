using HIGHWAYS.GameObjects;

namespace HIGHWAYS.Interfaces;
// KRAV #3:
// 1: Bridge Pattern.
// 2: I programmet har vi 2 abstraktioner; IBehavior och IRender, dessa med tillh�rande konkretioner.
// I v�ra konkretioner av IBehavior s� anv�nder vi av konkretionerna fr�n IRender f�r att samtidigt rendera ut effekter beroende p� beteende.
// 3: Genom att separera beteenden och rendering i olika hierarkier kan vi enkelt l�gga till nya beteenden eller renderingseffekter utan att p�verka den andra hierarkin.
// Detta minskar beroenden och g�r koden mer modul�r och l�ttare att underh�lla.
public interface IBehavior
{
    void OnCollision(IPlayer player);
    void RenderEffect(int x, int y);
    
}

