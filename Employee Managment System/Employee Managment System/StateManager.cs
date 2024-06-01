using Newtonsoft.Json;
using System;
using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Employee_Managment_System

{
    public class StateManager : Form
    {
        public static void SaveState(Dictionary<string, object?> state)
        {
            string json = JsonConvert.SerializeObject(state, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });
            File.WriteAllText("saves/data.json", json);
        }

        public static Dictionary<string, object?> LoadState()
        {
            string json = File.ReadAllText("saves/data.json");
            return JsonConvert.DeserializeObject<Dictionary<string, object?>>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });
        }
    }
}
