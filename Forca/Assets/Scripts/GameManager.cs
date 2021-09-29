using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Script principal do jogo da forca
/// Criado por:
///     Ana Julia Monici Orbetelli (11201810144)
///     Gabriel Soares de Camargo Munaro (21011615)
/// Para a disciplina: Programa��o Baseada em Componentes para Jogos
///     Universidade Federal do ABC, 2021
/// Professor: M�rio Minami
/// </summary>

public class GameManager : MonoBehaviour
{
    private int numTentativas;          // armazena as tentativas v�lidas da rodada
    private int maxNumTentativas;       // n�mero m�ximo de tentativas para forca ou salva��o
    int score = 0;

    public AudioSource acerto;          // �udio para quando uma letra da palavra oculta � descoberta
    public AudioSource erro;            // �udio para quando o jogador erra uma letra da palavra oculta

    public GameObject letra;            // prefab da letra no game
    public GameObject centro;           // objeto de texto que indica o centro da tela

    private string palavraOculta = "";  // palavra oculta a ser descoberta

    private int tamanhoPalavraOculta;   // tamanho da palavra oculta
    char[] letrasOcultas;               // letras da palavra oculta
    bool[] letrasDescobertas;           // indicador de quais letras foram descobertas
    
    // Start is called before the first frame update
    void Start()
    {
        centro = GameObject.Find("centroDaTela");

        InitGame();
        InitLetras();
        numTentativas = 0;
        maxNumTentativas = 10;
        UpdateNumTentativas();
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTeclado();
    }

    // M�todo para instanciar as letras da palavra oculta
    void InitLetras()
    {
        int numLetras = tamanhoPalavraOculta;
        for (int i = 0; i < numLetras; i++)
        {
            Vector3 novaPosicao;
            novaPosicao = new Vector3(centro.transform.position.x + ((i - numLetras / 2.0f) * 80), centro.transform.position.y, centro.transform.position.z);
            GameObject l = (GameObject)Instantiate(letra, novaPosicao, Quaternion.identity);
            l.name = "letra" + (i + 1);     // nomeia na hierarquia a GameObject com letra-(i�sima+1), i = 1.numLetras
            l.transform.SetParent(GameObject.Find("Canvas").transform);     // posiciona-se como filho do GameObject Canvas
        }
    }

    // M�todo para inicializar as vari�veis principais do jogo, como a palavra oculta
    void InitGame()
    {
        palavraOculta = PegaUmaPalavraDoArquivo();      // palavra oculta do jogo atual, � uma palavra aleat�ria do arquivo de palavras
        tamanhoPalavraOculta = palavraOculta.Length;    // determina-se o n�mero de letras da palavra oculta
        palavraOculta = palavraOculta.ToUpper();        // transforma-se a palavra em mai�scula
        letrasOcultas = new char[tamanhoPalavraOculta]; // instancia-se o array char das letras da palavra oculta
        letrasDescobertas = new bool[tamanhoPalavraOculta]; // instancia-se o array bool do indicador de letras descobertas
        letrasOcultas = palavraOculta.ToCharArray();    // copia-se a palavra no array de letras
    }

    /* M�todo para tratar a letra digitada pelo jogador
     * Verifica se letra teclada � parte da palavra oculta e atualiza a palavra
     * Tamb�m ajusta score e n�mero de tentativas
     */
    void CheckTeclado()
    {
        if (Input.anyKeyDown)
        {
            char letraTeclada = Input.inputString.ToCharArray()[0];
            int letraTecladaComoInt = System.Convert.ToInt32(letraTeclada);

            if (letraTecladaComoInt >= 97 && letraTecladaComoInt <= 122)
            {
                numTentativas++;
                UpdateNumTentativas();
                if (numTentativas > maxNumTentativas)
                {
                    SceneManager.LoadScene("Lab1_forca");
                }
                bool errou = true;
                for (int i = 0; i < tamanhoPalavraOculta; i++)
                {
                    if (!letrasDescobertas[i])
                    {
                        letraTeclada = System.Char.ToUpper(letraTeclada);
                        if (letrasOcultas[i] == letraTeclada)
                        {
                            letrasDescobertas[i] = true;
                            GameObject.Find("letra" + (i + 1)).GetComponent<Text>().text = letraTeclada.ToString();
                            score = PlayerPrefs.GetInt("score");
                            score++;
                            PlayerPrefs.SetInt("score", score);
                            UpdateScore();
                            acerto.Play();
                            errou = false;
                            VerificaSePalavraDescoberta();
                        }
                    }
                }
                if (errou)
                {
                    erro.Play();
                }
            }
        }
    }

    // M�todo para atualizar o n�mero de tentativas j� feitas pelo jogador
    void UpdateNumTentativas()
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = numTentativas + " / " + maxNumTentativas;
    }

    // M�todo para atualizar o Score do jogador
    void UpdateScore()
    {
        GameObject.Find("scoreUI").GetComponent<Text>().text = "Pontua��o: " + score;
    }

    // M�todo para verificar se palavra oculta foi inteira descoberta
    void VerificaSePalavraDescoberta()
    {
        bool condicao = true;
        for (int i = 0; i < tamanhoPalavraOculta; i++)
        {
            condicao = condicao && letrasDescobertas[i];
        }
        if (condicao)
        {
            PlayerPrefs.SetString("ultimaPalavraOculta", palavraOculta);
            SceneManager.LoadScene("Lab1_salvo");
        }
    }

    // M�todo para atribuir uma palavra oculta aleat�ria para o jogo atual a partir do arquivo de palavras
    string PegaUmaPalavraDoArquivo()
    {
        TextAsset t1 = (TextAsset)Resources.Load("palavras", typeof(TextAsset));
        string s = t1.text;
        string[] palavras = s.Split(' ');
        int palavraAleatoria = Random.Range(0, palavras.Length + 1);
        return (palavras[palavraAleatoria]);
    }
}
