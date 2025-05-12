using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneController : MonoBehaviour
{
    private AudioSource pressSound;

    enum Scenes
    {
        Title,
        Tutorial,
        Character,
        TutorialPoint,
        Tutorial2,
        Starting,
        Complete
    }

    private Scenes currentScene = Scenes.Title;

    [SerializeField]
    private GameObject sceneTutorial;
    [SerializeField]
    private AnimateXY sceneTutorial2;

    [SerializeField]
    private AnimateXY sceneTitleLogo;

    [SerializeField]
    private AnimateXY sceneTitleSpace;

    [SerializeField]
    private AnimateXY sceneCharacter;

    [SerializeField]
    private AnimateXY scenePoint;

    [SerializeField] private SpriteRenderer blackOverlay;

    [SerializeField] private AnimateCamera camera;

    [SerializeField] private List<AnimateXY> bgObjectsToAnimate = new List<AnimateXY>();

    [SerializeField] private CharacterSelector characterSelector;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pressSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.day == 1)
        {
            if (!characterSelector.hasSelected && currentScene == Scenes.Character) { return; }
            ToNextSceneDay1();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.day == 2)
        {
            ToNextSceneDay2();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.day == 3)
        {
            //ToNextSceneDay3();
            ToNextSceneDay2();
            return;
        }
    }

    void ToNextSceneDay1()
    {
        switch(currentScene)
        {
            case Scenes.Title:
            {
                    PlaySound();
                    currentScene = Scenes.Tutorial;
                    sceneTitleSpace.AnimateY(-170f, 1.5f);
                    sceneTitleLogo.AnimateY(-79f, 1.5f);
                    sceneTutorial.SetActive(true);
                break;
            }
            case Scenes.Tutorial:
                {

                    PlaySound();
                    currentScene = Scenes.Character;
                    Destroy(sceneTutorial.gameObject);
                    Destroy(sceneTitleLogo.gameObject);
                    Destroy(sceneTutorial.gameObject);
                    sceneCharacter.gameObject.SetActive(true);
                    break;
                }
            case Scenes.Character:
                {
                    PlaySound();
                    currentScene = Scenes.TutorialPoint;
                    Destroy(sceneCharacter.gameObject);
                    scenePoint.gameObject.SetActive(true);
                    GameManager.Instance.dataController.AddParticipant();
                    break;
                }
            case Scenes.TutorialPoint:
                {
                    PlaySound();
                    Destroy(scenePoint.gameObject);
                    currentScene = Scenes.Tutorial2;
                    sceneTutorial2.gameObject.SetActive(true);
                    break;
                }
            case Scenes.Tutorial2:
                {
                    PlaySound();
                    currentScene = Scenes.Starting;
                    sceneTutorial2.AnimateY(-196f, 1.5f);
                    blackOverlay.DOFade(0f, 2f).OnComplete(() => Destroy(blackOverlay.gameObject));
                    camera.StartAnimation();
                    foreach (var bg in bgObjectsToAnimate)
                    {
                        bg.StartAnimation();
                    }
                    break;
                }

        }
    }

    void ToNextSceneDay2()
    {
        switch (currentScene)
        {
            case Scenes.Title:
                {
                    PlaySound();
                    currentScene = Scenes.Tutorial;
                    sceneTitleSpace.AnimateY(-170f, 1.5f);
                    sceneTitleLogo.AnimateY(-79f, 1.5f);
                    sceneTutorial.SetActive(true);
                    break;
                }
            case Scenes.Tutorial:
                {
                    PlaySound();
                    currentScene = Scenes.TutorialPoint;
                    scenePoint.gameObject.SetActive(true);
                    Destroy(sceneTutorial.gameObject);
                    Destroy(sceneTitleLogo.gameObject);
                    Destroy(sceneTutorial.gameObject);
                    break;
                }
            case Scenes.TutorialPoint:
                {
                    PlaySound();
                    Destroy(scenePoint.gameObject);
                    currentScene = Scenes.Tutorial2;
                    sceneTutorial2.gameObject.SetActive(true);
                    break;
                }
            case Scenes.Tutorial2:
                {
                    PlaySound();
                    currentScene = Scenes.Starting;
                    sceneTutorial2.AnimateY(-196f, 1.5f);
                    blackOverlay.DOFade(0f, 2f).OnComplete(() => Destroy(blackOverlay.gameObject));
                    camera.StartAnimation();
                    break;
                }

        }
    }


    private void PlaySound()
    {
        // pressSound.Play();
    }
}
