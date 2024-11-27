namespace TaskSchedulerApp.Utilities
{
    /// <summary>
    /// Uygulamanın loglama işlemlerini gerçekleştiren merkezi sınıf.
    /// Tüm hata mesajları ve önemli olaylar burada konsola yazdırılır.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Loglama işlemlerini gerçekleştiren metod.
        /// Mesajları konsola belirli bir formatla yazdırır.
        /// </summary>
        /// <param name="message">Loglanacak olan mesaj.</param>
        public static void Log(string message)
        {
            // Log mesajını belirli bir formatta konsola yazdırır
            Console.WriteLine($"[LOG] {DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}");
        }
    }
}