# G Play Studio Template Project

## Dökümantasyon

[Game Manager](https://github.com/G-Play-Studio/GPlay-Template-Project#game-manager)

[Event Manager](https://github.com/G-Play-Studio/GPlay-Template-Project#event-manager)

[Custom Event Manager](https://github.com/G-Play-Studio/GPlay-Template-Project#custom-event-manager)

[Camera Manager](https://github.com/G-Play-Studio/GPlay-Template-Project#camera-manager)

[Object Pooling](https://github.com/G-Play-Studio/GPlay-Template-Project#object-pooling)

[Player Economy](https://github.com/G-Play-Studio/GPlay-Template-Project#player-economy)

[Character Controllers](https://github.com/G-Play-Studio/GPlay-Template-Project#character-controllers)


## Ana Sahne

![Screen Shot 2021-12-07 at 10 57 48](https://user-images.githubusercontent.com/50348497/144991029-2c8a20a5-725d-49d5-b6d3-4f515ed72f1e.jpg)

Hiyerarşi ve ana sahne bir proje için başlangıçta gerekli olan ögeleri içeriyor.


### Level Manager

![Screen Shot 2021-12-07 at 11 17 20](https://user-images.githubusercontent.com/50348497/144991902-0bebe13a-d047-45ef-b6ee-cb87c6fa1380.jpg)

**All Levels[]:** Oyundaki tüm levelları tuttuğumuz array.

**Enable Test Level:** Test etmek istediğimiz spesifik bir level var ise true yapıyoruz.

**Test Level:** Test etmek istediğimiz level objesi.

**Level Holder:** Sahneye spawnlanan levelları tutan parent objesi.


### Game Manager

![Screen Shot 2021-12-21 at 17 01 24](https://user-images.githubusercontent.com/50348497/146942400-7d8b5d9a-b6a1-4875-baf6-7888e5353ce8.jpg)


```csharp
public void StartLevel()
{
    EventManager.StartLevel(SaveLoadManager.GetLevel());
    gameState = GameState.Playing;
}

public void NextLevel()
{
    Destroy(LevelManager.Instance.ActiveLevel);
    LevelManager.Instance.ActiveLevel = LevelManager.Instance.CreateLevel();

    gameState = GameState.Idle;
    SetLevelText();
}

/// <summary>
/// Call when level is successfully finished.
/// </summary>
public void WinLevel()
{
    EventManager.SuccessLevel(SaveLoadManager.GetLevel());
    SaveLoadManager.IncreaseLevel();

    winScreen.SetActive(true);
    gameState = GameState.End;
}

/// <summary>
/// Call when level is failed.
/// </summary>
public void LoseLevel()
{
    EventManager.FailLevel(SaveLoadManager.GetLevel());

    loseScreen.SetActive(true);
    gameState = GameState.End;
}
```

StartLevel ve NextLevel fonksiyonlar UI üzerinden oyunu başlatmak ve yeni levela geçmek için kullanılır. 

WinLevel ve LoseLevel fonksiyonları oyunun win ve lose conditionı karar verdiğimiz yerde çağırılır.


### Canvas
![Screen Shot 2021-12-07 at 11 39 17](https://user-images.githubusercontent.com/50348497/144995301-815063f0-7926-42ac-8d24-a621f107cbc7.jpg)

Canvas içerisinde bir hyper casual oyunun başından sonuna kadar gerekli temel öğeler bulunuyor.

**Level Bar:** İçerisinde Text Mesh Pro olan levelların yazıldığı text.

**Tap To Start**: Oyunun başlangıçtaki "Tap To Start" yazısı ve ekrana bastığımızda olan eventi içeren obje.

![Screen Shot 2021-12-07 at 11 57 22](https://user-images.githubusercontent.com/50348497/144997959-d1e83a9d-1e4a-4204-a3f2-32276f101e59.jpg)

Eventin içine gerekli oldukça farklı fonksiyonlar eklenebilir.

**Success Screen:** Level success olduğunda açılan ekran. İçerisinde "Tap To Continue" için gerekli fonksiyonları içeren bir 'Tap' eventi var.

![Screen Shot 2021-12-07 at 12 00 51](https://user-images.githubusercontent.com/50348497/144998523-d0a7720a-080b-4a4b-b0fc-2bfa74da88cc.jpg)

Eventin içine gerekli oldukça farklı fonksiyonlar eklenebilir.

**Fail Screen:** Level fail olduğunda açılan ekran. İçerisinde "Tap To Continue" için gerekli fonksiyonları içeren bir 'Tap' eventi var.

![Screen Shot 2021-12-07 at 12 08 27](https://user-images.githubusercontent.com/50348497/144999736-e07b1559-4d12-4f1b-bb1d-e0a7b6a0042a.jpg)

Eventin içine gerekli oldukça farklı fonksiyonlar eklenebilir.


## Assets

![Screen Shot 2021-12-07 at 12 09 33](https://user-images.githubusercontent.com/50348497/144999927-76051379-02fe-44e8-9fba-e703dd625772.jpg)

**_Main Assets:** Projenin içerisinde kullandığımız ana assetler (modeller, texturelar, scriptler vs.)
![Screen Shot 2021-12-07 at 12 10 57](https://user-images.githubusercontent.com/50348497/145000186-8eb53109-7743-4762-ad86-ee3d2571cfee.jpg)


**GP Hive:** İçerisinde çeşitli scriptlerin olduğu kütüphanemiz. Projelerde kullanılacak core sistemler, 
geliştirmeye yardımcı olacak ve çeşitli sınıfları genişleten extension methodlar (Math, Vector3, Random).
![Screen Shot 2021-12-07 at 12 32 34](https://user-images.githubusercontent.com/50348497/145003676-2290abf0-ec60-4cea-a294-4b1208f3f3f9.jpg)


## Kütüphane

### [Event Manager](https://github.com/G-Play-Studio/Template-Project/blob/master/Assets/GP%20Hive/Core/EventManager.cs)

```csharp
public delegate void OnLevelStart();
public static event OnLevelStart LevelStarted;

public delegate void OnLevelSuccess();
public static event OnLevelSuccess LevelSuccessed;

public delegate void OnLevelFail();
public static event OnLevelFail LevelFailed;

public delegate void OnLevelRestart();
public static event OnLevelRestart LevelRestarted;

public delegate void OnNextLevelCreated();
public static event OnNextLevelCreated NextLevelCreated;
```

Önceden oluşturulmuş sabit eventler var ve bunlar oyunun belirli noktalarında triggerlanıyor. Start, Win, Lose, Restart anlarında çalışmasını istediğimiz fonksiyonları bu eventlere dinletiyoruz.


### [Custom Event Manager](https://github.com/G-Play-Studio/Template-Project/blob/master/Assets/GP%20Hive/Core/CustomEventManager.cs)

Default eventler harici oyun içi farklı olaylar için custom eventler ekleyebilecğeimiz bir sistem.

```csharp
private void OnEnable()
{
    CustomEventManager.Subscribe("TestEvent1", TestMethodOne);
    CustomEventManager.Subscribe("TestEvent1", TestMethodTwo);
    CustomEventManager.Subscribe("TestEvent2", TestMethodThree);
}

private void OnDisable()
{
    CustomEventManager.Unsubscribe("TestEvent1", TestMethodOne);
    CustomEventManager.Unsubscribe("TestEvent1", TestMethodTwo);
    CustomEventManager.Unsubscribe("TestEvent2", TestMethodThree);
}


private void TestMethodOne()
{
    Debug.Log("test 1");
}

private void TestMethodTwo()
{
    Debug.Log("test 2");
}

private void TestMethodThree()
{
    Debug.Log("test 3");
}
```

Eventleri trigger ettirirken.

```csharp
private void Trigger()
{
    CustomEventManager.TriggerEvent("TestEvent1");
    CustomEventManager.TriggerEvent("TestEvent2");
}
```


### [Camera Manager](https://github.com/G-Play-Studio/Template-Project/blob/master/Assets/GP%20Hive/Core/CameraManager.cs)

![Screen Shot 2021-12-21 at 17 20 42](https://user-images.githubusercontent.com/50348497/146945175-95f51be2-a0f4-46b9-af11-74bc872d1a55.jpg)


```csharp
public void SwitchCamera(string _key)
{
    if (!cameraList.ContainsKey(_key))
    {
        Debug.LogError($"Virtual Camera {_key} doesn't exist.", gameObject);
        return;
    }

    foreach (CinemachineVirtualCamera cam in cameraList.Values)
    {
        cam.Priority = 0;
    }

    cameraList[_key].Priority = 1;
}
```

Kamerayı değiştirmek istediğimiz yerde SwictchCamera("Kamera adı"); methodunu çağırıyoruz.

### [Object Pooling](https://github.com/G-Play-Studio/Template-Project/blob/master/Assets/GP%20Hive/Game/ObjectPooling.cs)

![Screen Shot 2022-04-06 at 11 10 08](https://user-images.githubusercontent.com/50348497/161927438-31738054-6d78-4eef-8f8f-0957691fa7ed.jpg)

Dinamik bir şekilde istediğimzi sayıda objeyi pool etmemize yardımcı olan bir sistem.

**Name:** Objeyi çağırırken kullandığımız isim.

**Object To Pool:** Pool edeceğimiz obje.

**Pool Count:** Kaç adet pool etmek istiyoruz.

**Expand Amount:** Pooldaki objelerimiz bittiğinde poola yeni eklenecek obje sayısı.


```csharp
    private void ExamplePoolMethod()
    {
        GameObject _pooledObject = ObjectPooling.GetFromPool("exampleObject");
    }    
```

Pool edilen objeler "Object Pooling" objesinin childi olarak oluşturulur. Eğer objelerin parentını değiştirmişsek objeleri geri vermek için:

```csharp
    private void ExamplePoolMethod()
    {
        GameObject _pooledObject = ObjectPooling.GetFromPool("exampleObject");
        ObjectPooling.Deposit(_pooledObject);
    }    
```

Bütün pool edilmiş objeleri poola geri vermek için:

```csharp
    private void ExamplePoolMethod()
    {
        ObjectPooling.DepositAll();
    }    
```

### [Player Economy](https://github.com/G-Play-Studio/Template-Project/blob/master/Assets/GP%20Hive/Game/PlayerEconomy.cs)

![Screen Shot 2022-04-06 at 11 31 46](https://user-images.githubusercontent.com/50348497/161931769-3ef75a4a-2d3d-4231-9371-046996d94448.jpg)

Eğer oyunda ekonomi varsa kullandığımız sistem. Inspectordeki butonlar ile debug amaçlı para ekleyebiliyoruz.


```csharp
    private void BuySomething(int price)
    {
        if(!PlayerEconomy.Instance.SpendMoney(price)) return;
        
        // Do something related to bought item. 
    }    
```

 ```SpendMoney(int amount)``` methodu eğer paramız yetiyor ise "true" yetmiyor ise "false" döndürüyor. Methodu çağırıp döndürdüğü değere göre satın alma işlemi yapıyopruz.
 
 
 ```csharp
    private void CheckBuyable(int price)
    {
        if(!PlayerEconomy.Instance.CheckEnoughMoney(price)) return;
        
        // Do something related to item. 
    }
```

Satın alınacak ögeyi henüz satın almadan önce paramızın yetip yetmediğini kontrol etmek istiyorsak ```CheckEnoughMoney(int price)``` methodunu kullanıyoruz
 
 ```csharp
    private void GetPrize(int amount)
    {
        PlayerEconomy.Instance.AddMoney(amount);       
    }
```

Para eklemek için kullandığımız method.

## Character Controllers

### Swerve Character Controller

![Screen Shot 2022-12-13 at 14 16 41](https://user-images.githubusercontent.com/50348497/207303783-9d658a3e-08c0-4d1d-b412-6e88fc6d81df.jpg)

![Screen Shot 2022-12-13 at 14 17 23](https://user-images.githubusercontent.com/50348497/207303906-6f8b9b30-b0cf-4f15-be84-fc5aaacb1aae.jpg)

Sahneye sürükleyip swerve controllerı direkt olarak kullanmaya başlayabilirsiniz. "Swerve Controller Config" scriptable objesinden sensitivity, hız vb. ayarları değiştirebilirsiniz.

![Screen Shot 2022-12-13 at 14 18 52](https://user-images.githubusercontent.com/50348497/207304231-ce1149d2-cf15-47e8-a9a5-149741809704.jpg)

Controllera platform üzerinde sınırlar belirlemek için "Swerve Controller" scripti üzerindeki "Use Limit Transform" booleanı true yapmanız gerek.

![Screen Shot 2022-12-13 at 14 21 19](https://user-images.githubusercontent.com/50348497/207304710-cf677316-3fda-4733-8db9-483533825e28.jpg)

Left ve Right Limit objelerini sahnede yerleştirip component üzerindeki alanlara atayabilirsiniz.

<img src="https://user-images.githubusercontent.com/50348497/207305044-beffe047-50c0-489b-b6d7-2ac79f1ed5a3.jpg" width="400" height="400"/>


### IO Character Controller

Prefabi sahneye, joysticki canvasa sürükleyip direkt olarak kullanmaya başlayabilirsiniz.

![Screen Shot 2022-12-13 at 17 55 24](https://user-images.githubusercontent.com/50348497/207366938-ec1bf6e7-4e8a-4aa8-8277-8db571770d8b.jpg)


![Screen Shot 2022-12-13 at 17 56 28](https://user-images.githubusercontent.com/50348497/207367115-d51d657f-d09e-4385-a401-019397340afd.jpg)

**Normalize Joystick Input:** Inputu normalize eder. Karakter hızı joystick oynatma miktarındna bağımsız hep maks seviyede olur.

**Use Animator:** Animasyona sahip karakterler için animatör kullanımını aktif eder.

**Animator Controller:** Kullanılacak controller.

**Avatar:** Karakterin avatarı.

**Use Navmesh For Boundaries:** Boundary belirlemek için navmesh kullanılır. Karakter boundaryler dışına çıkamaz.


Navmesh kullanırken sahneye gameobject ekleyip içine "Navmesh Surface" componentını atamnız gerekiyor.

![Screen Shot 2022-12-13 at 18 00 38](https://user-images.githubusercontent.com/50348497/207368168-66d1044c-ebcf-4aba-945d-9f18b843ed7d.jpg)

Use Geometry seçeneğini collidera çekip sadece zeminlere vereceğiniz layerı bake edecek şekilde filtrelemeniz gerekiyor. En doğru sonuç için collider tabanlı bake edilmeli.

