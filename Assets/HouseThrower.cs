using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HouseThrower : MonoBehaviour
{
  GameObject houseParent;

  RectTransform businessMan;
  float businessSpeed = .5f;
  bool activateMan = false;
  bool lockMan = true;

  Text redText, greenText, blueText, ipText;

  CameraController ct;


  int redScore, greenScore, blueScore;
  int credScore = 0, cgreenScore = 0, cblueScore = 0;


  // Start is called before the first frame update
  void Awake()
  {
    ct = Camera.main.GetComponent<CameraController>();
    this.houseParent = GameObject.FindWithTag("Player");
    GameObject ui = GameObject.FindWithTag("ui");
    Debug.Log(ui.transform.childCount);
    businessMan = ui.transform.GetChild(0).GetComponent<RectTransform>();
    redText = ui.transform.GetChild(1).GetComponent<Text>();
    greenText = ui.transform.GetChild(2).GetComponent<Text>();
    blueText = ui.transform.GetChild(3).GetComponent<Text>();
    ipText = ui.transform.GetChild(4).GetComponent<Text>();
    StartCoroutine(GetRequest("http://localhost:8080/get_scores"));
  }

  IEnumerator GetRequest(string uri)
  {
    using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
    {
      // Request and wait for the desired page.
      yield return webRequest.SendWebRequest();

      string[] pages = uri.Split('/');
      int page = pages.Length - 1;

      if (webRequest.isNetworkError)
      {
        Debug.Log(pages[page] + ": Error: " + webRequest.error);
      }
      else
      {
        Debug.Log(webRequest.downloadHandler.text);
        ServerResponce rs = JsonUtility.FromJson<ServerResponce>(webRequest.downloadHandler.text);
        this.redScore = rs.redScore;
        this.greenScore = rs.greenScore;
        this.blueScore = rs.blueScore;

        if (this.credScore > rs.redScore || this.cgreenScore > rs.greenScore || this.cblueScore > rs.blueScore)
        {
          for (int i = houseParent.transform.childCount - 1; i >= 0; i--)
          {
            Destroy(houseParent.transform.GetChild(i).gameObject);
          }
          this.credScore = 0;
          this.cblueScore = 0;
          this.cgreenScore = 0;
          this.redText.text = "£0k";
          this.greenText.text = "£0k";
          this.blueText.text = "£0k";
        }
        // if ((this.credScore > 500 || this.cgreenScore > 500 || this.cblueScore > 500) && !this.lockMan)
        // {
        //   this.activateMan = true;
        //   this.lockMan = true;
        // }
      }
      yield return new WaitForSeconds(1f);
      StartCoroutine(GetRequest("http://localhost:8080/get_scores"));
    }
  }

  IEnumerator DragMan()
  {
    yield return new WaitForSeconds(20f);
    this.activateMan = true;
  }

  public string LocalIPAddress()
  {
    IPHostEntry host;
    string localIP = "";
    host = Dns.GetHostEntry(Dns.GetHostName());
    foreach (IPAddress ip in host.AddressList)
    {
      if (ip.AddressFamily == AddressFamily.InterNetwork)
      {
        localIP = ip.ToString();
        break;
      }
    }
    return localIP;
  }

  void Start()
  {
    // cl.Connect("192.168.1.184", 8080);
    this.ipText.text = "Join today! " + LocalIPAddress() + ":8080";
    this.redText.text = "£0k";
    this.greenText.text = "£0k";
    this.blueText.text = "£0k";
    StartCoroutine(DragMan());
  }

  void MoveBusinessMan()
  {
    if (activateMan)
    {
      businessMan.anchoredPosition = Vector2.MoveTowards(
          businessMan.anchoredPosition,
          new Vector2(600, 0),
          100 * Time.deltaTime
      );
      if (businessMan.anchoredPosition.x == 600)
      {
        activateMan = false;
        businessMan.anchoredPosition = new Vector2(-600, 0);
      }
    }
  }

  // Update is called once per frame
  void Update()
  {
    MoveBusinessMan();
    if (this.credScore > 0 || this.cgreenScore > 0 || this.cblueScore > 0)
    {
      this.ct.SetCameraOut(true);
    }
    else
    {
      this.ct.SetCameraOut(false);
    }

    if (this.credScore < this.redScore)
    {
      int redDiff = this.redScore - this.credScore;
      for (int i = 0; i < redDiff / 4; i++)
      {
        GameObject hhh = Resources.Load("redHouse") as GameObject;
        hhh = Instantiate(hhh);
        hhh.transform.position = new Vector3(-50, 50 + (5 * i), 50);
        hhh.transform.SetParent(houseParent.transform);
      }
      this.credScore = this.redScore;
      this.redText.text = "£" + this.credScore + "k";

    }
    if (this.cgreenScore < this.greenScore)
    {
      int greenDiff = this.greenScore - this.cgreenScore;
      for (int i = 0; i < greenDiff / 4; i++)
      {
        GameObject hhh = Resources.Load("greenHouse") as GameObject;
        hhh = Instantiate(hhh);
        hhh.transform.position = new Vector3(0, 50 + (5 * i), 50);
        hhh.transform.SetParent(houseParent.transform);
      }
      this.cgreenScore = this.greenScore;
      this.greenText.text = "£" + this.cgreenScore + "k";
    }
    if (this.cblueScore < this.blueScore)
    {
      int blueDiff = this.blueScore - this.cblueScore;
      for (int i = 0; i < blueDiff / 4; i++)
      {
        GameObject hhh = Resources.Load("blueHouse") as GameObject;
        hhh = Instantiate(hhh);
        hhh.transform.position = new Vector3(50, 50 + (5 * i), 50);
        hhh.transform.SetParent(houseParent.transform);
      }
      this.cblueScore = this.blueScore;
      this.blueText.text = "£" + this.cblueScore + "k";
    }
  }
}
