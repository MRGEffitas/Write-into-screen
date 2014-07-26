using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using WindowsInput;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.IO.Compression;

namespace write_into_screen
{
    class SimulateUser
    {

        [STAThread]

        public static void ReadAndApplyConfig(String path)
        {
            //read configuration
            
            StreamReader re;
            try { 
             re = File.OpenText(path);
            }
            catch(System.IO.FileNotFoundException e){
                Console.WriteLine("config.json is missing, recheck your config file!");
                return;
            }

            JsonTextReader reader = new JsonTextReader(re);
            JArray root = null;

            try {
            root = JArray.Load(reader);
            }
            
            catch(   Newtonsoft.Json.JsonReaderException e)
            {
                Console.WriteLine("Incorrect JSON syntax, recheck your config file!");
                return;
            }

            // apply commands based on configuration
            foreach (JObject o in root)
            {
                // normal text to be typed, with RETURN at the end
                if (o["text_type"] != null)
                {
                    InputSimulator.SimulateTextEntry((string)o["text_type"]);
                    InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);
                }

                // normal text to be typed, without RETURN at the end
                if (o["text_type_without_return"] != null)
                {
                    InputSimulator.SimulateTextEntry((string)o["text_type_without_return"]);
                    InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);
                }

                // sleep for this amount of miliseconds
                if (o["sleep"] != null)
                {
                    //Console.WriteLine("Sleep " + o["sleep"]);
                    Thread.Sleep((int)o["sleep"]);
                }

                // read this file, convert to base64 and paste into clipboard
                if (o["file_base64_into_clipboard"] != null)
                {
                    FileStream fs = new FileStream((string)o["file_base64_into_clipboard"],
                                           FileMode.Open,
                                           FileAccess.Read);
                    byte[] filebytes = new byte[fs.Length];
                    fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
                    string encodedData =
                        Convert.ToBase64String(filebytes,
                                               Base64FormattingOptions.InsertLineBreaks);

                    System.Windows.Forms.Clipboard.SetText(encodedData);

                }

                // zip the contents of the directory, and put it into dropper.zip, one directory aboves
                if (o["zip_directory"] != null) {
                    string startPath = (string)o["zip_directory"];
                    string zipPath = startPath + "\\..\\dropper.zip";              

                    ZipFile.CreateFromDirectory(startPath, zipPath);
                }

                // read file into clipboard 
                if (o["file_into_clipboard"] != null)
                {
                    FileStream fs = new FileStream((string)o["file_into_clipboard"],
                                           FileMode.Open,
                                           FileAccess.Read);
                    byte[] filebytes = new byte[fs.Length];
                    fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
                    System.Windows.Forms.Clipboard.SetText(System.Text.Encoding.UTF8.GetString(filebytes));

                }

                // read (binary) file, convert into hex, paste into clipboard
                if (o["file_into_hex_into_clipboard"] != null)
                {
                    FileStream fs = new FileStream((string)o["file_into_hex_into_clipboard"], FileMode.Open, FileAccess.Read, FileShare.None);
                    BinaryReader reader2 = new BinaryReader(fs);
                    reader2.BaseStream.Position = 0x0;     // The offset you are reading the data from
                    byte[] data = reader2.ReadBytes(Convert.ToInt32(fs.Length)); // Read 16 bytes into an array
                    reader2.Close();

                    string data_as_hex = BitConverter.ToString(data);
                    System.Windows.Forms.Clipboard.SetText(data_as_hex.Replace("-",""));
                }

                // simulate the press of 1 key
                if (o["key_combo1"] != null)
                {
                    
                    InputSimulator.SimulateKeyPress((VirtualKeyCode)System.Enum.Parse(typeof(VirtualKeyCode), (string)o["key_combo1"][0]["name"]));

                }

                // simulate the simultan press of two keys, e.g. ctrl + v
                if (o["key_combo2"] != null)
                {
                    
                        InputSimulator.SimulateModifiedKeyStroke((VirtualKeyCode)System.Enum.Parse(typeof(VirtualKeyCode), (string)o["key_combo2"][0]["name"]),
                            (VirtualKeyCode)System.Enum.Parse(typeof(VirtualKeyCode), (string)o["key_combo2"][1]["name"]));                    

                }

                // simulate the simultan press of three keys, e.g. ctrl + (shift + v)
                if (o["key_combo3"] != null)
                {
                    InputSimulator.SimulateModifiedKeyStroke(
                            (VirtualKeyCode)System.Enum.Parse(typeof(VirtualKeyCode), (string)o["key_combo3"][0]["name"]),
                            new[] {(VirtualKeyCode)System.Enum.Parse(typeof(VirtualKeyCode), (string)o["key_combo3"][1]["name"])
                            ,(VirtualKeyCode)System.Enum.Parse(typeof(VirtualKeyCode), (string)o["key_combo3"][2]["name"])});
                }
                // simulate the simultan press of three keys, e.g. (ctrl + shift) + v
                if (o["key_combo3_2"] != null)
                {
                    InputSimulator.SimulateModifiedKeyStroke(
                            new[] {(VirtualKeyCode)System.Enum.Parse(typeof(VirtualKeyCode), (string)o["key_combo3_2"][0]["name"])
                            ,(VirtualKeyCode)System.Enum.Parse(typeof(VirtualKeyCode), (string)o["key_combo3_2"][1]["name"])},
                            (VirtualKeyCode)System.Enum.Parse(typeof(VirtualKeyCode), (string)o["key_combo3_2"][2]["name"])      );
                }
            }

        }
    }
}
