using UnityEngine;

public class CameraSeguidora : MonoBehaviour
{
    [SerializeField] private Transform alvo;
    [SerializeField] private float suavidade = 0.18f;
    [SerializeField] private bool seguirNoEixoY = false;

    // Distancia fixa entre camera e alvo, capturada no inicio.
    private Vector3 deslocamento;
    private Vector3 velocidadeInterna;

    private void Start()
    {
        if (alvo == null) return;
        deslocamento = transform.position - alvo.position;
    }

    // LateUpdate roda depois de todos os Update do frame.
    private void LateUpdate()
    {
        if (alvo == null) return;

        Vector3 destino = alvo.position + deslocamento;

        if (!seguirNoEixoY) destino.y = transform.position.y;
        destino.z = transform.position.z;   // nunca mexe na profundidade

        transform.position = Vector3.SmoothDamp(
            transform.position, destino, ref velocidadeInterna, suavidade);
    }
}