# Proje Repository

Bu repository, iki ayrı ASP.NET MVC uygulamasını tek bir yapı altında barındırmaktadır. Projeler, modern yazılım geliştirme prensiplerine uygun olarak geliştirilmiştir.

## Projeler Hakkında Genel Bilgi

### Etkinlik Yönetimi Uygulaması

Bu uygulama, etkinliklerin ve katılımcıların veritabanında yönetilmesini amaçlamaktadır. Uygulamada;

 - **Veritabanı Modeli:**  
    - **Etkinlikler Tablosu:** Başlık, tarih, yer, ayrıntılar gibi alanları içerir.  
    - **Katılımcılar Tablosu:** Ad, soyad, e-posta ve etkinlik katılım durumu bilgilerini barındırır.  
    - **İlişki:** Bir katılımcı birden fazla etkinliğe kayıt olabilir (çoktan çoka ilişki).

- **Ana Sayfa:**  
  - Planlanmış etkinliklerin listelemesi (başlık, tarih, yer).  
  - Etkinlik detay sayfasına yönlendirme linkleri.  
  - Katılımcı ve etkinlik ekleme formlarına erişim.

- **Etkinlik Detay Sayfası:**  
  - Etkinlik bilgileri ve katılımcı listesinin AJAX ile dinamik güncellenmesi.

- **Etkinlik/Katılımcı Ekleme Sayfaları:**  
  - Hem client-side hem de server-side validasyon ile form işlemleri gerçekleştirilmektedir.

- **Kullanılan Teknolojiler:**  
  - .NET 9.0 MVC, Entity Framework (veya benzeri ORM)  
  - Katmanlı Mimari, Dependency Injection, Repository Design Pattern  
  - SOLID prensipleri, JavaScript & AJAX

---

### GoRest Kullanıcı Yönetim Paneli

Bu uygulama, [GoRest API](https://gorest.co.in/public/v2/users) üzerinden kullanıcı verilerini çekerek Razor (CSHTML) sayfaları ve JavaScript yardımıyla görüntülemekte ve yönetim paneli üzerinden CRUD işlemleri (ekleme, düzenleme, silme) sağlamaktadır.

- **Özellikler:**  
  - Kullanıcı verilerinin dinamik gösterimi ve yönetimi  
  - AJAX ile sayfa yenilemesi olmaksızın işlemlerin gerçekleştirilmesi  
  - URL navigasyonu ve tarayıcı geçmişinin yönetimi

- **Kullanılan Teknolojiler:**  
  - ASP.NET Core MVC, Razor, JavaScript & AJAX



