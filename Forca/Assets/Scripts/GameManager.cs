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
/// Para a disciplina: Programação Baseada em Componentes para Jogos
///     Universidade Federal do ABC, 2021
/// Professor: Mário Minami
/// </summary>

public class GameManager : MonoBehaviour
{
    private int numTentativas;          // armazena as tentativas válidas da rodada
    private int maxNumTentativas;       // número máximo de tentativas para forca ou salvação
    int score = 0;

    public AudioSource acerto;          // áudio para quando uma letra da palavra oculta é descoberta
    public AudioSource erro;            // áudio para quando o jogador erra uma letra da palavra oculta

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

    // Método para instanciar as letras da palavra oculta
    void InitLetras()
    {
        int numLetras = tamanhoPalavraOculta;
        for (int i = 0; i < numLetras; i++)
        {
            Vector3 novaPosicao;
            novaPosicao = new Vector3(centro.transform.position.x + ((i - numLetras / 2.0f) * 80), centro.transform.position.y, centro.transform.position.z);
            GameObject l = (GameObject)Instantiate(letra, novaPosicao, Quaternion.identity);
            l.name = "letra" + (i + 1);     // nomeia na hierarquia a GameObject com letra-(iésima+1), i = 1.numLetras
            l.transform.SetParent(GameObject.Find("Canvas").transform);     // posiciona-se como filho do GameObject Canvas
        }
    }

    // Método para inicializar as variáveis principais do jogo, como a palavra oculta
    void InitGame()
    {
        palavraOculta = PegaUmaPalavraDoArquivo();      // palavra oculta do jogo atual, é uma palavra aleatória do arquivo de palavras
        tamanhoPalavraOculta = palavraOculta.Length;    // determina-se o número de letras da palavra oculta
        palavraOculta = palavraOculta.ToUpper();        // transforma-se a palavra em maiúscula
        letrasOcultas = new char[tamanhoPalavraOculta]; // instancia-se o array char das letras da palavra oculta
        letrasDescobertas = new bool[tamanhoPalavraOculta]; // instancia-se o array bool do indicador de letras descobertas
        letrasOcultas = palavraOculta.ToCharArray();    // copia-se a palavra no array de letras
    }

    /* Método para tratar a letra digitada pelo jogador
     * Verifica se letra teclada é parte da palavra oculta e atualiza a palavra
     * Também ajusta score e número de tentativas
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

    // Método para atualizar o número de tentativas já feitas pelo jogador
    void UpdateNumTentativas()
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = numTentativas + " / " + maxNumTentativas;
    }

    // Método para atualizar o Score do jogador
    void UpdateScore()
    {
        GameObject.Find("scoreUI").GetComponent<Text>().text = "Pontuação: " + score;
    }

    // Método para verificar se palavra oculta foi inteira descoberta
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

    // Método para atribuir uma palavra oculta aleatória para o jogo atual a partir do arquivo de palavras
    string PegaUmaPalavraDoArquivo()
    {
        TextAsset t1 = (TextAsset)Resources.Load("palavras", typeof(TextAsset));
        string s = t1.text;
        string[] palavras = s.Split(' ');
        int palavraAleatoria = Random.Range(0, palavras.Length + 1);
        return (palavras[palavraAleatoria]);
    }
}
