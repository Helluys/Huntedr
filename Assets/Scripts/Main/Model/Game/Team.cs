using System.Collections.Generic;

public class Team {

    public readonly string name;
    public readonly Faction faction;
    private readonly List<Ship> ships;
    public readonly TeamAI ai;

    public Team (string name, Faction faction, List<Ship> ships, AIPersonality aiPersonality) {
        this.name = name;
        this.faction = faction;
        this.ships = ships;
        this.ai = new TeamAI(this, aiPersonality);
    }

    public IReadOnlyList<Ship> GetShips () {
        return this.ships;
    }
}
