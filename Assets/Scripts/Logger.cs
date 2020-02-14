using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections.Concurrent;

namespace TreasureHunt
{

    public class Logger : MonoBehaviour
    {
        StreamWriter m_outputFile;

        public static ConcurrentStack<string> logMessages = new ConcurrentStack<string>();


        void Awake()
        {
            if (Application.isMobilePlatform)
            {
                m_outputFile = new StreamWriter(Path.Combine(Application.persistentDataPath, $"TreasureHuntLog{DateTime.Now:HH-mm-ss-tt-zz-yyyy-M-d-dddd}.txt"));
                Application.logMessageReceivedThreaded += this.OnLog;
            }
        }

        void OnLog(string condition, string stackTrace, LogType type)
        {
            m_outputFile.WriteLine(condition + "\n" + stackTrace + "\n\n\n");
            m_outputFile.Flush();

            logMessages.Push(condition);
            
            if (logMessages.Count > 500)
                logMessages.Clear();

        }

    }

}
