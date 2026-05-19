using UnityEngine;

public class SphereMalus : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        HudManager hud = HudManager.instance; //On récupère le hud dans la variable hud
        AudioManager am = AudioManager.instance; //On récupère l'AudioManager dans la variable am   
        if(col.gameObject.tag == "Player")
        {
            hud.subPV(20);
            am.PlaySFX(am.sfx_list.sfx_hit);
            Destroy(this.gameObject);
        }
    }
}