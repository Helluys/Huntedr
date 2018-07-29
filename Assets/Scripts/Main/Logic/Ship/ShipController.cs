[System.Serializable]
public class ShipController {

    public ShipControllerModel model;
    public ShipControllerModel.Instance instance;

    public ShipController(Ship holder, ShipControllerModel model) {
        this.model = model;
        instance = this.model.CreateInstance(holder);
        instance.OnStart();
    }

    public void OnUpdate () {
        instance.OnUpdate();
    }
}
