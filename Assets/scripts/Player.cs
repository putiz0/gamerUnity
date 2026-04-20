using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera cam;
    private playerStatus status;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 movimento;

    public GameObject projecaoPrefab;
    private bool podeAtacar = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        status = GetComponent<playerStatus>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // ======================
    // INPUT
    // ======================
    public void OnMovimento(InputAction.CallbackContext context)
    {
        movimento = context.ReadValue<Vector2>();

        if (status.EstaCongelado() && movimento.sqrMagnitude > 0f)
        {
            status.QueimaduraPorGelo();
        }
    }

    public void OnAtaque(InputAction.CallbackContext context)
    {
        if (context.performed)
            Atacar();
    }

    // ======================
    // MOVIMENTO
    // ======================
    void FixedUpdate()
    {
         float velocidade = status.GetStat(Stat.VelocidadeDeMovimento) *
                           (1f + Mathf.Max(0f, status.GetStat(Stat.BonusVelocidadeMovimentoPct)));

        // redução se congelado
        if (status.EstaCongelado())
        {
            velocidade *= 0.5f;
        }

        rb.linearVelocity = movimento * velocidade;
        AtualizarAnimacao();
    }
    void Update()
    {
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            inventarioMenagen.Instance.DescateItem();
            Debug.Log("o G foi apertado");
        }
    }

    void AtualizarAnimacao()
    {
        if (animator == null)
        {
            return;
        }

        if (movimento.sqrMagnitude > 0.01f)
        {
            if (spriteRenderer != null && movimento.x < 0f)
            {
                spriteRenderer.flipX = true;
            }
            else if (spriteRenderer != null && movimento.x > 0f)
            {
                spriteRenderer.flipX = false;
            }
            animator.Play("andando");
        }
        else
        {
            animator.Play("parado");
        }
    }

    // ======================
    // ATAQUE
    // ======================
    void Atacar()
    {
        if (!podeAtacar) return;

        float velocidadeAtaque = status.GetStat(Stat.VelocidadeDeAtaque) *
                                 (1f + Mathf.Max(0f, status.GetStat(Stat.BonusVelocidadeAtaquePct)));
        float delayAtaque = 1f / Mathf.Max(0.01f, velocidadeAtaque);

        StartCoroutine(CooldownAtaque(delayAtaque));

        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = cam.ScreenToWorldPoint(mousePos);
        mouseWorld.z = 0;

        Vector3 direcao = (mouseWorld - transform.position).normalized;

        GameObject proj = Instantiate(projecaoPrefab, transform.position, Quaternion.identity);

        Rigidbody2D rbp = proj.GetComponent<Rigidbody2D>();
        rbp.linearVelocity = direcao * 10f;

        
        Projetil p = proj.GetComponent<Projetil>();
        if (p != null)
        {
            p.dono = gameObject;
            p.atacante = GetComponent<EntityStatus>();

            // 🔥 NOVO SISTEMA
            p.danoFisico = status.GetStat(Stat.AtaqueFisico) *
                           (1f + Mathf.Max(0f, status.GetStat(Stat.BonusAtaqueFisicoPct)));
                            p.danoFogo = status.GetStat(Stat.AtaqueDeFogo);
            p.danoGelo = status.GetStat(Stat.AtaqueDeGelo);
            p.danoEletrico = status.GetStat(Stat.AtaqueEletrico);

            p.distanciaMaxima = status.GetStat(Stat.DistanciaDoAtaque) *
                                (1f + Mathf.Max(0f, status.GetStat(Stat.BonusDistanciaAtaquePct)));
        }
    }

    IEnumerator CooldownAtaque(float time)
    {
        podeAtacar = false;
        yield return new WaitForSeconds(time);
        podeAtacar = true;
    }
}
