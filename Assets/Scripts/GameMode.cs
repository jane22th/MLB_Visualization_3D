using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameMode : MonoBehaviour {
    GameObject m_game_mode_canvas, m_game_select_canvas, m_game_load_canvas;

    GameObject m_sim_obj, m_manual_obj, m_exit_obj;
    Button m_sim_btn, m_manual_btn, m_exit_btn;
	// Use this for initialization
	void Start () {
        m_game_mode_canvas = GameObject.Find("GameModeUI");
        m_game_load_canvas = GameObject.Find("GameLoadUI");
        m_game_select_canvas = GameObject.Find("GameSelectUI");

        m_sim_obj = GameObject.Find("Simulation");
        m_manual_obj = GameObject.Find("Manual");
        m_exit_obj = GameObject.Find("Exit");

        m_sim_btn = m_sim_obj.GetComponent<Button>();
        m_manual_btn = m_manual_obj.GetComponent<Button>();
        m_exit_btn = m_exit_obj.GetComponent<Button>();

        m_sim_btn.onClick.AddListener(delegate { simulationHandler(); });
        m_manual_btn.onClick.AddListener(delegate { manualHandler(); });
        m_exit_btn.onClick.AddListener(delegate { exitHandler(); });

        m_game_mode_canvas.SetActive(true);
        m_game_load_canvas.SetActive(false);
        m_game_select_canvas.SetActive(false);
	}

    void simulationHandler() {
        m_game_mode_canvas.SetActive(false);
        m_game_load_canvas.SetActive(true);
    }
    
    void manualHandler() {
    }

    void exitHandler() {
        Application.Quit();
    }
}
