using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ControleBolinha : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float velocidade = 7f;
    [SerializeField] private float forcaDoPulo = 11f;

    [Header("Checagem de chao")]
    [SerializeField] private Transform pontoDoPe;
    [SerializeField] private float raioDaChecagem = 0.25f;
    [SerializeField] private LayerMask camadaDoChao;

    private Rigidbody rb;
    private float entradaHorizontal;
    private bool querPular;
    private bool noChao;

    // Roda uma vez, quando o objeto nasce.
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Roda a cada frame desenhado. Entrada do jogador vive aqui.
    private void Update()
    {
        var teclado = Keyboard.current;
        if (teclado == null) return;

        entradaHorizontal = 0f;
        if (teclado.aKey.isPressed || teclado.leftArrowKey.isPressed)  entradaHorizontal -= 1f;
        if (teclado.dKey.isPressed || teclado.rightArrowKey.isPressed) entradaHorizontal += 1f;

        // wasPressedThisFrame so eh true no frame exato do aperto,
        // por isso guardamos a intencao numa flag.
        if (teclado.spaceKey.wasPressedThisFrame || teclado.upArrowKey.isPressed ||
             teclado.wKey.isPressed) querPular = true;
    }

    // Roda em passo fixo (50x por segundo). Fisica vive aqui.
    private void FixedUpdate()
    {
        // A segunda condicao evita re-pular no frame seguinte ao pulo,
        // quando a bola ainda encosta na plataforma mas ja esta subindo.
        noChao = Physics.CheckSphere(pontoDoPe.position, raioDaChecagem, camadaDoChao)
                 && rb.linearVelocity.y <= 0.1f;

        Vector3 v = rb.linearVelocity;

        v.x = entradaHorizontal * velocidade;

        if (querPular && noChao) v.y = forcaDoPulo;
        querPular = false;

        rb.linearVelocity = v;
    }

    // Desenha a esfera de checagem na aba Scene. So no editor.
    private void OnDrawGizmosSelected()
    {
        if (pontoDoPe == null) return;
        Gizmos.color = noChao ? Color.green : Color.red;
        Gizmos.DrawWireSphere(pontoDoPe.position, raioDaChecagem);
    }
}