# Captain Monster Modifiers

발헤임(Valheim)의 몬스터에 다양한 랜덤 속성을 부여하는 BepInEx 모드입니다.

## 원작자 (Original Author)

**warpalicious** — 원본 MonsterModifiers 모드 제작자  
본 모드는 warpalicious의 원작을 기반으로 KorCaptain이 확장 수정한 버전입니다.

---

## 주요 기능

### 몬스터 속성 시스템

레벨 2 이상 몬스터는 최대 N개(설정 가능)의 랜덤 속성을 보유합니다.  
속성은 몬스터 HP 바 위의 아이콘으로 표시됩니다.

### 저항 속성 (방어 계열)

| 속성 | 효과 |
|------|------|
| **PierceImmunity** | 관통 데미지 -70% 저항 |
| **SlashImmunity** | 베기 데미지 -70% 저항 |
| **BluntImmunity** | 둔기 데미지 -70% 저항 |
| **ElementalImmunity** | 화염/냉기/번개/독/영혼 데미지 -70% 저항 |
| **StaggerImmune** | 경직(스태거) 면역 |
| **PersonalShield** | 개인 방어막 |
| **ShieldDome** | 방어 돔 생성 |

### 공격 속성 (공격 계열)

| 속성 | 효과 |
|------|------|
| **StaminaSiphon** | 플레이어 스태미나 흡수 |
| **EitrSiphon** | 플레이어 에이트르 흡수 |
| **FoodDrain** | 플레이어 음식 고갈 |
| **IgnoreArmor** | 방어력 무시 |
| **ShieldBreaker** | 방패 파괴 |
| **Forceful** | 강력한 넉백 (5배) |
| **Knockback** | 7미터 넉백 공격 |
| **FastAttackSpeed** | 빠른 공격 속도 |
| **FireInfused / FrostInfused / PoisonInfused / LightningInfused** | 원소 속성 공격 추가 |
| **Vampiric** | 공격 시 체력 흡수 |
| **Absorption** | 저항 속성 피해를 체력으로 흡수 |
| **BloodLoss** | 출혈 상태효과 |
| **Wet** | 젖음 상태효과 |

### 이동/감지 속성

| 속성 | 효과 |
|------|------|
| **FastMovement** | 이동 속도 증가 |
| **DistantDetection** | 원거리 감지 |
| **Quiet** | 소음 제거 (은신 특화) |

### 사망 속성 (Death Effects)

| 속성 | 효과 |
|------|------|
| **FireDeath / FrostDeath / PoisonDeath** | 사망 시 해당 원소 폭발 |
| **HealDeath** | 사망 시 주변 몬스터 치유 |
| **StaggerDeath** | 사망 시 주변 경직 |
| **TarDeath** | 사망 시 타르 투척 |

### 특수 속성

| 속성 | 효과 |
|------|------|
| **SoulEater** | 영혼 흡수 |
| **RemoveStatusEffect** | 플레이어 상태효과 제거 |

---

## 설정 (Config)

설정 파일: `BepInEx/config/KorCaptain.Captain_Monster_modifiers.cfg`

### Balance 섹션

| 항목 | 기본값 | 설명 |
|------|--------|------|
| Max Modifiers | 5 | 몬스터 1마리당 최대 속성 수 |

### Monster_Base 섹션

| 항목 | 기본값 | 설명 |
|------|--------|------|
| Monster Attack 효과 % | 100 | 공격 속성 전체 성능 조절 (0=효과 없음) |
| Monster Defense & Etc 효과 % | 100 | 방어/저항 속성 전체 성능 조절 (0=저항 없음) |

> **예시**: `Monster Defense & Etc 효과 % = 0` 설정 시 PierceImmunity 몬스터가 관통 데미지를 100% 받음

---

## 서버 동기화

서버와 클라이언트 모두 동일한 버전이 설치되어야 합니다.  
버전 불일치 시 접속이 차단됩니다.  
설정값은 서버 설정이 클라이언트에 자동 동기화됩니다.

---

## 설치

1. BepInEx가 설치된 발헤임 디렉토리에 `CaptainMonsterModifiers.dll`을 `BepInEx/plugins/` 폴더에 복사
2. Jotunn 라이브러리 필요

---

## 버전 히스토리

### v1.3.0 (KorCaptain 수정판)
- 면역(Immune) → -70% 저항으로 변경 (관통/베기/둔기/원소)
- Monster_Base Config 추가: Attack 효과 %, Defense & Etc 효과 %
- 새 속성 추가: Knockback (7미터 넉백)
- 서버 싱크 Config 동기화

### v1.2.6 (warpalicious 원작)
- 원작 최종 버전

---

## 크레딧

- **원작자**: warpalicious (원본 MonsterModifiers)
- **수정/확장**: KorCaptain
