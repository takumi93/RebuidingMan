using UnityEngine;

public class BodyRig : MonoBehaviour
{
    [Header("頭に連動するRigを指定")]
    [SerializeField] public GameObject BodyToHeadRig = null;
    [Header("足に連動するRigを指定")]
    [SerializeField] public GameObject BodyToLegRig = null;
}
