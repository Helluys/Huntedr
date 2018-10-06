using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "SingleTargetPicker", menuName = "Game data/Target Pickers/Single Target Picker")]
public class SingleTargetPicker : TargetPicker<Ship> {

    [SerializeField] private GameObject uiPrefab;
    [SerializeField] private bool sameFaction;

    private int targetIndex;
    private IReadOnlyList<Ship> shipList;
    private bool pickingCancelled;

    public Ship target { get { return targetIndex >= 0 ? shipList[targetIndex] : null; } }

    protected override void OnStartPicking () {
        shipList = FilterShipList(GameManager.instance.shipList);
        caster.StartCoroutine(PickingCoroutine());
    }

    private IReadOnlyList<Ship> FilterShipList (IReadOnlyList<Ship> inputShipList) {
        List<Ship> returnedList = new List<Ship>();

        foreach (Ship ship in inputShipList)
            if (!(ship.team.faction.Equals(caster.team.faction) ^ sameFaction))
                returnedList.Add(ship);

        return returnedList.AsReadOnly();
    }

    private IEnumerator PickingCoroutine () {
        pickingCancelled = false;
        targetIndex = GetClosestShipIndex();

        // Instantiate UI
        RectTransform uiTargetInstanceTransform = Instantiate(uiPrefab, GameManager.uiCanvas.transform).GetComponent<RectTransform>();

        bool endPicking = false;
        while (!endPicking) {
            // Update UI
            uiTargetInstanceTransform.localPosition = Camera.main.WorldToScreenPoint(target.transform.position) - 0.5f * new Vector3(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight, 0);
            uiTargetInstanceTransform.localScale = (30f / (30f + Vector3.Dot(this.target.transform.position - caster.transform.position, caster.transform.forward))) * Vector3.one;

            uiTargetInstanceTransform.gameObject.SetActive(Vector3.Dot(this.target.transform.position - Camera.main.transform.position, Camera.main.transform.forward) > 0f);

            yield return null;

            if (Input.GetKeyDown(KeyCode.Tab))
                targetIndex = (targetIndex + 1) % shipList.Count;

            if (Input.GetKeyDown(KeyCode.Escape) || pickingCancelled) {
                targetIndex = -1;
                endPicking = true;
            }

            if (Input.GetKeyDown(KeyCode.F))
                endPicking = true;

        }

        // Destroy UI
        Destroy(uiTargetInstanceTransform.gameObject);

        EndPicking(target);
    }

    protected override void OnCancelPicking () {
        pickingCancelled = true;
    }
    
    private int GetClosestShipIndex () {
        int index = -1;
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < shipList.Count; i++) {
            Ship ship = shipList[i];
            float distance = (ship.transform.position - caster.transform.position).magnitude;

            if (!ship.Equals(caster) && distance < minDistance) {
                index = i;
                minDistance = distance;
            }
        }

        return index;
    }
}
