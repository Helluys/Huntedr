using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "SingleTargetPicker", menuName = "Game data/Target Pickers/Single Target Picker")]
public class SingleTargetPicker : TargetPicker<Ship> {

    [SerializeField] private bool sameFaction;

    private int targetIndex;
    private IReadOnlyList<Ship> shipList;

    public Ship target { get { return targetIndex >= 0 ? shipList[targetIndex] : null; } }

    protected override void OnStartPicking () {
        shipList = GameManager.instance.shipList;
        caster.StartCoroutine(PickingCoroutine());
    }

    private IEnumerator PickingCoroutine () {
        targetIndex = GetClosestShipIndex();

        bool endPicking = false;
        while (!endPicking) {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Tab))
                targetIndex = (targetIndex + 1) % shipList.Count;

            if (Input.GetKeyDown(KeyCode.Escape)) {
                targetIndex = -1;
                endPicking = true;
            }

            if (Input.GetKeyDown(KeyCode.F))
                endPicking = true;

        }

        EndPicking(target);
    }

    protected override void OnCancelPicking () {

    }

    private float DistanceTo (Ship ship) {
        return (ship.transform.position - caster.transform.position).magnitude;
    }

    private int GetClosestShipIndex () {
        int index = -1;
        Ship target = null;
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < shipList.Count; i++) {
            Ship ship = shipList[i];
            float distance = DistanceTo(ship);
            if (!ship.Equals(caster) && !(ship.faction.Equals(caster.faction) ^ sameFaction) && distance < minDistance) {
                target = ship;
                index = i;
                minDistance = distance;
            }
        }

        return index;
    }
}
