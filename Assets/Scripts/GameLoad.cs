using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameLoad : MonoBehaviour {
    GameObject m_game_mode_canvas, m_game_select_canvas, m_game_load_canvas;

    GameObject m_new_obj, m_load_obj, m_back_obj;
    Button m_new_btn, m_load_btn, m_back_btn;

    // Use this for initialization
    void Start() {
        m_game_mode_canvas = GameObject.Find("GameModeUI");
        m_game_load_canvas = GameObject.Find("GameLoadUI");
        m_game_select_canvas = GameObject.Find("GameSelectUI");

        m_new_obj = GameObject.Find("New");
        m_load_obj = GameObject.Find("Load");
        m_back_obj = GameObject.Find("GameLoadUI/Back");

        m_new_btn = m_new_obj.GetComponent<Button>();
        m_load_btn = m_load_obj.GetComponent<Button>();
        m_back_btn = m_back_obj.GetComponent<Button>();

        m_new_btn.onClick.AddListener(delegate { newHandler(); });
        m_load_btn.onClick.AddListener(delegate { loadHandler(); });
        m_back_btn.onClick.AddListener(delegate { backHandler(); });
    }

    void newHandler() {
        m_game_load_canvas.SetActive(false);
        m_game_select_canvas.SetActive(true);
    }

    void loadHandler() {
    }

    void backHandler() {
        m_game_mode_canvas.SetActive(true);
        m_game_load_canvas.SetActive(false);
    }
}