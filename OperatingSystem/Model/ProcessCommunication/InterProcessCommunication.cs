using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.ProcessCommunication
{
    internal class InterProcessCommunication
    {
        private readonly string memoryMappedFileName = "MyMemoryMappedFile";
        private readonly string mutexName = "MyMutex";
        private bool shouldStop = false;
        private Thread thread;
        private Mutex mutex;
        private MemoryMappedFile memoryMappedFile;

        public void StartListening()
        {
            mutex = new Mutex(true, mutexName);
            memoryMappedFile = MemoryMappedFile.CreateOrOpen(memoryMappedFileName, 4096);

            shouldStop = false;
            thread = new Thread(() =>
            {
                while (true)
                {
                    mutex.WaitOne();
                    try
                    {
                        using (MemoryMappedViewStream stream = memoryMappedFile.CreateViewStream())
                        {
                            if (stream.Length > 0)
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    string messageJson = reader.ReadToEnd();
                                    if (!string.IsNullOrEmpty(messageJson))
                                    {
                                        Message message = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(messageJson);
                                        ProcessReceivedMessage(message);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Обработка исключения чтения сообщения
                        Console.WriteLine($"Error reading message: {ex.Message}");
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }

                    if (shouldStop)
                    {
                        break;
                    }

                    Thread.Sleep(1000); // Пауза между проверками новых сообщений
                }
            });
            thread.Start();
        }

        public void StopListening()
        {
            shouldStop = true;
            if (thread != null && thread.IsAlive)
            {
                thread.Join();
            }
            memoryMappedFile.Dispose();
            mutex.Dispose();
        }

        public void SendMessage(string receiver, string content)
        {
            Message message = new Message
            {
                Sender = Environment.MachineName, // Имя текущей машины
                Content = content
            };

            mutex.WaitOne();
            try
            {
                using (MemoryMappedViewStream stream = memoryMappedFile.CreateViewStream())
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        string messageJson = Newtonsoft.Json.JsonConvert.SerializeObject(message);
                        writer.Write(messageJson);
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка исключения отправки сообщения
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        private void ProcessReceivedMessage(Message message)
        {
            // Обработка полученного сообщения
            Console.WriteLine($"Received message from {message.Sender}: {message.Content}");
        }
    }
}
