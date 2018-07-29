using UnityEngine;

public class Cooldown {

    [SerializeField] private FloatStatistic time = new FloatStatistic(1f);

    private float lastUseTime = -Mathf.Infinity;
    
    public bool isAvailable { get { return Time.time >= lastUseTime + time.value; } }

    public float remainingTime { get { return Mathf.Max(0f, this.lastUseTime + this.time.value - Time.time); } }

    public Cooldown (float time = 1f) {
        this.time = new FloatStatistic(time);
    }

    public void Use () {
        lastUseTime = Time.time;
    }

}
