using Core.Entities.Concrete;
using System.Runtime.Serialization;

namespace Business.Constants
{
    //sbit ve newlenmeyecek ise static yazılır yani uygulama boyunca 1 tane instancei olur.
    public static class Messages
    {
        public static string ProductAdded = "Ürün eklendi";
        public static string ProductNameInvalid = "Ürün ismi geçersiz.";
        public static string MaintenanceTime = "Sistem bakımda";
        public static string ProductsListed = "Ürünler Listelendi.";
        public static string ProductListedByCategory = "Kategoriye göre Ürünler getirildi";
        public static string GetProductById = "Idyi ürüne göre al";
        public static string MinMaxProductList = "Minimum ve maximum arası ürün listesi getirildi.";
        public static string GetProductDetail = "Ürün detayları getirildi";
        public static string ProductCountOfCategoryError = "Category için verilebilecek maksimum ürün sayısına ulaşılmıştır.";
        public static string ProductNameAlreadyExists = "Böyle bir ürün zaten sistemde kayıtlıdır.";
        public static string CheckCategoryCount = "Kategory sayısı 15'i geçtiği için ürün eklemesi yapılamaz";
        public static string AuthorizationDenied = "Yetkiniz yok.";
        public static string UserRegistered = "User Kayıt edildi.";
        public static string UserNotFound = "User Bulunamadı";
        public static string PasswordError = "Password hatalı";
        public static string SuccessfulLogin = "Başarı ile giriş yapıldı";
        public static string UserAlreadyExists = "User zaten vardır.";
        public static string AccessTokenCreated = "Token oluşturuldu";
    }
}
