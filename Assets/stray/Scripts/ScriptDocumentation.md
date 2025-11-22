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
## 9. CrossingController.cs
상호작용 시 연결된 문(Door) 객체들을 번갈아가며 활성화/비활성화하는 클래스입니다.

### 설명
- **역할**: `PlayerControl`로부터 상호작용 신호를 받으면 리스트에 등록된 문들의 `Active` 상태를 토글합니다.
- **사용법**: Trigger 객체에 이 컴포넌트를 추가하고, `doors` 리스트에 제어할 문 오브젝트들을 할당하세요. 문들이 번갈아 열리게 하려면 초기 상태를 서로 다르게(하나는 켜고 하나는 끄고) 설정하면 됩니다.

### 필드 (Fields)
| 접근 제어자 | 타입 | 이름 | 설명 |
| :--- | :--- | :--- | :--- |
| `[SerializeField] private` | `List<GameObject>` | `doors` | 제어할 문 오브젝트들의 리스트입니다. |

### 메서드 (Methods)
| 메서드 서명 | 입력 (Input) | 출력 (Output) | 동작 설명 |
| :--- | :--- | :--- | :--- |
| `void Interact()` | 없음 | 없음 | 등록된 모든 문의 `activeSelf` 상태를 반전시킵니다. |
