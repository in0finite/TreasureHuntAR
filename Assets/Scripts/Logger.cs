using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace TreasureHunt
{

    public class Logger : MonoBehaviour
    {
        StreamWriter m_outputFile;


        void Awake()
        {
            if (Application.isMobilePlatform)
            {
                m_outputFile = new StreamWriter(Path.Combine(Application.persistentDataPath, $"TreasureHuntLog{DateTime.Now.Ticks}.txt"));
                Application.logMessageReceivedThreaded += this.OnLog;
            }
        }

        void OnLog(string condition, string stackTrace, LogType type)
        {
            m_outputFile.WriteLine(condition + "\n" + stackTrace + "\n\n\n");
            m_outputFile.Flush();
        }

    }

}
