using LibreHardwareMonitor.Hardware;

namespace CustomSystemMonitor
{
    public class Program
    {
        static void Main(string[] args)
        {
            Computer computer = new Computer();
            computer.Open();
            computer.IsCpuEnabled = true;
            computer.IsGpuEnabled = true;

            Console.WriteLine("Monitoring CPU Load. Press Ctrl+C to exit.");

            try
            {
                RunMonitor(computer);

                //TestOutputMethod(computer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
            }
        }

        private static void RunMonitor(Computer computer)
        {
            while (true)
            {
                var outputDict = new Dictionary<string, string>();
                foreach (var hardware in computer.Hardware)
                {
                    hardware.Update();

                    foreach (var sensor in hardware.Sensors)
                    {
                        // Disabled - Not working on laptop for some reason
                        // May be worth testing on desktop
                        //if (sensor.Name.Contains("Core Average") && sensor.SensorType == SensorType.Temperature)
                        //{
                        //    //Console.WriteLine($"CPU Temp: {sensor.Value} C");
                        //    outputDict.TryAdd("CPU Temp", $"{sensor.Value}C");
                        //    continue;
                        //}
                        if (sensor.Name.Contains("CPU Total") && sensor.SensorType == SensorType.Load)
                        {
                            //Console.WriteLine($"CPU Total Load: {sensor.Value}%");
                            outputDict.TryAdd("CPU Total Load", $"{sensor.Value}%");
                            continue;
                        }
                        if (sensor.Name.Contains("GPU Core") && sensor.SensorType == SensorType.Temperature)
                        {
                            //Console.WriteLine($"GPU Core Temp: {sensor.Value} C");
                            outputDict.TryAdd("GPU Core Temp", $"{sensor.Value} C");
                            continue;
                        }
                        if (sensor.Name.Contains("GPU Core") && sensor.SensorType == SensorType.Load)
                        {
                            //Console.WriteLine($"GPU Core Load: {sensor.Value}%");
                            outputDict.TryAdd("GPU Core Load", $"{sensor.Value}%");
                            continue;
                        }
                    }
                }

                foreach (var item in outputDict)
                {
                    Console.WriteLine($"{item.Key}: {item.Value}");
                }

                Thread.Sleep(3000);
            }
        }

        // REMOVE - Just for testing
        private static void TestOutputMethod(Computer computer)
        {
            foreach (var hardware in computer.Hardware)
            {
                hardware.Update();
                foreach (var sensor in hardware.Sensors)
                {
                    if (sensor.SensorType == SensorType.Temperature && sensor.Name == "CPU Package")
                        Console.WriteLine($"{sensor.Name}: {sensor.Value}");
                }
            }
        }
    }
}