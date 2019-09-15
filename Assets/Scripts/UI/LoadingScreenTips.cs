using UnityEngine;

public class LoadingScreenTips : ScriptableObject
{
    private const string TIP_PREF_KEY = "loadingTip";
    
    [SerializeField]
    private string[] tips;

    private static LoadingScreenTips _instance;

    public static string GetRandom
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<LoadingScreenTips>(string.Format("{0}/LoadingScreenTips",
                    Metadata.ResourcesFolder.GameData));
            }

            if (PlayerPrefs.HasKey(TIP_PREF_KEY))
            {
                return PlayerPrefs.GetString(TIP_PREF_KEY);
            }

            string newTip = _instance.tips[Random.Range(0, _instance.tips.Length)];

            PlayerPrefs.SetString(TIP_PREF_KEY, newTip);
            
            return newTip;
        }
    }

    public static void ClearTip()
    {
        PlayerPrefs.DeleteKey(TIP_PREF_KEY);
    }
}
