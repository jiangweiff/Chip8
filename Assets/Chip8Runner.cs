using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chip8Runner : MonoBehaviour
{
    Chip8 emu = new Chip8();
    public Transform cube;
    Transform[] screenpixels;
    Dictionary<KeyCode, int> keyMapper = new Dictionary<KeyCode, int>();
    // Start is called before the first frame update
    void Start()
    {
        emu.Initialize();        
        var fn = EditorUtility.OpenFilePanel("open rom", "", "");
        emu.LoadGame(fn);

        // initialize screen;
        screenpixels = new Transform[64*32];
        for(int x = 0; x < 64; ++x) {
            for (int y = 0; y < 32; ++y) {
                screenpixels[y*64+x] = GameObject.Instantiate(cube);
                screenpixels[y*64+x].transform.position = new Vector3(x-32, -(y-16), 0);
            }
        }
        cube.gameObject.SetActive(false);

        // keymapper
        keyMapper[KeyCode.Alpha1] = 0x1;
        keyMapper[KeyCode.Alpha2] = 0x2;
        keyMapper[KeyCode.Alpha3] = 0x3;
        keyMapper[KeyCode.Alpha4] = 0xC;

        keyMapper[KeyCode.Q] = 0x4;
        keyMapper[KeyCode.W] = 0x5;
        keyMapper[KeyCode.E] = 0x6;
        keyMapper[KeyCode.R] = 0xD;

        keyMapper[KeyCode.A] = 0x7;
        keyMapper[KeyCode.S] = 0x8;
        keyMapper[KeyCode.D] = 0x9;
        keyMapper[KeyCode.F] = 0xE;

        keyMapper[KeyCode.Z] = 0xA;
        keyMapper[KeyCode.X] = 0x0;
        keyMapper[KeyCode.C] = 0xB;
        keyMapper[KeyCode.V] = 0xF;
    }

    // Update is called once per frame
    void Update()
    {
        if (emu.drawFlag) {
            for (int i = 0; i < 64*32; ++i) {
                screenpixels[i].gameObject.SetActive(emu.gfx[i] == 1);
            }
            emu.drawFlag = false;
        }

        UpdateInput();        
    }

    void UpdateInput()
    {
        foreach(var kv in keyMapper) {
            emu.key[kv.Value] = (byte)(Input.GetKey(kv.Key) ? 1 : 0);
        }
    }

    void FixedUpdate()
    {
        emu.EmulateCycle();
    }
}
