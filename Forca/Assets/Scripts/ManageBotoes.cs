using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageBotoes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("score", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Método para o botão de iniciar jogo
    public void StartMundoGame()
    {
        SceneManager.LoadScene("Lab1");
    }

    // Método para o botão de voltar à tela de menu inicial do jogo
    public void IrParaMenu()
    {
        SceneManager.LoadScene("Lab1_start");
    }

    // Método do botão para ir à tela de créditos do jogo
    public void VerCreditos()
    {
        SceneManager.LoadScene("Lab1_creditos");
    }

    // Método para o botão de sair do jogo
    public void Sair()
    {
        Application.Quit();
    }
}
