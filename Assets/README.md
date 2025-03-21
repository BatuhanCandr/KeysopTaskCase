# Unity Game - Karakter Kontrolü ve Özellikler

Bu proje, Unity'de geliştirilmiş bir 2D oyun örneğidir. Aşağıda, oyundaki bazı temel özellikler ve mekanikler açıklanmaktadır.

## Karakter Kontrol Tuşları

- **Hareket**: Karakteri yönlendirmek için **Joystick** veya **Yön tuşları (sağ/sol)** kullanılabilir.
- **Zıplama**: **UI Butonu** tuşu ile zıplama yapılır.
- **Kazma**: Kazma işlemleri, ilgili kazma araçlarını kullanarak **atanan UI Butonları** ile yapılabilir.

## Hareket Mantığı

Karakter, **FixedJoystick** kullanılarak yatay eksende hareket eder. Karakterin sağa veya sola hareket etmesi için joystick yönlendirilir. Bu hareket, `runSpeed` ile hızlandırılır ve joystick durduğunda, karakterin hareketi yavaşlar (deceleration ile). Zıplama ise karakter yerle temas ettiğinde (grounded olduğunda) aktif hale gelir. Eğer karakter havada ise, ikinci bir zıplama (double jump) yapılabilir.


# DigController Script (Kazma Kontrolü)

`DigController` scripti, oyunda oyuncunun kazma işlemlerini yönetir. Oyuncunun farklı kazma aletlerini (kürek ve pickaxe) etkinleştirmesini ve oyuncunun aksiyonlarına göre parçacık yayılmasını kontrol eder. Ayrıca, demo modu ve normal moddaki farklı davranışları yönetir.

## Değişkenler

### Silahlar:
- **sideScythe** (GameObject): Oyuncunun kazma araçlarından biri olan yan çengelli.
- **downScythe** (GameObject): Oyuncunun kazma araçlarından diğeri olan aşağı çengelli.

### Kazıma Ayarları:
- **m_Shovel** (Shovel): Kazıma için kullanılan kürek aracı.
- **m_ShovelParticleSystem** (ParticleSystem): Kürekle kazıma yaparken kullanılan parçacık sistemi.
- **m_Pickaxe** (Shovel): Kazıma için kullanılan pickaxe (veya çekiç) aracı (Shovel sınıfıyla aynı).
- **m_PickaxeParticleSystem** (ParticleSystem): Pickaxe ile kazıma yaparken kullanılan parçacık sistemi.

### Durum:
- **isDemo** (bool): Oyunun demo modunda mı yoksa normal modda mı olduğunu belirten bir bayrak.

## Yöntemler

### `DigByShovel()`
Kürekle kazıma işlemini başlatır ve ilgili parçacık efektini tetikler.

### `DigByPickaxe()`
Pickaxe ile kazıma işlemini başlatır ve ilgili parçacık efektini tetikler.

### `DigAndEmitParticles(Shovel shovel, ParticleSystem particleSystem)`
Kürek veya pickaxe ile kazma işleminin temel mantığını ve parçacık yayılmasını yönetir. Demo modunda ise `EmitParticlesInDemo()` kullanılır, normal modda ise `EmitParticlesInNormalMode()` kullanılır.

### `EmitParticlesInDemo(Shovel shovel, ParticleSystem particleSystem)`
Demo modunda parçacık yayılmasını yönetir. Küreğin kazma alanına bağlı olarak parçacıklar üretir ve bunları parçacık sistemi ile yayar.

### `EmitParticlesInNormalMode(Shovel shovel, ParticleSystem particleSystem)`
Normal modda parçacık yayılmasını yönetir. Küreğin kazma alanına bağlı olarak yayılacak parçacık sayısını hesaplar ve bunları parçacık sistemi ile yayar.

### `SetFalseScythe()`
Yan ve aşağı çengelli kazmaları devre dışı bırakır ve oyuncunun kazma durumunu false olarak ayarlar.

### `SetTrueScythe(int side)`
`side` parametresine (0 yan çengelli için, 1 aşağı çengelli için) bağlı olarak uygun kazmayı etkinleştirir. Ayrıca oyuncunun kazma durumunu true olarak ayarlar.

## Kullanım

- Oyuncu kürek veya pickaxe kullanarak kazma yapabilir ve kazma alanına bağlı olarak parçacıklar yayılır.
- Demo modunda, belirli bir parçacık yayılma mantığı kullanılır.
- `sideScythe` ve `downScythe` farklı kazma yönleri için kullanılır ve oyuncunun aksiyonlarına göre etkinleştirilip devre dışı bırakılabilir.


## Rigidbody ve CharacterController Farkları

### Rigidbody
- **Rigidbody**, fiziksel simülasyon için kullanılan bir bileşendir. Bu, hareket, zıplama, ve çarpışma gibi fiziksel etkileşimleri simüle eder. Unity'deki `Rigidbody2D` bileşeni, 2D oyunlarda objelerin hareketini kontrol eder. Bu bileşeni kullandığınızda, objenin fiziği tamamen Unity'nin fizik motoruna bırakılır.
- **Özellikleri**:
    - Fiziksel etkileşimler ve çarpışmalar simüle edilir.
    - Hareket, hız ve kuvvetler aracılığıyla yapılır.
    - `Rigidbody` üzerinde uygulanan kuvvetler (örneğin zıplama gücü) karakterin hızını ve hareket yönünü belirler.
    - **Çarpışmalar** fizik motoru tarafından yönetilir ve objelerin birbirine çarpma etkisi (drag, gravity) doğru şekilde hesaplanır.

### CharacterController
- **CharacterController**, bir karakterin hareketini basit ve doğrudan kontrol etmek için kullanılan bir bileşendir. Rigidbody'den farklı olarak, fiziksel simülasyonun çoğunu yönetmez; bunun yerine daha doğrudan bir hareket kontrolü sağlar.
- **Özellikleri**:
    - Fiziksel etkileşimler sınırlıdır (örneğin, sürüklenme, zıplama gibi olaylar).
    - `Move()` fonksiyonu ile karakterin hareketi manuel olarak yönetilir.
    - Çarpışmalar, karakterin yerle teması gibi etkileşimler `CharacterController` tarafından yönetilir.
    - Fiziksel motor yerine, kontrol edilen objenin hareketini ve çarpışmalarını daha düzgün ve hassas şekilde yönetmek için kullanılır.

Özetle, **Rigidbody** daha fiziksel bir yaklaşım sağlarken, **CharacterController** daha doğrudan ve kontrollü bir yaklaşım sağlar. `Rigidbody` ile oyun objesinin doğal fiziksel davranışlarını yönetirken, `CharacterController` ile oyun karakterinin daha düzgün ve kontrollü hareket etmesini sağlarsınız.

# Bonus Özellikler
- **Dinamik Kameralar**: Kamera, karakterin etrafında döner ve karakterin bulunduğu zemin ile doğru bir şekilde hizalanır. Bu özellik sayesinde, karakter her yerden hareket edebilmekte ve kamera her zaman karakteri izler.
- **Bonus Ses Efektleri**: Kazma işlemleri ve yürüme sırasında farklı ses efektleri çalınır ve ortamın atmosferini güçlendirir.
- **Deathzone:** Karakter belirlenen ortam dışına çıktığında aşağıya düştüğünde oyun yeniden başlar.
- **Acceleration/Deceleration:** Karakter yürürken daha yumuşak bir etki alınabilmesi için hızlanma ve yavaşlama kontrolü.


## Bonus Grappling Hook Açıklaması
Bu script, Unity oyununda bir grappling hook (tutunma kancası) mekanizmasını yönetir. Oyuncu, joystick ile ok yönünü kontrol eder ve ok, joystick'e göre yönlendirilir. Bu script, oyuncunun kancayı atabilmesi ve hedefe doğru hareket etmesini sağlar.

**Başlıca Özellikler:**

- **Joystick Kontrolü:** Kullanıcı, joystick hareketiyle ok yönünü ve kancanın hedef noktasını kontrol edebilir.

- **Ok Hareketi ve Dönme:** Joystick hareketine göre ok, düzgün bir şekilde hedef pozisyona hareket eder ve doğru yönde döner.

- **Grappling Mekanizması:** Kullanıcı, GetGrapplingHook() fonksiyonunu çağırarak kancayı ateşler. Kanca, belirli bir mesafeye kadar ilerler ve bir yüzeye çarptığında oyuncuyu hedefe doğru çeker.

- **Çizgi Çekme:** Kanca atıldığında, bir LineRenderer kullanılarak kancanın hareketi görsel olarak izlenebilir.

- **Oyuncu Hareketi:** Kanca hedefine ulaştığında, oyuncu kancanın bağlı olduğu noktaya doğru hareket eder.

- **Çalışma Adımları:**

- **Joystick Yönü:** Kullanıcı joystick'i hareket ettirirse ok görünür ve ok, joystick yönünde hareket eder ve döner.

- **Kanca Fırlatma:** GetGrapplingHook() metodu, kancanın atılmasını tetikler. Kanca bir yüzey ile çarptığında, hedef pozisyon belirlenir.

- **Çizgi Çekme:** LineRenderer kullanılarak kancanın hareketi görsel olarak uzatılır.

- **Oyuncu Hareketi:** Kanca hedefine ulaştığında, oyuncu hedefe doğru çekilir ve belirli bir mesafeye geldiğinde işlem sonlanır.

**Kullanılan Bileşenler:**
- **Joystick:** Kanca yönünü kontrol etmek için.
- **LineRenderer:** Kancanın hedefe doğru hareket ettiğini göstermek için.
- **SpriteRenderer:** Okun görselini kontrol eder.
