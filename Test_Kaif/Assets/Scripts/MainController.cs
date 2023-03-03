using UnityEngine;

public class MainController : MonoBehaviour
{
    public static MainController Inst;

    public GlobalParamsSO globalParams;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
        }
    }

    public void Start()
    {
        UIController.Inst.Setup();
        BallController.Inst.Setup();
    }
}