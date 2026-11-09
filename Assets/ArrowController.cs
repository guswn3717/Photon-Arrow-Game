/*  화살을 화면 위에서 아래로 떨어뜨리는 스크립트
 *      - 화살이 위에서 아래로 1초에 하나씩 떨어지는 기능 --> transform.Translate()
 *      - 화살이 게임화면 밖으로 나오면 화살 오브젝트를 소멸시키는 기능 --> Destroy()
*/

using UnityEngine;

public class ArrowController : MonoBehaviour
{
    //화살 이동 속도 (초당 Unity 단위)
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

    // 게임 시작 시 한번 실행
    void Start()
    {
        player = GameObject.Find("player");
    }

    // 매 프레임 실행
    void Update()
    {
        //1. 화살을 아래 방향으로 이동
        //  - Translate 메서드 : 오브젝트를 현재 좌표에서 인수 값만큼 이동시키는 메서드
        //  - Y좌표에 (-arrowSpeed * Time.deltaTime)를 지정하면 오브젝트를 조금씩 위에서 아래로 움직인다
        //  - 프레임 마다 등속으로 낙하시킨다
        transform.Translate(0, -arrowSpeed * Time.deltaTime, 0);

        //2. 화면 맊으로 나온 화살 소멸시키기
        //  - 화살을 내버려 두면 화면 밖으로 나가게 되고, 눈에 보이지는 않지만 계속 떨이짐
        //  - 화살이 보이지 않는 곳에서 계속 떨이지면 컴퓨터 역시 계산을 해야하므로 메모리 낭비
        //  - 메모리가 낭비되지 않고록 화살이 화면 밖으로 나가면 오브젝트를 소멸시켜야 함
        //3. Destory 메서드 : 매개변수로 전달한 오브젝트를 삭제
        //  - 매개변수로 자신(화살 오브젝트)을 가르키는 gameObject 변수를 전달하므로 화실이
        //  - 화면 밖으로 나갔을 때 자기 자신을 소멸시킴
        if (transform.position.y < -5.0f)
        {
            Destroy(gameObject);
            
            return;
        }

        if (player == null)
        {
            return;
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
            GameObject gameDirector = GameObject.Find("GameDirector");
            
            // 방법 A: 실행할 때 알아서 씬에서 GameDirector를 찾아오게 수정 // FindObjectOfType -> FindFirstObjectByType 유니티6 기능으로 바꿈
            GameDirector director = FindFirstObjectByType<GameDirector>(); 
            if (director != null)
            {
                director.DecreaseHp();
            }
            else
            {
                Debug.LogError("씬에 GameDirector가 존재하지 않습니다!");
            }

            
            Destroy(gameObject);  // 화살 본인도 파괴 (선택 사항)
        }

    }
}
