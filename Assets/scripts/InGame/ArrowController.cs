using UnityEngine;
using Photon.Pun;

public class ArrowController : MonoBehaviour
{
    // 화살 이동 속도 (초당 Unity 단위)
    float arrowSpeed = 6.0f;
    
    GameObject player;
    
    // 충돌 판정에 사용할 원의 중심 좌표 및 방향 벡터
    Vector2 arrowCirclePoint = Vector2.zero;      // 화살 원의 중심 좌표
    Vector2 playerCirclePoint = Vector2.zero;     // 플레이어 원의 중심 좌표
    Vector2 arrowPlayerDirection = Vector2.zero;  // 두 원의 중심을 연결하는 벡터

    // 충돌 판정에 사용할 거리 및 반지름
    float arrowPlayerDistance = 0.0f; // 두 원의 중심 사이 거리
    float arrowRadius = 0.5f;         // 화살 원의 반지름
    float playerRadius = 1.0f;        // 플레이어 원의 반지름

    // ★ 중복 피격 및 연쇄 호출을 막아주는 안전장치 플래그
    bool hasDamaged = false;

    // 게임 시작 시 한번 실행
    void Start()
    {
        // 씬 전체에서 '내 네트워크 뷰(IsMine)를 가진 진짜 내 캐릭터'를 정확히 찾아옵니다.
        FindMyLocalPlayer();
    }

    // 씬에 있는 플레이어 중 '내 캐릭터'만 정확히 찾아내는 함수
    void FindMyLocalPlayer()
    {
        PlayerController[] allPlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        foreach (var p in allPlayers)
        {
            if (p.photonView.IsMine)
            {
                player = p.gameObject;
                break;
            }
        }
    }

    // 매 프레임 실행
    void Update()
    {
        // 1. 화살을 아래 방향으로 이동
        transform.Translate(0, -arrowSpeed * Time.deltaTime, 0);

        // 2. 화면 밖으로 나온 화살 소멸시키기
        if (transform.position.y < -5.0f)
        {
            PhotonView pv = GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            return;
        }

        // 혹시 플레이어가 아직 안 찾아졌다면 다시 탐색 시도
        if (player == null)
        {
            FindMyLocalPlayer();
            if (player == null) return;
        }
        
        // 4. 화살과 플레이어의 중심 좌표 구하기
        arrowCirclePoint = transform.position;
        playerCirclePoint = player.transform.position;

        // 5. 두 원의 중심을 연결하는 벡터 계산
        arrowPlayerDirection = playerCirclePoint - arrowCirclePoint;
        
        // 6. 두 원의 중심 사이의 거리 구하기 (벡터의 길이)
        arrowPlayerDistance = arrowPlayerDirection.magnitude;

        // 7. 충돌 판정: 두 원의 중심 거리가 (화살 반지름 + 플레이어 반지름) 보다 작거나 같으면 충돌!
        if (arrowPlayerDistance <= arrowRadius + playerRadius)
        {
            // ★ 아직 데미지를 주지 않은 상태라면 딱 한 번만 즉사 로직 실행!
            if (!hasDamaged)
            {
                hasDamaged = true; // 잠금!

                // 충돌한 대상이 플레이어라면
                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    // 내 화면에 생성된 내 캐릭터인 경우에만 즉사 데미지(1.0) 부여
                    if (playerController.photonView.IsMine)
                    {
                        playerController.TakeDamage(1.0f); // 한 방 컷!
                    }
                }
        
                // 화살 파괴 (네트워크 오브젝트이므로 안전하게 처리)
                PhotonView pv = GetComponent<PhotonView>();
                if (pv != null)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.Destroy(gameObject);
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
