using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class telaDeMorte : MonoBehaviour
{
    public GameObject tela_de_morte;
    public Button reiniciar_;

    void Start()
    {
        if (tela_de_morte != null)
            tela_de_morte.SetActive(false);

        if (reiniciar_ != null)
            reiniciar_.onClick.AddListener(ReiniciarJogo);
    }

    public void Ativar()
    {
        if (tela_de_morte != null)
            tela_de_morte.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ReiniciarJogo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
