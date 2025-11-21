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
    Graph화 해서 검증

# Components

- GameManager.cs
    게임의 흐름을 관리

- PlayerControl.cs
    플레이어의 움직임, 상호작용 정의

- MapControl.cs
    상호작용 입력을 받으면 해당되는 타일의 상태를 변경

- GridVerify.cs
    맵 검증에 필요한 함수 정의
    Select: 1. 전체 맵 변경 횟수를 미리 정해놓음. << 1인칭이라 이거 해야할듯
            2. 따로 표시하지 않고 맵이 Invalid해지면 모든 상호작용 비활성화.

- Option: MapGenerator.cs
    자동 생성 알고리즘 구현

# Prefabs

- Maps[]
    사전 생성된 맵 (2~3개)

- MapTiles
    - Floor
    - Wall
    - Door
    - Lamp