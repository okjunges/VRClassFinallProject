# 스크립트 명세서 (Script Documentation)

이 문서는 `Assets/stray/Scripts/` 경로에 있는 모든 C# 스크립트의 명세서입니다.

---

## 1. GameManager.cs
게임의 전체적인 흐름과 상태(Phase)를 관리하는 싱글톤 클래스입니다.

### 설명
- **역할**: Survivor(생존자)와 Chaser(추적자) 페이즈를 관리하고, 타이머를 업데이트하며, 승리/패배 조건을 체크합니다.
- **싱글톤**: `Instance` 프로퍼티를 통해 어디서든 접근 가능합니다.

### 필드 (Fields)
| 접근 제어자 | 타입 | 이름 | 설명 |
| :--- | :--- | :--- | :--- |
| `public static` | `GameManager` | `Instance` | GameManager의 싱글톤 인스턴스입니다. |
| `public` | `GamePhase` | `CurrentPhase` | 현재 게임의 진행 상태입니다. (Survivor, Chaser, GameOver) |
| `[SerializeField] private` | `float` | `survivorPhaseDuration` | 생존자 페이즈의 지속 시간(초)입니다. 기본값: 60f |
| `[SerializeField] private` | `float` | `chaserPhaseDuration` | 추적자 페이즈의 지속 시간(초)입니다. 기본값: 60f |
| `public` | `float` | `phaseTimer` | 현재 페이즈의 남은 시간입니다. |

### 프로퍼티 (Properties)
| 타입 | 이름 | 설명 |
| :--- | :--- | :--- |
| `float` | `SurvivorPhaseDuration` | `survivorPhaseDuration`에 대한 접근/수정 프로퍼티입니다. |
| `float` | `ChaserPhaseDuration` | `chaserPhaseDuration`에 대한 접근/수정 프로퍼티입니다. |

### 메서드 (Methods)
| 메서드 서명 | 입력 (Input) | 출력 (Output) | 동작 설명 |
| :--- | :--- | :--- | :--- |
| `void StartSurvivorPhase()` | 없음 | 없음 | 게임 상태를 `Survivor`로 변경하고 타이머를 초기화합니다. 플레이어 모델을 숨깁니다. |
| `void StartChaserPhase()` | 없음 | 없음 | 게임 상태를 `Chaser`로 변경하고 타이머를 초기화하며, `ChaserAI`에게 추적 시작을 알립니다. 플레이어 모델을 표시하고 Idle 애니메이션을 재생합니다. |
| `void SkipPhase()` | 없음 | 없음 | 현재 페이즈의 남은 시간을 0으로 만들어 즉시 다음 페이즈로 넘깁니다. (Debug용) |
| `void SurvivorWin()` | 없음 | 없음 | 게임 상태를 `GameOver`로 변경하고 승리 로그를 출력합니다. |
| `void SurvivorLose()` | 없음 | 없음 | 게임 상태를 `GameOver`로 변경하고 패배 로그를 출력합니다. |

---

## 2. MapControl.cs
타일 그리드를 관리하고 타일 회전 로직을 처리하는 클래스입니다.

### 설명
- **역할**: 맵의 타일들을 2차원 배열로 관리하고, 플레이어의 상호작용 요청 시 타일을 회전시킵니다. 회전 전 `GridVerify`를 통해 유효성을 검사합니다.

### 필드 (Fields)
| 접근 제어자 | 타입 | 이름 | 설명 |
| :--- | :--- | :--- | :--- |
| `[SerializeField] private` | `Tile[]` | `tiles` | 맵에 존재하는 모든 타일 오브젝트들의 배열입니다. 초기화 시 "Tile" 태그로 자동 검색됩니다. |
| `[SerializeField] private` | `int` | `width` | 그리드의 가로 크기입니다. 기본값: 5 |
| `[SerializeField] private` | `int` | `height` | 그리드의 세로 크기입니다. 기본값: 5 |

### 메서드 (Methods)
| 메서드 서명 | 입력 (Input) | 출력 (Output) | 동작 설명 |
| :--- | :--- | :--- | :--- |
| `void InitializeGrid()` | 없음 | 없음 | "Tile" 태그를 가진 객체들을 찾아 `tiles` 배열을 구성하고, 좌표에 맞춰 `tileGrid`에 매핑합니다. |
| `bool TryRotateTile(Tile tile)` | `Tile tile` (회전할 타일) | `bool` (성공 여부) | 1. Survivor 페이즈인지 확인합니다.<br>2. 타일을 시계방향으로 회전시킵니다.<br>3. `GridVerify`로 맵 유효성을 검사합니다.<br>4. 유효하지 않으면 타일을 원래대로 되돌리고 `false`를 반환합니다. |
| `Tile GetTileAt(int x, int y)` | `int x`, `int y` | `Tile` | 해당 그리드 좌표에 있는 타일 객체를 반환합니다. |

---

## 3. GridVerify.cs
맵의 유효성(추적자의 경로 존재 여부)을 검증하는 클래스입니다.

### 설명
- **역할**: BFS(너비 우선 탐색) 알고리즘을 사용하여 추적자가 플레이어(또는 목표 지점)까지 도달할 수 있는지 확인합니다.

### 필드 (Fields)
| 접근 제어자 | 타입 | 이름 | 설명 |
| :--- | :--- | :--- | :--- |
| `[SerializeField] private` | `Vector2Int` | `chaserStartPos` | 추적자의 시작 그리드 좌표입니다. 기본값: (0, 0) |

### 메서드 (Methods)
| 메서드 서명 | 입력 (Input) | 출력 (Output) | 동작 설명 |
| :--- | :--- | :--- | :--- |
| `bool IsMapValid(Tile[,] grid, int width, int height)` | `Tile[,] grid` (타일 배열), `int width`, `int height` | `bool` (유효 여부) | 현재 플레이어 위치를 가져와 `HasPath`를 호출하여 경로 존재 여부를 반환합니다. |
| `bool HasPath(...)` | `grid`, `width`, `height`, `start`, `end` | `bool` | BFS를 수행하여 `start`에서 `end`까지 연결된 경로가 있는지 탐색합니다. 벽(`hasWall`)이 없는 방향으로만 이동 가능합니다. |

---

## 4. Tile.cs
개별 타일의 데이터와 동작을 정의하는 클래스입니다.

### 설명
- **역할**: 타일의 4방향(북, 동, 남, 서)에 대한 벽, 문, 램프 상태를 저장하고, 회전 기능을 제공합니다.

### 구조체 (Structs)
- **Side**: 타일의 한 면에 대한 정보를 담습니다. (`hasWall`, `hasDoor`, `hasLampOn`, `hasLampOff`)

### 필드 (Fields)
| 접근 제어자 | 타입 | 이름 | 설명 |
| :--- | :--- | :--- | :--- |
| `public` | `Side` | `north`, `east`, `south`, `west` | 각 방향의 면 정보입니다. |
| `public` | `int` | `x`, `y` | 그리드 상의 좌표입니다. |

### 메서드 (Methods)
| 메서드 서명 | 입력 (Input) | 출력 (Output) | 동작 설명 |
| :--- | :--- | :--- | :--- |
| `void RotateClockwise()` | 없음 | 없음 | 타일 오브젝트를 Y축으로 90도 회전시키고, 논리적인 `Side` 데이터(north, east...)도 시계방향으로 교체합니다. |
| `Side GetSide(int direction)` | `int direction` (0:N, 1:E, 2:S, 3:W) | `Side` | 해당 방향의 `Side` 데이터를 반환합니다. |

---

## 5. PlayerControl.cs
플레이어의 이동과 상호작용을 처리하는 클래스입니다.

### 설명
- **역할**: FPS 시점 제어, WASD 이동, 'E' 키 상호작용, 페이즈별 모델 가시성 제어 등을 담당합니다.

### 필드 (Fields)
| 접근 제어자 | 타입 | 이름 | 설명 |
| :--- | :--- | :--- | :--- |
| `public static` | `PlayerControl` | `Instance` | 싱글톤 인스턴스입니다. |
| `public` | `Vector2Int` | `gridPosition` | 현재 플레이어의 그리드 좌표입니다. |
| `[SerializeField] private` | `float` | `moveSpeed` | 이동 속도입니다. |
| `[SerializeField] private` | `float` | `runSpeed` | 달리기 속도입니다. |
| `[SerializeField] private` | `float` | `mouseSensitivity` | 마우스 감도입니다. |
| `[SerializeField] private` | `Camera` | `playerCamera` | 플레이어 카메라입니다. |
| `[SerializeField] private` | `float` | `interactionDistance` | 상호작용 거리(현재 사용 안함, 타일 위 상호작용으로 변경됨) |
| `[SerializeField] private` | `MapControl` | `mapControl` | MapControl 참조입니다. |
| `[SerializeField] private` | `Animator` | `animator` | 애니메이터 컴포넌트입니다. |
| `[SerializeField] private` | `GameObject` | `playerModel` | 플레이어의 시각적 모델(자식 오브젝트)입니다. |

### 프로퍼티 (Properties)
| 타입 | 이름 | 설명 |
| :--- | :--- | :--- |
| `float` | `MoveSpeed` | `moveSpeed` 접근/수정 |
| `float` | `RunSpeed` | `runSpeed` 접근/수정 |
| `float` | `MouseSensitivity` | `mouseSensitivity` 접근/수정 |

### 메서드 (Methods)
| 메서드 서명 | 입력 (Input) | 출력 (Output) | 동작 설명 |
| :--- | :--- | :--- | :--- |
| `void Start()` | 없음 | 없음 | 카메라를 플레이어 자식으로 설정하고 마우스 커서를 잠급니다. |
| `void HandleMouseLook()` | 없음 | 없음 | 마우스 입력으로 시점을 회전합니다. `LAlt` 키를 누르면 커서를 풀고 시점 이동을 일시 정지합니다. |
| `void HandleMovement()` | 없음 | 없음 | `GetAxisRaw`를 사용하여 WASD 이동을 처리합니다. 입력이 없으면 관성을 제거합니다. |
| `void HandleInteraction()` | 없음 | 없음 | `E` 키를 누르면 현재 밟고 있는 타일(`gridPosition`)을 회전시킵니다. |
| `void SetModelVisibility(bool isVisible)` | `bool` | 없음 | 플레이어 모델(`playerModel`)의 활성화 상태를 설정합니다. |
| `void PlayIdle()` | 없음 | 없음 | Idle 애니메이션 트리거를 실행합니다. |
| `void Die()` | 없음 | 없음 | Death 애니메이션 트리거를 실행하고 컴포넌트를 비활성화합니다. |

---

## 6. ChaserAI.cs
추적자(적)의 AI를 담당하는 클래스입니다.

### 설명
- **역할**: Chaser 페이즈가 되면 플레이어를 향해 이동하고, 플레이어와 접촉 시 게임을 종료시킵니다.

### 필드 (Fields)
| 접근 제어자 | 타입 | 이름 | 설명 |
| :--- | :--- | :--- | :--- |
| `public static` | `ChaserAI` | `Instance` | 싱글톤 인스턴스입니다. |
| `[SerializeField] private` | `float` | `speed` | 이동 속도입니다. |
| `[SerializeField] private` | `float` | `catchDistance` | 플레이어를 잡았다고 판단하는 거리입니다. |

### 프로퍼티 (Properties)
| 타입 | 이름 | 설명 |
| :--- | :--- | :--- |
| `float` | `Speed` | `speed` 접근/수정 |

### 메서드 (Methods)
| 메서드 서명 | 입력 (Input) | 출력 (Output) | 동작 설명 |
| :--- | :--- | :--- | :--- |
| `void StartChasing()` | 없음 | 없음 | 추적 상태(`isChasing`)를 true로 설정하고 타겟(플레이어)을 설정합니다. |
| `void Update()` | 없음 | 없음 | 추적 중일 때 플레이어 방향으로 이동하고, 거리가 `catchDistance` 이내면 `GameManager.SurvivorLose()`를 호출합니다. |

---

## 7. UIManager.cs
모든 UI 요소(디버그 창, 상호작용 프롬프트)를 중앙에서 관리하는 클래스입니다.

### 설명
- **역할**: `OnGUI`를 사용하여 디버그 윈도우와 상호작용 텍스트를 그립니다. `~` 키로 디버그 UI를 토글합니다.
- **싱글톤**: `Instance`를 통해 접근 가능합니다.

### 필드 (Fields)
| 접근 제어자 | 타입 | 이름 | 설명 |
| :--- | :--- | :--- | :--- |
| `public static` | `UIManager` | `Instance` | 싱글톤 인스턴스입니다. |
| `[SerializeField] private` | `bool` | `showDebugUI` | 디버그 UI 표시 여부입니다. |
| `[SerializeField] private` | `Rect` | `windowRect` | 디버그 윈도우의 위치와 크기입니다. |
| `[SerializeField] private` | `Vector2` | `promptPosition` | 상호작용 프롬프트의 화면 상대 위치입니다. |
| `[SerializeField] private` | `int` | `promptFontSize` | 상호작용 프롬프트 폰트 크기입니다. |
| `[SerializeField] private` | `Color` | `promptTextColor` | 상호작용 프롬프트 텍스트 색상입니다. |

### 메서드 (Methods)
| 메서드 서명 | 입력 (Input) | 출력 (Output) | 동작 설명 |
| :--- | :--- | :--- | :--- |
| `void SetInteractionPrompt(bool isVisible, string text)` | `bool`, `string` | 없음 | 상호작용 프롬프트의 표시 여부와 텍스트를 설정합니다. |
| `void OnGUI()` | 없음 | 없음 | 활성화된 UI 요소들을 그립니다. |

---

## 8. InteractionPrompt.cs
플레이어가 특정 구역(Trigger)에 진입했을 때 상호작용 안내를 요청하는 클래스입니다.

### 설명
- **역할**: Trigger 진입/이탈을 감지하여 `UIManager`에게 프롬프트 표시/숨김을 요청합니다.

### 필드 (Fields)
| 접근 제어자 | 타입 | 이름 | 설명 |
| :--- | :--- | :--- | :--- |
| `[SerializeField] private` | `string` | `triggerTag` | 감지할 태그입니다. 기본값: "Trigger" |
| `[SerializeField] private` | `string` | `promptText` | 표시할 텍스트입니다. 기본값: "[E]로 상호작용" |

### 메서드 (Methods)
| 메서드 서명 | 입력 (Input) | 출력 (Output) | 동작 설명 |
| :--- | :--- | :--- | :--- |
| `void OnTriggerEnter(Collider other)` | `Collider` | 없음 | 태그가 일치하면 `UIManager.Instance.SetInteractionPrompt(true)`를 호출합니다. |
| `void OnTriggerExit(Collider other)` | `Collider` | 없음 | 태그가 일치하면 `UIManager.Instance.SetInteractionPrompt(false)`를 호출합니다. |
