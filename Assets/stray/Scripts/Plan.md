# Plan

Unity 2023.3.42f
1인칭 퍼즐 게임

Phase 1. 생존자(플레이어)가 주어진 시간 내에 미로를 지나며 추적자의 진로를 방해하도록 회전 가능한 타일을 회전시킴
Phase 2. 추적자가 생존자를 쫓아 주어진 시간 내에 미로를 탐색함

추적자가 생존자를 찾아내면 패배, 시간 내에 찾지 못하면 승리.

## Map

특성
- 상호작용을 통해 지정된 맵 타일(+ 연결된 벽)을 회전할 수 있음.
- 각 타일이 정사각형인 격자 미로식
  
조건
- 추적자의 경로가 완전히 막혀서는 안 됨.
    (Graph화 해서 검증)

## Components

- GameManager.cs
    게임의 흐름을 관리

- PlayerControl.cs
    플레이어의 움직임, 상호작용 정의

- /Map/MapControl.cs
    상호작용 입력을 받으면 해당되는 타일의 상태를 변경

- /UI/UIManager.cs
    UI를 관리

- Option: MapGenerator.cs
    자동 생성 알고리즘 구현
    - GridVerify.cs
        맵 검증에 필요한 함수 정의
        Select: 1. 전체 맵 변경 횟수를 미리 정해놓음.
                2. 맵이 Invalid해지면 모든 상호작용 비활성화.


## Prefabs

- Maps[]
    사전 생성된 맵 (2~3개)

- MapTiles
    - Floor
    - Wall
    - Door
    - Lamp

## TODO

### 1. 시스템 복구 및 재구현 (System Restoration)
- [ ] **UIManager**:
    - 남은 시간(Timer) 및 현재 페이즈(Phase) 표시
    - 게임 오버 / 승리 화면

### 2. 맵 시스템 보완 (Map System)
- [ ] **MapInit**: 씬 시작 시 Crossing/Door 자동 설정 및 초기화

### 3. 게임 루프 완성 (Game Loop Polish)
- [ ] **Start Scene**: 메인 메뉴 (Start, Quit)
- [ ] **Pause Menu**: ESC 키 일시정지 및 메뉴
- [ ] **Data Persistence**: 옵션 설정 저장 (볼륨, 감도 등)