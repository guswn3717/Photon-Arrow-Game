using UnityEngine;
using Photon.Pun; // 1. 포톤 네임스페이스 추가

public class ArrowGenerator : MonoBehaviour
{
    // 화살 프리팹을 저장하는 GameObject 변수 (Resources 폴더 안에 있는 "Arrow"를 사용합니다)
    public GameObject arrowPrefab = null;
   
    int arrowPositionX = 0;
    
    // 화살 생성 시간 간격 : 1초마다 화살 생성
    float arrowCreateInterval = 1.0f;

    // 프레임마다 경과한 시간을 누적하는 변수
    float elapsedTime = 0.0f;  

    void Start()
    {
        
    }

    void Update()
    {
        // ★핵심★ 방장(Master Client)인 사람만 화살을 생성합니다!
        // 이 코드가 없으면 방에 있는 모든 사람이 각자 화살을 만들어 화면이 난장판이 됩니다.
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        // 1. 프레ි 사이의 경과 시간을 누적
        elapsedTime += Time.deltaTime;

        // 2. 누적 시간이 화살 생성 간격(1초)을 초과하면 화살 생성
        if (elapsedTime > arrowCreateInterval)
        {
            // 누적 경과 시간 초기화
            elapsedTime = 0.0f;

            // 4. 화살이 생성될 x 좌표를 무작위로 결정
            arrowPositionX = Random.Range(-6, 7);

            // 5. 네트워크 상에 화살 생성 (Resources 폴더 안에 "Arrow" 프리팹이 있어야 합니다!)
            // PhotonNetwork.Instantiate를 쓰면 모든 유저 화면에 이 화살이 똑같이 소환됩니다.
            PhotonNetwork.Instantiate("Arrow", new Vector3(arrowPositionX, 7, 0), Quaternion.identity);
        }
    }
}