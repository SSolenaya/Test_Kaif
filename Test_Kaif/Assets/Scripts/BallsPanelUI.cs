using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallsPanelUI : MonoBehaviour
{
    [SerializeField] private Image _ballSpritePrefab;
    [SerializeField] private List<Image> _ballImgList = new List<Image>(5);

    void Start()
    {
        SetupBallSprites();
    }

    private void SetupBallSprites()
    {
        for( int i = 0; i< MainController.Inst.globalParams.maxBallsAmount; i++)
        {
            var ballSprite = Instantiate(_ballSpritePrefab, transform);
            ballSprite.color = Color.white;
            _ballImgList.Add(ballSprite);
        }
    }

    public void ChangeAvailableBallUI(int ballsAvailable)
    {
        int whiteBallSpritesNum = MainController.Inst.globalParams.maxBallsAmount - ballsAvailable;
        _ballImgList.ForEach(_ => _.color = Color.red);
        if (whiteBallSpritesNum > 0)
        {
            for (int i = 0; i < whiteBallSpritesNum; i++)
            {
                _ballImgList[i].color = Color.white;
                _ballImgList[i].gameObject.transform.SetSiblingIndex(0);
            }
        }
    }
}
