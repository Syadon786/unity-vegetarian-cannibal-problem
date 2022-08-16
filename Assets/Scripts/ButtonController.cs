using UnityEngine;
using System.IO;
using System.Diagnostics;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    
    public void ConsoleStart()
    {
        //string path = Directory.GetCurrentDirectory();
       // path = Directory.GetParent(path).ToString();
       // UnityEngine.Debug.Log(path);
        Process.Start(@"ConsoleSolution\CannibalAndVegetarian\bin\Debug\netcoreapp3.1\CannibalAndVegetarian.exe");
    }
    public void LoadSceneButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
