using Newtonsoft.Json;
using System;
using System.IO;
using Unity;

namespace VerySpecificCredentialManager.Services
{
    public class StorageService
    {
        [Dependency]
        public EncryptionService EncryptionService { get; set; }

        public void Write<T>(string key, T value)
        {
            File.WriteAllBytes(GetKeyPath(key), EncryptionService.Encrypt(JsonConvert.SerializeObject(value), "super secure password lol"));
        }

        public T Read<T>(string key)
        {
            if (!File.Exists(GetKeyPath(key))) { return default; }

            return JsonConvert.DeserializeObject<T>(EncryptionService.DecrypToString(File.ReadAllBytes(GetKeyPath(key)), "super secure password lol"));
        }

        private string GetKeyPath(string key) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{key}.db");

    }
}
