using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public List<GameObject> decorativeZombies;
    public Image fadeOutImage;

    // Update is called once per frame
    void Update()
    {
        foreach(var zombie in decorativeZombies)
        {
            Vector3 pos = zombie.transform.position;
            pos += Vector3.right * 4 * Time.deltaTime;
            if (pos.x > 20) pos = new Vector3(-20, pos.y, pos.z);
            zombie.transform.position = pos;
        }
    }

    public void BeginFirstLevel()
    {
        StartCoroutine(_BeginFirstLevel());
    }
    private IEnumerator _BeginFirstLevel()
    {
        yield return CoroutineBuilder.Linear01(t =>
        {
            fadeOutImage.color = Color.Lerp(Color.clear, Color.black, t);
        });
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }
}
