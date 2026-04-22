# Rescue
<img width="1530" height="956" alt="image" src="https://github.com/user-attachments/assets/69720ca6-7774-44cb-abac-2b9ca673c727" />

<div align="center">

# 🚑 Field Game Rescue
### 3D 생존·구출 게임 — Unity 2022.3 / C#

[![Unity](https://img.shields.io/badge/Unity-2022.3-222C37?style=for-the-badge&logo=unity&logoColor=white)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-68217A?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![GitHub](https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white)](https://github.com/dae-won-kim/Field-Game-Rescue)

[▶ 게임플레이 영상 보기](https://youtu.be/YoMkUSNgSEE)

</div>

---

## 📖 프로젝트 개요

C# 및 Unity 3D 개발 역량 확보를 목적으로 제작한 **1인 개발 3D 생존·구출 게임**입니다.  
섬에 고립된 부상자를 구출하기 위해 자원을 수집·운반하고 구급차를 수리하면서,  
**제한 시간 내 NPC 체력 게이지를 채워 탈출**하는 것이 목표입니다.

---

## 🗂️ 씬 구성

| Scene | 설명 |
|-------|------|
| **Title Scene** | 게임 시작 화면 |
| **Loading Scene** | 리소스 로드 및 전환 화면 |
| **Game Scene** | 메인 게임플레이 |

---

## 👤 개발 정보

| 항목 | 내용 |
|------|------|
| 개발 형태 | 1인 개발 (기획 · 개발 · QA) |
| 개발 기간 | 2025.03 — 2025.06 |
| 엔진 | Unity 2022.3 |
| 언어 | C# |

---

## 🛠️ 기술 스택 및 구현 키워드

### Singleton
- `GameStatus`, `AudioController`를 씬 간 단일 인스턴스로 유지
- `Awake()`에서 중복 인스턴스 즉시 `Destroy` → 전역 상태 일관성 보장
- `AudioController`에 `DontDestroyOnLoad` 적용 → 씬 전환 중 BGM 연속 재생

### Object Pool
- `Queue<IObject>` 기반으로 Coin · Trap · FXObject 재사용
- Lazy Instantiation 방식 → 풀이 비었을 때만 신규 생성, `Instantiate/Destroy` 비용 제거
- `IObject` 추상 클래스를 공통 인터페이스로 사용 → `ObjectPool`이 구체 타입에 의존하지 않음 (OCP 준수)

### State Machine
- `Step` enum(`Move / Repairing / Eating / Emotion / Rescue`)으로 플레이어 행동 명확히 분리
- `nextStep` 버퍼 도입 → 상태 진입(`onEnter`) 처리가 정확히 1회만 실행되도록 보장
- `stepTimer`로 각 상태의 지속 시간을 독립적으로 관리

### Observer
- `SceneManager.sceneLoaded` C# 델리게이트 이벤트 구독
- 씬 전환 시 BGM 자동 교체 → 씬과 오디오 시스템 간 직접 결합 제거
- 새 씬 추가 시 `PlayBGMForScene()` 내 `switch` 분기만 확장하면 됨

### IObject 추상 클래스
- `OnEnter / OnExit / OnInit / OnDisabled` 4단계 수명주기 훅 강제화
- `Trap` 코루틴 종료 후 `PoolObject()` 직접 호출 → 외부 관리자 없이 자율 반환

### FeverTime System
- NPC 구출 완료 시 `GameState`를 `Normal → FeverTime`으로 전환
- `Coroutine`으로 7초간 이동 속도 증가·체력 회복·스트레스 초기화 버프 비동기 관리
- `GameStatus.StartFeverTime() / EndFeverTime()` 으로 전역 상태 변경 → 다른 시스템이 `IsFeverTime` 프로퍼티만으로 분기

### 아이템 생성 (2가지 방식)
- **Timer 기반 `Instantiate()`**: 일정 시간 경과마다 일반 아이템 신규 생성
- **Object Pooling**: Coin · Trap은 `Queue<IObject>` 풀에서 재사용 → GC 부하 최소화

---

## 🗺️ 레벨 디자인 — 유저 테스트 기반 개선

유저 테스트 피드백을 반영하여 레벨을 반복 개선했습니다.

| Category | Feedback (Problem) | Solution (Action) |
|----------|--------------------|-------------------|
| UX & UI | 키 안내 불일치, 목표 불명확 | UI 가이드 동기화 및 미션 마커 추가 |
| Balance | 체력·스트레스 소모가 빨라 과도한 난이도 | 소모율 재조정 및 아이템 스폰 빈도 최적화 |
| Game Loop | 고스트레스 시 이동 저하로 데스 스파이럴 발생 | 이동 패널티 수치 조정 |
| Pacing | 맵이 넓어 이동 시간이 길고 지루함 | 맵 레이아웃 압축으로 조우 밀도 향상 |

---

## 💬 개발 회고

### ✅ 느낀 점
- 1인 개발을 통해 **기획·아트·개발 파트 분담과 협업의 필요성**을 직접 체감
- QA 및 베타 테스트의 중요성 확인
  - 개발자의 플레이 경험과 유저의 플레이 경험이 크게 다름
  - 난이도, 인게임 설명 등 전반에 걸친 **피드백 루프**가 필수임을 학습

### ⚠️ 한계 및 아쉬운 점
- 그래픽 에셋의 한계로 인해 일부 미완성 요소 존재

---

<div align="center">
<sub>Made with ❤️ by dae-won-kim · Unity 2022.3 · C#</sub>
</div>

