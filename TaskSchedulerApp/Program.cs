using TaskSchedulerApp.Commands;
using TaskSchedulerApp.Services;
using TaskSchedulerApp.Utilities;

/// <summary>
/// Programın giriş noktası. Kullanıcı menüsünü ve otomatik backup işlemini yönetir.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        // Konfigürasyonu yükler ve yolları belirler
        var config = new Configuration();
        string activeFolderPath = config.GetPath("ActiveFolder");
        string backupFolderPath = config.GetPath("BackupFolder");
        int backupInterval = config.GetTimer("BackupIntervalInMinutes");

        // Servislerin oluşturulması
        var backupService = new BackupService(activeFolderPath, backupFolderPath);
        var activeFolderService = new ActiveFolderService(activeFolderPath);

        // Observer'ların bağlanması
        var consoleNotifier = new ConsoleNotifier(); // Bildirimler konsola gönderilir
        backupService.AddObserver(consoleNotifier);
        activeFolderService.AddObserver(consoleNotifier);

        // Komutların tanımlanması
        var backupCommand = new BackupCommand(backupService);
        var deleteFilesCommand = new DeleteFilesCommand(activeFolderService);

        // Otomatik backup işlemi için zamanlayıcı
        Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(backupInterval * 60000); // Backup zaman aralığı
                backupCommand.Execute(); // Backup işlemini tetikler
            }
        });

        // Kullanıcı menüsünün yönetimi
        while (true)
        {
            Console.WriteLine("\nMenü:");
            Console.WriteLine("1. Backup işlemini manuel çalıştır.");
            Console.WriteLine("2. Active klasöründeki dosyaları manuel olarak sil.");
            Console.WriteLine("3. Çıkış.");
            Console.Write("Seçiminiz: ");

            string? choice = Console.ReadLine()?.Trim();
            if (choice == "1")
            {
                // Manuel backup işlemini çalıştır
                backupCommand.Execute();
            }
            else if (choice == "2")
            {
                Console.Write("Kaç dakikadan eski dosyalar silinsin? ");
                if (int.TryParse(Console.ReadLine(), out int minutesOld))
                {
                    deleteFilesCommand.Execute(minutesOld);
                }
                else
                {
                    Console.WriteLine("Geçersiz giriş! Lütfen bir sayı girin.");
                }
            }
            else if (choice == "3")
            {
                Console.WriteLine("Program sonlandırılıyor...");
                break;
            }
            else
            {
                Console.WriteLine("Geçersiz seçim! Lütfen tekrar deneyin.");
            }
        }
    }
}
