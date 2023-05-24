using System.Linq;
using Mods;
using UnityEngine;

public class Tests : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!ModsManager.Instance.EnumerateAllMods().Any())
            {
                print("======= NO MOD DISCOVERED =======");
            }
            else
            {
                print("======= DISCOVERED MODS: =======");
                foreach (var mod in ModsManager.Instance.EnumerateAllMods())
                {
                    print(mod.ToString());
                }
                print("==============================");
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ModsManager.Instance.GetMod("BaseGame")?.SetModEnabled(false);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ModsManager.Instance.GetMod("BaseGame")?.SetModEnabled(true);
        }
			
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ModsManager.Instance.DiscoverMods();
        }	
        
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            print(ModsManager.Instance.GetMod("BaseGame")?.TryGet("HelloTest"));
        }
    }
    
}
