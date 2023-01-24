using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableLantern : Interactable
{
    public Light luz;
    public float cantBateria = 100;
    private float perdidaBateria = 1f;

    public void Start() 
    {
        BarraBateria.instance.batterySlider.maxValue = 100;
        BarraBateria.instance.batterySlider.value = cantBateria;
    }

    public override void Awake()
    {
        luz = GetComponentInChildren<Light>();
    }

    //Bateria
    private void Update() 
    {
        cantBateria = Mathf.Clamp(cantBateria, 0, 100);
        int valorBateria = (int)cantBateria;

        if(luz.enabled == true && cantBateria > 0)  
        {
            cantBateria -= perdidaBateria * Time.deltaTime;
            BarraBateria.instance.batterySlider.value = cantBateria;
        }
        if(cantBateria == 0)
            luz.intensity = 0f;
        if(cantBateria > 0 && cantBateria <= 25) 
            luz.intensity = 2f;
        if(cantBateria > 25 && cantBateria <= 50) 
            luz.intensity = 4f;
        if(cantBateria > 50 && cantBateria <= 75) 
            luz.intensity = 6f;
        if(cantBateria > 75 && cantBateria <= 100) 
            luz.intensity = 8f;
    }

    public override void OnFocus()
    {
        print("Focus on " + gameObject.name);
    }

    public override void OnInteract(GameObject itemsContainer, ref List<HandItem> allPlayerObjects)
    {
        if (!CheckLanternInPlayer(allPlayerObjects))
        {
            GameObject lantern = Instantiate(prefab) as GameObject;
            HandLanternItem itemToAdd = lantern.GetComponent<HandLanternItem>();

            lantern.transform.parent = itemsContainer.transform;
            lantern.gameObject.transform.localPosition = itemPosition;
            lantern.transform.localRotation = itemRotation;
            lantern.SetActive(false);

            Destroy(this.gameObject);
            allPlayerObjects.Add(itemToAdd);
        }
    }

    private bool CheckLanternInPlayer(List<HandItem> allPlayerObjects)
    {
        foreach (var item in allPlayerObjects)
        {
            if (item is HandLanternItem)
                return true;
        }

        return false;
    }

    public override void OnLoseFocus()
    {
        print("Lose focus on " + gameObject.name);
    }
}
