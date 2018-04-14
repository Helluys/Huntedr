using UnityEngine;

public class GameManager : MonoBehaviour {

    static GameManager instance;

    void Start() {
        if (instance != null) {
            Debug.LogError("Only one instance of gameManager is allowed");
            Destroy(this);
        } else {
            instance = this;
        }
    }
}
