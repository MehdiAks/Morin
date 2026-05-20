using UnityEngine;

public static class GameProgress
{
    public static bool PremierOuverture = true;

    public static bool SalleDevinetteValidee = false;
    public static bool SalleTuyauValidee = false;
    public static bool SalleParcoursValidee = false;


    public static void LoadSave(){
        SalleDevinetteValidee = (PlayerPrefs.GetInt("SalleDevinette") == 1);
        SalleTuyauValidee = (PlayerPrefs.GetInt("SalleTuyau") == 1);
        SalleParcoursValidee = (PlayerPrefs.GetInt("SalleParcours") == 1);
    }
}
