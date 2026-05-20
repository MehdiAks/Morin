using UnityEngine;

public static class GameProgress
{
    public static bool PremierOuverture = true;

    public static bool SalleDevinetteValidee = false;
    public static bool SalleTuyauValidee = false;
    public static bool SalleParcoursValidee = false;


    public static void LoadSave(){
        SalleDevinetteValidee = PlayerPrefs.GetBool("SalleDevinette");
        SalleTuyauValidee = PlayerPrefs.GetBool("SalleTuyau");
        SalleParcoursValidee = PlayerPrefs.GetBool("SalleParcours");
    }
}
