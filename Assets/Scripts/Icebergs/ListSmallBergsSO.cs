using UnityEngine;

[CreateAssetMenu(fileName = "ListSmallBergs", menuName = "Scriptable Objects/ListSmallBergs")]
public class ListSmallBergsSO : ScriptableObject
{
    [SerializeField] GameObject[] smallIcebergs;

    public GameObject GetRandomSmallIceberg()
    {
        int randomIndex = Random.Range(0, smallIcebergs.Length);

        return smallIcebergs[randomIndex];
    }
}
