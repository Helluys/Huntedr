public class EffectLootable : Lootable {

    public Effect effect;
    
    public override void PickUp (Ship ship) {
        ship.status.AddEffect(Instantiate(effect));
        Destroy(gameObject);
    }
}
