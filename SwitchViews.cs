using UnityEngine;

public class SwitchViews : MonoBehaviour
{
    public static void SwitchView(GameObject targetScene, GameObject callerScene)
    {
        targetScene.SetActive(true);
        callerScene.SetActive(false);
    }
}
