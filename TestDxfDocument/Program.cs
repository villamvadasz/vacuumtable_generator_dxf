using myPoint;
using System;
using System.IO;
using System.Text.Json;
using myGenerator;

namespace TestDxfDocument
{
    /// <summary>
    /// This is just a simple test of work in progress for the netDxf library.
    /// </summary>
    public class Program
    {
        static int demoMode = 0;
        public static int Main(string[] args)
        {
            int result = 0;
            string fileName_Load = "BoxParameter.json";
            string fileName_Save = "generated.dxf";
            if (demoMode != 0)
            {
                System.Console.Out.WriteLine("!!!DEMO!!!");
            }

            if (args.Length == 1)
            {
                fileName_Load = args[0];
                DoMainWorkOfThisThing_vacuumTable(fileName_Load, fileName_Save);
                WriteOutDefaultParameters();
            }
            else if (args.Length == 2)
            {
                fileName_Load = args[0];
                fileName_Save = args[1];
                DoMainWorkOfThisThing_vacuumTable(fileName_Load, fileName_Save);
                WriteOutDefaultParameters();
            }
            else if (args.Length == 3)
            {
                string parameter = args[0];
                fileName_Load = args[1];
                fileName_Save = args[2];
                if (String.Compare(parameter, "-vacuumTable", comparisonType: StringComparison.OrdinalIgnoreCase) == 0)
                {
                    DoMainWorkOfThisThing_vacuumTable(fileName_Load, fileName_Save);
                    WriteOutDefaultParameters();
                }
                else
                {

                }
            }
            else
            {
                System.Console.Out.WriteLine("Usage:");
                System.Console.Out.WriteLine("TestDxfDocument.exe InputFile.json OutputFile.dxf");
                System.Console.Out.WriteLine("TestDxfDocument.exe -vacuumTable InputFile.json OutputFile.dxf");
                WriteOutDefaultParameters();
            }
            return result;
        }

        private static void DoMainWorkOfThisThing_vacuumTable(string fileName_Load, string fileName_Save)
        {
            VacuumTable_Parameters parameters_actual;
            {
                try
                {
                    if (demoMode != 0)
                    {
                        parameters_actual = new VacuumTable_Parameters();
                    }
                    else
                    {
                        string fileName = fileName_Load;
                        string jsonString = File.ReadAllText(fileName);
                        VacuumTable_Parameters parameters_load = JsonSerializer.Deserialize<VacuumTable_Parameters>(jsonString)!;
                        parameters_actual = parameters_load;

                        VacuumTable_Generator generator = new VacuumTable_Generator();

                        Object[] config = new object[2];
                        config[0] = parameters_actual;
                        config[1] = fileName_Save;

                        generator.configure(config);
                        generator.generate();
                    }
                }
                catch
                {
                    //Write out default parameters
                    {
                        string fileName = "BoxParameter.json";
                        VacuumTable_Parameters parameters_save = new VacuumTable_Parameters();
                        var options = new JsonSerializerOptions { WriteIndented = true };
                        string jsonString = JsonSerializer.Serialize(parameters_save, options);

                        File.WriteAllText(fileName, jsonString);
                        parameters_actual = parameters_save;
                    }
                }
            }
        }

        private static void WriteOutDefaultParameters()
        {
            //Write out default parameters
            try
            {
                string fileName = "VacuumTable_Parameter_default.json";
                VacuumTable_Parameters parameters_save = new VacuumTable_Parameters();
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(parameters_save, options);

                File.WriteAllText(fileName, jsonString);
            }
            catch
            {

            }
        }
    }
}