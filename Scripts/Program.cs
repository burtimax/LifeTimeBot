using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string apiUrl = "https://api-inference.huggingface.co/models/openai/whisper-large-v3-turbo";
        string apiToken = "TOKEN"; // Замените на ваш токен Hugging Face
        string audioFilePath = "C:\\Users\\timof\\Desktop\\audio_2024-12-13_01-35-03.ogg"; // Укажите путь к вашему аудиофайлу

        try
        {
            byte[] audioBytes = File.ReadAllBytes(audioFilePath);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");

                using (var content = new ByteArrayContent(audioBytes))
                {
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/ogg");

                    Console.WriteLine("Отправка аудио на обработку...");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Распознанный текст:");
                        Console.WriteLine(result);
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка: {response.StatusCode}");
                        string errorDetails = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Детали ошибки: {errorDetails}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }
}