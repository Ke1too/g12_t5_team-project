using UnityEngine;
using UnityEngine.SceneManagement;

public class FusumaController : MonoBehaviour
{
    public string[] miniGameScenes;

    public void GoNextScene()
    {
        if (miniGameScenes.Length == 0) return;

        int idx = Random.Range(0, miniGameScenes.Length);
        SceneManager.LoadScene(miniGameScenes[idx]);
    }
}
    