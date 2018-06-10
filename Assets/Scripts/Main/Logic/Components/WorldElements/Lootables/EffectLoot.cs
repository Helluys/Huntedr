public class EffectLoot : Lootable {

    public Effect effect;
    
    public override void PickUp (Ship ship) {
        Instantiate(effect).Activate(ship);
        Destroy(gameObject);
    }
}
