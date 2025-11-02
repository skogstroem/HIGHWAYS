HIGHWAYS
---------------
HIGHWAYS är ett konsolspel som går ut på att få så hög poäng som möjligt. Navigera din pointer(bil) i olika lanes för att undvika hinder (bilar/bomber) samtidigt som du plockar upp powerups i form av scoreboosts eller extra HP. 
Du startar spelet via kompilering och möts av "hemskärmen" av spelet. Därefter tillåts spelaren välja gamemode; SOLO eller VS BOT. Vid spel VS BOT så fortsätter spelet tills en av spelarna har slut på HP.


How It's Made:
---------------
Språk använt: C#
Spelet är framförallt byggt efter SOLID-principerna men nyttjar specifikt de 6 konceptkraven angivna i projektppgiften. Dessa är utkommenterade i koden där de tillämpas med respektive kort förklaring.


AI Usage:
---------------
AI har använts som rådgivare vid startskedet av projektet för att ta fram en plan hur vi skulle kunna få in konceptkraven i spelidén.
AI har också rådfrågats till hur vi på ett bra sätt kunnat genomföra logiken bakom den interaktiva spelplanen.
CO-pilot använts som assistent vid kodskrivandet.

CHATTENS SAMMANSTÄLLNING AV KONCEPTKRAVEN:

/// 1. GENERICS - ObjectBuffer<T> (ObjectBuffer.cs)
///    - Används med olika typ-argument: ObjectBuffer<GameObject> (Lane.cs rad 7, 15) och ObjectBuffer<Powerup> (Game.cs rad 23, 37)
///    - Constraint: where T : class
///    - Typsäker cirkulär buffer som fungerar med vilken referenstyp som helst
///    - Spårar senaste 10 powerups spelaren plockade upp och visar statistik i UI
/// 
/// 2. STRATEGY PATTERN - IMovementStrategy (IMovementStrategy.cs)
///    - Subtyper: ZigZagStrategy (sicksack-rörelse) och StraightStrategy (står stilla)
///    - Injiceras i AIPlayer.cs konstruktor (rad 19, 24)
///    - Används i AIPlayer.Update() (rad 63) för att bestämma AI:ns nästa lane
///    - Användaren väljer strategi vid runtime (Program.cs rad 54-56)
/// 
/// 3. BRIDGE PATTERN - IBehavior använder IRenderable
///    - Abstraktion 1: IBehavior med 4 konkretioner (DamageBehavior, HealBehavior, ScoreBehavior, BombBehavior)
///    - Abstraktion 2: IRenderable med 2 konkretioner (AsciiRenderer, ColoredRenderer)
///    - Alla IBehavior-konkretioner komponeras med IRenderable i konstruktorn för att visa effekter
///    - Bridge: DamageBehavior/HealBehavior/ScoreBehavior/BombBehavior använder AsciiRenderer/ColoredRenderer
///    - RenderEffect() anropas i DrawUI (Game.cs rad 342) och visar effekten i hp-baren efter kollision
///    - Exempel: HealBehavior visar '♥', ScoreBehavior visar '★', DamageBehavior visar 'X', BombBehavior visar '*'
///    - Möjliggör 8 olika kombinationer (4 behaviors × 2 renderers) utan att skapa 8 klasser
/// 
/// 4. FACTORY METHOD - IGameObjectFactory (IGameObjectFactory.cs)
///    - Fabrik-hierarki: ObstacleFactory och PowerupFactory (båda implementerar IGameObjectFactory)
///    - Produkt-hierarki 1: Obstacle med ObstacleType (Debris/Bomb) - skapas av ObstacleFactory
///    - Produkt-hierarki 2: Powerup med PowerupType (Health/Score) - skapas av PowerupFactory
///    - Injiceras i Game.cs konstruktor (rad 30, 34-35)
///    - Används i SpawnRow() (rad 118, 125) för att kapsla komplex objektskapande
///    - ObstacleFactory avgör själv: 5% chans Bomb (instant game over), annars Debris (tar 1 hjärta)
///    - PowerupFactory avgör själv: 20% chans Score powerup (dubblerar poäng), annars Health powerup (ger 1 hjärta)
/// 
/// 5. ITERATOR PATTERN - Lane (Lane.cs rad 5)
///    - Implementerar IEnumerable<GameObject>
///    - GetEnumerator() delegerar till underliggande IEnumerable (rad 55-61)
///    - Itereras i Game.CheckCollisions() (rad 175-177) och CleanupObjects() (rad 228)
///    - Kapslar ObjectBuffer och filtrerar automatiskt till aktiva objekt
/// 
/// 6. LINQ METOD-SYNTAX (Game.cs)
///    - CheckCollisions() (rad 175-178): .Where().Where().ToList() för kollisionsdetektion
///    - CleanupObjects() (rad 227-239): .SelectMany().Count() och .OfType<T>() för statistik
///    - DrawUI() (rad 364): .LastOrDefault() för att hämta senaste meddelande
///    - Simplifierar nested loops, filtrering och typ-kontroller


Rapport: Genomgång av samtliga konceptkrav i HIGHWAYS

1) Generics
Egen generisk typ: ObjectBuffer<T> i SPACESHIP/ObjectBuffer.cs
Constraint: where T : class
Använder typ-parametern i implementationen: interna fält T[] _buffer, metodparametrar TryAdd(T item), returtyper IEnumerable<T> GetAll(), Predicate<T> i RemoveWhere.
Konstruerad med olika typ-argument
SPACESHIP/Lane.cs: ObjectBuffer<GameObject> (rad 7, 15)
SPACESHIP/Game.cs: ObjectBuffer<string> (rad 23, 37)
Constraint och subtyp-kravet
Constrainten är referenstyper (class) vilket utesluter värdetyper; vi konstruerar med underklasser till object (t.ex. GameObject, string) vilket motiverar constrainten och ger typsäkerhet. Det kunde inte lika gärna ersättas med ObjectBuffer<object> utan att tappa typsäkerhet och ergonomi.
Hur används:
Lane använder ObjectBuffer<GameObject> för att lagra och hantera lane-objekt (uppdatera, rendera, filtrera, städa).
Game använder ObjectBuffer<string> för att visa meddelanden i UI (senaste händelse överst).

2) Strategy pattern (med dependency injection)
Abstraktion: IMovementStrategy i SPACESHIP/Interfaces/IMovementStrategy.cs
Konkretioner:
StraightStrategy i SPACESHIP/StraightStrategy.cs (lätt – ingen sidoförflyttning)
ZigZagStrategy i SPACESHIP/ZigZagStrategy.cs (svår – sicksackbeteende)
Injection och användning:
SPACESHIP/Program.cs: användaren väljer strategi vid runtime; instans injiceras i AIPlayer-konstruktorn.
SPACESHIP/AIPlayer.cs: strategin används i Update(...) för att besluta nästa lane.
Olika beteenden:
Strategierna skiljer sig i logik (stilla vs sicksack), inte bara data.

3) Bridge Pattern
Abstraktion A (beteende): IBehavior i SPACESHIP/Interfaces/IBehavior.cs
Metoder: OnCollision(IPlayer player) och RenderEffect(int x, int y)
Abstraktion B (rendering): IRenderable i SPACESHIP/Interfaces/IRenderable.cs
Metoder: Render(int x, int y), GetSymbol(), GetColor()
Konkretioner
IBehavior: DamageBehavior, HealBehavior, ScoreBehavior, BombBehavior i SPACESHIP/*Behavior.cs
IRenderable: AsciiRenderer, ColoredRenderer i SPACESHIP/*Renderer.cs
Komposition (det viktiga i Bridge):
Varje IBehavior tar en IRenderable i konstruktorn och anropar den i RenderEffect(...).
Var används bryggan?
SPACESHIP/GameObject.cs: HandleCollision(...) triggar kollisionslogik; RenderCollisionEffect(...) exponeras.
SPACESHIP/Game.cs: DrawUI() anropar _lastCollisionObject.RenderCollisionEffect(...) på HP-raden för att visa t.ex. “hjärta” eller “stjärna” i rätt färg vid kollision.
Effekt: Vi kan kombinera valfritt beteende med valfri renderer (4×2 kombinationer) utan klass-explosion.

4) Factory Method Pattern
Fabriks-abstraktion: IGameObjectFactory i SPACESHIP/Interfaces/IGameObjectFactory.cs med CreateGameObject(int x, int y).
Fabriks-hierarki (minst två subtyper)
ObstacleFactory i SPACESHIP/ObstacleFactory.cs
PowerupFactory i SPACESHIP/PowerupFactory.cs
Produkt-hierarki
Abstrakt produkt: GameObject i SPACESHIP/GameObject.cs
Konkreta produkter: Obstacle (Debris, Bomb) och Powerup (Health, Score)
Skillnad i beteende (inte bara data)
DamageBehavior tar 1 HP
BombBehavior nollar hjärtan
HealBehavior ger 1 HP (effekt “+<3”/grön hjärta i UI)
ScoreBehavior dubblar poäng (effekt “★”/blå stjärna i UI)
Injektion
SPACESHIP/Program.cs: fabriker skapas och injiceras i Game.
SPACESHIP/Game.cs konstruktor: tar emot fabrikerna som beroenden.
Motivering “inte lika gärna skapas utan fabrik”
Både ObstacleFactory och PowerupFactory kapslar:
Slumpmässig typfördelning (t.ex. 5% Bomb; 20% Score)
Komposition av IRenderable och IBehavior (inkl. effekt-renderer)
Konstruktionen involverar flera objekt och regler som är otympliga att duplicera på anropsplatsen.

5) Iterator pattern / IEnumerable & Enumerator
Egen itererbar typ: Lane i SPACESHIP/Lane.cs implementerar IEnumerable<GameObject>.
Delegation till underliggande enumererbar
GetEnumerator() returnerar _objects.GetAll().Where(obj => obj.IsActive).GetEnumerator();
Uppfyller kurskravet att “anropa GetEnumerator() på den underliggande” istället för att skriva egen enumerator eller yield.
Var itereras den?
SPACESHIP/Game.cs: t.ex. i CheckCollisions() skapas humanCollisions genom LINQ-kedja från humanLane (som är Lane, d.v.s. vår itererbara typ).
UpdateAll(), RenderAll() i Lane använder iteration över ObjectBuffer<GameObject>.GetAll() med LINQ-filtrering.
Motivering
Lane kapslar domänlogik (koordinatsättning, uppdatering/rendering, aktiv-filter, städning) ovanpå en cirkulär buffer (ObjectBuffer<T>) – inte direkt ersättbart med en enkel List<T> utan att tappa funktionalitet.

6) LINQ metod-syntax
Kollisionsdetektion (SPACESHIP/Game.cs):
humanLane.Where(obj => obj.IsActive).Where(obj => Math.Abs(obj.Y - PlayerYPosition) <= 1).ToList();
Förenklar nested if + loopar till en kedja av filter.
Statistik (SPACESHIP/Game.cs):
Total/Obstacle/Powerup: _playerLanes.SelectMany(lane => lane).OfType<Obstacle>().Count(obj => obj.IsActive);
Förenklar dubbla loopar + typkontroll + räknare.
Senaste meddelande (SPACESHIP/Game.cs):
_messageEvents.GetAll().LastOrDefault();
Hämtar sista element utan explicit loop.
Lane-aktiv filtrering (SPACESHIP/Lane.cs):
_objects.GetAll().Where(obj => obj.IsActive)
Förenklar filtreringslogik utan temporär lista.
Översikt av viktiga filkopplingar
Fabriker: ObstacleFactory, PowerupFactory implementerar IGameObjectFactory; injiceras i Game från Program.
Bridge: IBehavior-konkretioner använder IRenderable-konkretioner för att rendera effekter; Game.DrawUI() anropar RenderCollisionEffect(...) på senaste kollisionens GameObject.
Strategy: IMovementStrategy väljs i Program och används av AIPlayer.
Generics: ObjectBuffer<T> används både i Lane och Game med olika type arguments.
Iterator: Lane är itererbar och används direkt i LINQ-kedjor i Game.
LINQ: används konsekvent för filtrering, projektion och aggregation för att förenkla algoritmer.
Detta uppfyller samtliga konceptkrav enligt specifikationen, med tydlig användning, injektion, komposition och förenklad algoritmik där det är lämpligt.
