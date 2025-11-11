/* ======================================================
 SQL KURULUM BETÝÐÝ (GÜVENLÝ ve DÜZELTÝLMÝÞ)
 Bu script, nesnelerin var olup olmadýðýný kontrol eder.
 Var olanlarý önce siler, sonra yeniden oluþturur.
 Bu sayede birden çok kez çalýþtýrýlabilir.
======================================================
*/

-- Foreign Key'leri (iliþkileri) kýrmamak için
-- SP'leri ve Tablolarý ters sýrada siliyoruz.

/* ======================================================
 ADIM 1: MEVCUT STORED PROCEDURE'LERÝ SÝLME
======================================================
*/

IF OBJECT_ID('sp_Kitap_Yonet', 'P') IS NOT NULL DROP PROCEDURE sp_Kitap_Yonet;
IF OBJECT_ID('sp_Uye_Yonet', 'P') IS NOT NULL DROP PROCEDURE sp_Uye_Yonet;
IF OBJECT_ID('sp_Odunc_Ver', 'P') IS NOT NULL DROP PROCEDURE sp_Odunc_Ver;
IF OBJECT_ID('sp_Iade_Al', 'P') IS NOT NULL DROP PROCEDURE sp_Iade_Al;
IF OBJECT_ID('sp_Rezervasyon_Yap', 'P') IS NOT NULL DROP PROCEDURE sp_Rezervasyon_Yap;
IF OBJECT_ID('sp_Ceza_Ode', 'P') IS NOT NULL DROP PROCEDURE sp_Ceza_Ode;
IF OBJECT_ID('sp_Rezervasyon_Iptal', 'P') IS NOT NULL DROP PROCEDURE sp_Rezervasyon_Iptal;
IF OBJECT_ID('sp_Kategori_Listele', 'P') IS NOT NULL DROP PROCEDURE sp_Kategori_Listele;
IF OBJECT_ID('sp_Yazar_Listele', 'P') IS NOT NULL DROP PROCEDURE sp_Yazar_Listele;
IF OBJECT_ID('sp_Kitap_Listele_Musait', 'P') IS NOT NULL DROP PROCEDURE sp_Kitap_Listele_Musait;
IF OBJECT_ID('sp_Odunc_Listele_Aktif', 'P') IS NOT NULL DROP PROCEDURE sp_Odunc_Listele_Aktif;
IF OBJECT_ID('sp_Ceza_Listele_Odenmemis', 'P') IS NOT NULL DROP PROCEDURE sp_Ceza_Listele_Odenmemis;
IF OBJECT_ID('sp_Rezervasyon_Listele_Aktif', 'P') IS NOT NULL DROP PROCEDURE sp_Rezervasyon_Listele_Aktif;
GO

/* ======================================================
 ADIM 2: MEVCUT TABLOLARI SÝLME (Ýliþki sýrasýna göre)
======================================================
*/

IF OBJECT_ID('Rezervasyonlar', 'U') IS NOT NULL DROP TABLE Rezervasyonlar;
IF OBJECT_ID('Cezalar', 'U') IS NOT NULL DROP TABLE Cezalar;
IF OBJECT_ID('OduncIslemleri', 'U') IS NOT NULL DROP TABLE OduncIslemleri;
IF OBJECT_ID('Kitaplar', 'U') IS NOT NULL DROP TABLE Kitaplar;
IF OBJECT_ID('Kategoriler', 'U') IS NOT NULL DROP TABLE Kategoriler;
IF OBJECT_ID('Yazarlar', 'U') IS NOT NULL DROP TABLE Yazarlar;
IF OBJECT_ID('Uyeler', 'U') IS NOT NULL DROP TABLE Uyeler;
GO

/* ======================================================
 ADIM 3: TABLOLARI YENÝDEN OLUÞTURMA
======================================================
*/

-- Önce baðýmsýz tablolar
CREATE TABLE Kategoriler (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Ad VARCHAR(100) NOT NULL,
    Aciklama VARCHAR(500)
);

CREATE TABLE Yazarlar (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Ad VARCHAR(100) NOT NULL,
    Soyad VARCHAR(100) NOT NULL,
    Ulke VARCHAR(50)
);

CREATE TABLE Uyeler (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Ad VARCHAR(100) NOT NULL,
    Soyad VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Telefon VARCHAR(20),
    TCKimlik VARCHAR(11) UNIQUE,
    Adres VARCHAR(500),
    UyelikTarihi DATETIME NOT NULL DEFAULT GETDATE(),
    Aktif BIT NOT NULL DEFAULT 1
);
GO

-- Þimdi baðýmlý tablolar
CREATE TABLE Kitaplar (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ISBN VARCHAR(20) NOT NULL,
    Ad VARCHAR(200) NOT NULL,
    YayinEvi VARCHAR(100),
    YayinYili INT,
    Durum VARCHAR(20) NOT NULL DEFAULT 'Mevcut',
    EklemeTarihi DATETIME NOT NULL DEFAULT GETDATE(),
    
    YazarId INT FOREIGN KEY REFERENCES Yazarlar(Id),
    KategoriId INT FOREIGN KEY REFERENCES Kategoriler(Id)
);

CREATE TABLE OduncIslemleri (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UyeId INT FOREIGN KEY REFERENCES Uyeler(Id) NOT NULL,
    KitapId INT FOREIGN KEY REFERENCES Kitaplar(Id) NOT NULL,
    
    OduncAlmaTarihi DATETIME NOT NULL DEFAULT GETDATE(),
    PlanlananIadeTarihi DATETIME NOT NULL,
    GeriGetirmeTarihi DATETIME NULL,
    Durum VARCHAR(50) NOT NULL DEFAULT 'Ödünç Verildi' 
);

CREATE TABLE Cezalar (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UyeId INT FOREIGN KEY REFERENCES Uyeler(Id) NOT NULL,
    OduncIslemiId INT FOREIGN KEY REFERENCES OduncIslemleri(Id) NOT NULL,
    
    CezaBaslangicTarihi DATE NOT NULL,
    CezaBitisTarihi DATE NOT NULL,
    GecikenGunSayisi INT NOT NULL,
    CezaTutari DECIMAL(10, 2) NOT NULL,
    
    Aciklama VARCHAR(500),
    Odendi BIT NOT NULL DEFAULT 0
);

CREATE TABLE Rezervasyonlar (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UyeId INT FOREIGN KEY REFERENCES Uyeler(Id) NOT NULL,
    KitapId INT FOREIGN KEY REFERENCES Kitaplar(Id) NOT NULL,
    RezervasyonTarihi DATETIME NOT NULL DEFAULT GETDATE(),
    Durum VARCHAR(50) NOT NULL DEFAULT 'Beklemede' 
);
GO

/* ======================================================
 ADIM 4: STORED PROCEDURE'LERÝ YENÝDEN OLUÞTURMA
======================================================
*/

-- 1. Kitap Yönetimi
CREATE PROCEDURE sp_Kitap_Yonet
    @Operation      VARCHAR(10),
    @Id             INT = NULL, @ISBN           VARCHAR(20) = NULL,
    @Ad             VARCHAR(200) = NULL, @YayinEvi       VARCHAR(100) = NULL,
    @YayinYili      INT = NULL, @Durum          VARCHAR(20) = NULL,
    @YazarId        INT = NULL, @KategoriId     INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @Operation = 'add'
        INSERT INTO Kitaplar (ISBN, Ad, YayinEvi, YayinYili, Durum, YazarId, KategoriId)
        VALUES (@ISBN, @Ad, @YayinEvi, @YayinYili, 'Mevcut', @YazarId, @KategoriId);
    IF @Operation = 'update'
        UPDATE Kitaplar SET ISBN = @ISBN, Ad = @Ad, YayinEvi = @YayinEvi, YayinYili = @YayinYili,
        Durum = @Durum, YazarId = @YazarId, KategoriId = @KategoriId
        WHERE Id = @Id;
    IF @Operation = 'delete'
        UPDATE Kitaplar SET Durum = 'Silindi' WHERE Id = @Id;
    IF @Operation = 'list'
        SELECT K.*, Y.Ad AS YazarAdi, Y.Soyad AS YazarSoyadi, C.Ad AS KategoriAdi
        FROM Kitaplar AS K
        LEFT JOIN Yazarlar AS Y ON K.YazarId = Y.Id
        LEFT JOIN Kategoriler AS C ON K.KategoriId = C.Id
        WHERE K.Durum != 'Silindi' ORDER BY K.Ad;
END
GO

-- 2. Üye Yönetimi
CREATE PROCEDURE sp_Uye_Yonet
    @Operation      VARCHAR(10),
    @Id             INT = NULL, @Ad             VARCHAR(100) = NULL,
    @Soyad          VARCHAR(100) = NULL, @Email          VARCHAR(100) = NULL,
    @Telefon        VARCHAR(20) = NULL, @TCKimlik       VARCHAR(11) = NULL,
    @Adres          VARCHAR(500) = NULL, @Aktif          BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @Operation = 'add'
        INSERT INTO Uyeler (Ad, Soyad, Email, Telefon, TCKimlik, Adres)
        VALUES (@Ad, @Soyad, @Email, @Telefon, @TCKimlik, @Adres);
    IF @Operation = 'update'
        UPDATE Uyeler SET Ad = @Ad, Soyad = @Soyad, Email = @Email, Telefon = @Telefon,
        TCKimlik = @TCKimlik, Adres = @Adres, Aktif = @Aktif
        WHERE Id = @Id;
    IF @Operation = 'delete'
        UPDATE Uyeler SET Aktif = 0 WHERE Id = @Id;
    IF @Operation = 'list'
        SELECT * FROM Uyeler WHERE Aktif = 1 ORDER BY Ad;
END
GO

-- 3. Ödünç Verme
CREATE PROCEDURE sp_Odunc_Ver
    @UyeId INT, @KitapId INT, @IadeTarihi DATETIME
AS
BEGIN
    SET NOCOUNT ON; BEGIN TRANSACTION;
    BEGIN TRY
        UPDATE Kitaplar SET Durum = 'Ödünç Verildi' 
        WHERE Id = @KitapId AND Durum = 'Mevcut';
        IF @@ROWCOUNT = 0
            THROW 50001, 'Bu kitap þu anda ödünç alýnamaz (durumu Mevcut deðil).', 1;
        INSERT INTO OduncIslemleri (UyeId, KitapId, OduncAlmaTarihi, PlanlananIadeTarihi, Durum)
        VALUES (@UyeId, @KitapId, GETDATE(), @IadeTarihi, 'Ödünç Verildi');
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION; ;THROW;
    END CATCH
END
GO

-- 4. Ýade Alma (Ceza ve Rezervasyon Kontrollü - SON HALÝ)
CREATE PROCEDURE sp_Iade_Al
    @OduncId INT
AS
BEGIN
    SET NOCOUNT ON; BEGIN TRANSACTION;
    DECLARE @GunlukCezaTutari DECIMAL(10, 2) = 1.00; 
    DECLARE @KitapId INT, @UyeId INT, @PlanlananIadeTarihi DATE;
    DECLARE @BugunTarihi DATE = GETDATE();
    DECLARE @GecikenGunSayisi INT = 0;
    BEGIN TRY
        SELECT @KitapId = KitapId, @UyeId = UyeId, @PlanlananIadeTarihi = PlanlananIadeTarihi
        FROM OduncIslemleri WHERE Id = @OduncId AND Durum = 'Ödünç Verildi';
        IF @KitapId IS NULL
            THROW 50002, 'Bu iþlem zaten iade edilmiþ veya bulunamadý.', 1;
        
        SET @GecikenGunSayisi = DATEDIFF(DAY, @PlanlananIadeTarihi, @BugunTarihi);
        IF @GecikenGunSayisi > 0
        BEGIN
            DECLARE @HesaplananCeza DECIMAL(10, 2) = @GecikenGunSayisi * @GunlukCezaTutari;
            INSERT INTO Cezalar (UyeId, OduncIslemiId, CezaBaslangicTarihi, CezaBitisTarihi, GecikenGunSayisi, CezaTutari, Aciklama, Odendi)
            VALUES (@UyeId, @OduncId, @PlanlananIadeTarihi, @BugunTarihi, @GecikenGunSayisi, @HesaplananCeza, 
                    CAST(@GecikenGunSayisi AS VARCHAR) + ' gün gecikme bedeli.', 0);
        END

        DECLARE @RezervasyonId INT = NULL, @YeniKitapDurumu VARCHAR(20) = 'Mevcut';
        SELECT TOP 1 @RezervasyonId = Id FROM Rezervasyonlar
        WHERE KitapId = @KitapId AND Durum = 'Beklemede' ORDER BY RezervasyonTarihi ASC;
        IF @RezervasyonId IS NOT NULL
        BEGIN
            SET @YeniKitapDurumu = 'Rezerve Edildi'; 
            UPDATE Rezervasyonlar SET Durum = 'Hazýr' WHERE Id = @RezervasyonId;
        END

        UPDATE Kitaplar SET Durum = @YeniKitapDurumu WHERE Id = @KitapId;
        UPDATE OduncIslemleri SET Durum = 'Ýade Edildi', GeriGetirmeTarihi = @BugunTarihi
        WHERE Id = @OduncId;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION; ;THROW;
    END CATCH
END
GO

-- 5. Rezervasyon Yapma
CREATE PROCEDURE sp_Rezervasyon_Yap
    @UyeId INT, @KitapId INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Kitaplar WHERE Id = @KitapId AND Durum = 'Ödünç Verildi')
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM Rezervasyonlar WHERE UyeId = @UyeId AND KitapId = @KitapId AND Durum = 'Beklemede')
            INSERT INTO Rezervasyonlar (UyeId, KitapId, Durum) VALUES (@UyeId, @KitapId, 'Beklemede');
        ELSE
            THROW 50004, 'Bu kitaba zaten aktif bir rezervasyonunuz var.', 1;
    END
    ELSE
        -- *** DÜZELTÝLMÝÞ SATIR ***
        THROW 50003, 'Bu kitap ''Mevcut'' veya ''Rezerve Edildi'' durumunda olduðu için rezervasyon yapýlamaz.', 1;
END
GO

-- 6. Ceza Ödeme
CREATE PROCEDURE sp_Ceza_Ode
    @CezaId INT
AS BEGIN SET NOCOUNT ON; UPDATE Cezalar SET Odendi = 1 WHERE Id = @CezaId AND Odendi = 0; END
GO

-- 7. Rezervasyon Ýptal
CREATE PROCEDURE sp_Rezervasyon_Iptal
    @RezervasyonId INT
AS BEGIN SET NOCOUNT ON; UPDATE Rezervasyonlar SET Durum = 'Ýptal' WHERE Id = @RezervasyonId; END
GO

-- 8. Kategori Listele
CREATE PROCEDURE sp_Kategori_Listele
AS BEGIN SET NOCOUNT ON; SELECT * FROM Kategoriler ORDER BY Ad; END
GO

-- 9. Yazar Listele
CREATE PROCEDURE sp_Yazar_Listele
AS BEGIN SET NOCOUNT ON; SELECT * FROM Yazarlar ORDER BY Ad; END
GO

-- 10. Müsait Kitap Listele
CREATE PROCEDURE sp_Kitap_Listele_Musait
AS BEGIN SET NOCOUNT ON; SELECT * FROM Kitaplar WHERE Durum = 'Mevcut' ORDER BY Ad; END
GO

-- 11. Aktif Ödünç Listesi
CREATE PROCEDURE sp_Odunc_Listele_Aktif
AS BEGIN
    SET NOCOUNT ON;
    SELECT O.Id, O.KitapId, O.UyeId, O.OduncAlmaTarihi, O.PlanlananIadeTarihi,
        K.Ad AS KitapAdi, U.Ad AS UyeAdi, U.Soyad AS UyeSoyadi
    FROM OduncIslemleri AS O
    INNER JOIN Kitaplar AS K ON O.KitapId = K.Id
    INNER JOIN Uyeler AS U ON O.UyeId = U.Id
    WHERE O.Durum = 'Ödünç Verildi'
    ORDER BY O.PlanlananIadeTarihi ASC;
END
GO

-- 12. Ödenmemiþ Ceza Listesi
CREATE PROCEDURE sp_Ceza_Listele_Odenmemis
AS BEGIN
    SET NOCOUNT ON;
    SELECT C.*, U.Ad AS UyeAdi, U.Soyad AS UyeSoyadi
    FROM Cezalar AS C
    INNER JOIN Uyeler AS U ON C.UyeId = U.Id
    WHERE C.Odendi = 0
    ORDER BY C.CezaBaslangicTarihi DESC;
END
GO

-- 13. Aktif Rezervasyon Listesi
CREATE PROCEDURE sp_Rezervasyon_Listele_Aktif
AS BEGIN
    SET NOCOUNT ON;
    SELECT R.*, K.Ad AS KitapAdi, U.Ad AS UyeAdi, U.Soyad AS UyeSoyadi
    FROM Rezervasyonlar AS R
    INNER JOIN Kitaplar AS K ON R.KitapId = K.Id
    INNER JOIN Uyeler AS U ON R.UyeId = U.Id
    WHERE R.Durum = 'Beklemede' OR R.Durum = 'Hazýr'
    ORDER BY R.RezervasyonTarihi ASC;
END
GO

PRINT 'Veritabaný kurulumu baþarýyla tamamlandý (Hata düzeltildi).';