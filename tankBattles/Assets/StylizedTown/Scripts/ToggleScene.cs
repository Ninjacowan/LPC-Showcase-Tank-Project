
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleScene : MonoBehaviour {

    

    void Update() {
        
        if (Input.GetKeyDown(KeyCode.E)) {
            
            

            if (SceneManager.GetActiveScene().name.Equals("SampleSceneDay")) {

                SceneManager.LoadScene("SampleSceneNight");
            }
            else { 
                SceneManager.LoadScene("SampleSceneDay");
            }
            
        }
       


    }
}
