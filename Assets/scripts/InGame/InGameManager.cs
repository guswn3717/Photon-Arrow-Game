using UnityEngine;
using Photon.Pun;

public class InGameManager : MonoBehaviour
{
    // Resources 폴더 안에 있는 플레이어 프리팹의 이름 (확장자 제외)
    public string playerPrefabName = "player"; 

    void Start()
    {
        // 방에 들어가자마자 네트워크 상에 내 캐릭터를 생성합니다.
        PhotonNetwork.Instantiate(playerPrefabName, new Vector3(0, -3.5f, 0), Quaternion.identity);
    }
}