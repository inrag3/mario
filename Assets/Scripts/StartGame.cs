using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartGame : MonoBehaviour
{
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(StartTheGame);
    }

    public void StartTheGame() =>
        SceneManager.LoadScene(1);

    private void OnDisable()
    {
        button.onClick.RemoveListener(StartTheGame);
    }
}
