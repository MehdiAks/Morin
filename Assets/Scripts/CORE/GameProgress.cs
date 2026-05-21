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

    public static void SaveRoomValidation(){
        PlayerPrefs.SetInt("SalleDevinette", SalleDevinetteValidee ? 1 : 0);
        PlayerPrefs.SetInt("SalleTuyau", SalleTuyauValidee ? 1 : 0);
        PlayerPrefs.SetInt("SalleParcours", SalleParcoursValidee ? 1 : 0);
        PlayerPrefs.Save();
    }
}
