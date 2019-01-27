using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

  private Vector3 cameraTarget, ogCameraPosition;
  private bool toggleView;
  float cameraSpeed = 5f;

  // Start is called before the first frame update
  void Start()
  {
    ogCameraPosition = Camera.main.transform.position;
    cameraTarget = Camera.main.transform.position;
    cameraTarget.y = 8;
    cameraTarget.z = -20;
  }

  // Update is called once per frame
  void Update()
  {
    if (toggleView)
    {
      Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, this.cameraTarget, Time.deltaTime * this.cameraSpeed);
    }
    else
    {
      Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, this.ogCameraPosition, Time.deltaTime * this.cameraSpeed);
    }
  }

  public void SetCameraOut(bool value){
    this.toggleView = value;
  }

  public bool GetHasCameraMoved(){
    return this.toggleView;
  }
}
