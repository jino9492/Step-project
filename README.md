# STEP : 2021 2학기 진로 

## 프로젝트 정보 ##
```
Developer | 정진오, 서현아, 이정빈
Language  | C#
Tool      | Unity
```

## 프로젝트 개요 ##

스토리 기반의 2D 탑다운(TopDown) 게임

불 꺼진 폐건물을 탈출해야 하는 스토리를 가진 공포게임으로 손전등과 사운드를 이용하여 공포분위기를 조성

사운드(발소리)를 이용해 괴물을 피하는 것이 핵심

![image](https://user-images.githubusercontent.com/66864237/152681925-171d7069-c248-4eee-b922-26a3dd0e26d3.png)

## 프로젝트 설명 ##

### 손전등 ###
![image](https://user-images.githubusercontent.com/66864237/152682008-1803d4d0-93e4-4590-9a40-137c255127db.png)

* 어두운 폐건물 내에서 의지할 수 있는 유일한 빛
* 괴물은 이 빛을 싫어한다는 설정이 있어서 괴물이 오는 방향을 예측하고 바라보고 있으면 괴물이 사라지도록 설계

### 괴물(AI) ###
![image](https://user-images.githubusercontent.com/66864237/152682221-61b1194e-a767-40fd-bedd-070d3872e767.png)
![image](https://user-images.githubusercontent.com/66864237/152682128-2fbf6830-f0c6-4086-957e-1bf8a24db385.png)

* 폐건물 안을 돌아다니는 정체불명의 괴물
* 게임 내내 플레이어를 쫒아다니며, 플레이어는 소리를 이용해 괴물에게서 도망쳐야 함

### 사운드 ###
![image](https://user-images.githubusercontent.com/66864237/152682239-3ba200f2-0e74-4638-bf0f-c46e6ef19b4b.png)
![image](https://user-images.githubusercontent.com/66864237/152682245-ed4d44aa-966e-4ecd-8cfe-09089c961c87.png)

* 괴물은 이동 시 범위 안에 있는 플레이어에게 발소리를 들려줌
* Audio Source Spatial Blend 기능으로 좌우음향을 제공
* 방 안으로 들어가면 필터가 씌어지면서 방에 들어와있는 느낌을 강화

### 상호작용 ###
![image](https://user-images.githubusercontent.com/66864237/152682363-bbaf27af-f431-443d-99f4-0469b14d3bcb.png)

* 곳곳에 빛나고 있는 오브젝트나 문, 엘리베이터 등에 Z를 눌러 상호작용

### 지도 ###

* M을 눌러 현재 층의 모양을 지도를 통해서 확인
* 지워진 부분은 장애물, 직접적으로 알려주지는 않으므로 플레이어 본인이 파악해야 함
