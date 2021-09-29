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

    // M�todo para o bot�o de iniciar jogo
    public void StartMundoGame()
    {
        SceneManager.LoadScene("Lab1");
    }

    // M�todo para o bot�o de voltar � tela de menu inicial do jogo
    public void IrParaMenu()
    {
        SceneManager.LoadScene("Lab1_start");
    }

    // M�todo do bot�o para ir � tela de cr�ditos do jogo
    public void VerCreditos()
    {
        SceneManager.LoadScene("Lab1_creditos");
    }

    // M�todo para o bot�o de sair do jogo
    public void Sair()
    {
        Application.Quit();
    }
}
