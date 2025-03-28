# 🏃 Study Run
무한히 이어지는 맵에서 장애물을 피하는 게임입니다. 


![Image](https://github.com/user-attachments/assets/ab4f5250-0bce-456c-8aca-4c79661d7cb3)

## 🔖 와이어프레임
![제목 없는 다이어그램 drawio](https://github.com/user-attachments/assets/47d03aa0-dd79-46a7-a781-6f598c030c2a)

## 📑 기능 담당
### 권수민
  - Coroutine을 사용하여 비동기적으로 씬을 로드하고 진행도를 업데이트하는 로딩 바 구현
  - 체력, 점수, 최고 점수, 정지 UI 구현 

### 송민경 
  - 포션의 공통된 기능을 담은 Potion 클래스와 인터페이스를 설계한 뒤, 이를 상속 및 구현하여 Speed Potion과 Health Potion이 각각의 고유 효과를 다형성을 통해 실행할 수 있도록 구성.
  - 가속 포션 획득 시 코루틴 함수를 실행하여 무적효과에 진입하였다는 걸 보여주도록 구현.
  - 게임 시작시 카메라의 움직임이 부드럽게 이동하도록 구현.


### 신소현
  - SoundManager를 통해 BGM, SFX를 관리하고, 각 상황에 맞는 사운드를 출력할 수 있도록 구현
  - 업적 리스트를 생성하고, 리스트를 AchievementManager에서 받아 Json으로 업적 달성 상황 저장 / 로드가 가능하도록 구현

### 박호준
  - InputSystem 을 활용해 플레이어 조작
  - 캐릭터 애니메이션

### 정순원
  - ChunkLoadManager를 통해 오브젝트 풀링 시스템을 활용한 청크 기반 랜덤 맵 생성 시스템 구현
  - Obstacle에서 Box Collision을 쉽게 조작하고 맵 청크 내에 배치가 용이하도록 하는 시스템 구현

## 
