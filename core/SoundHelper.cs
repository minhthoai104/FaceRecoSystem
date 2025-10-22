using System;
using System.IO;
using WMPLib;

public class SoundHelper
{
    private static WindowsMediaPlayer _player;
    private static string _loadedFile;

    /// <summary>
    /// Nạp trước file âm thanh (chưa phát)
    /// </summary>
    public static void LoadMp3(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"[Âm thanh] Không tìm thấy file: {filePath}");
                return;
            }

            if (_player == null)
                _player = new WindowsMediaPlayer();

            _player.URL = filePath;
            _player.settings.volume = 100;
            _loadedFile = filePath;

            Console.WriteLine($"[Âm thanh] Đã load sẵn file: {Path.GetFileName(filePath)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Âm thanh] Lỗi khi load MP3: {ex.Message}");
        }
    }

    /// <summary>
    /// Phát âm thanh đã load sẵn
    /// </summary>
    public static void PlayLoaded()
    {
        try
        {
            if (_player == null || string.IsNullOrEmpty(_loadedFile))
            {
                Console.WriteLine("[Âm thanh] Chưa load file nào để phát.");
                return;
            }

            _player.controls.currentPosition = 0; // phát lại từ đầu
            _player.controls.play();
            Console.WriteLine($"[Âm thanh] Đang phát: {Path.GetFileName(_loadedFile)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Âm thanh] Lỗi phát MP3: {ex.Message}");
        }
    }
}
