using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _restartPanel;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _quitButtonButton;

    public static event Action OnRestart;
    public static event Action OnStarted;

    private void Awake()
    {
        _restartPanel.GetComponent<Animator>().SetBool("isEnable", false);

    }
    void Start()
    {
        TowerBuilder.OnLosed += () => _restartPanel.GetComponent<Animator>().SetBool("isEnable", true);
        _restartButton.onClick.AddListener(() =>
        {
            _restartPanel.GetComponent<Animator>().SetBool("isEnable", false);
            StartCoroutine(WaiteCorutine(0.7f, () => OnRestart?.Invoke()));
        });
        _startButton.onClick.AddListener(() =>
        {
            _startPanel.GetComponent<Animator>().SetBool("isEnable", false);
            StartCoroutine(WaiteCorutine(0.7f, () => OnStarted?.Invoke()));
        });
    }

    private IEnumerator WaiteCorutine(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
